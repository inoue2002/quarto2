using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class YoukanPutPieceAlgorithm : PutPieceAlgorithm
{
    public override Position putPiece(Board board)
    {
        try
        {
            Debug.Log("=== Youkan駒配置開始 ===");
            
            // 今から設置しようとしているピース
            PieceId nextPutPeaceId = board.getSelectedPieceId();
            List<PieceId> availablePieces = board.getSelectablePieces();
            
            Debug.Log($"配置駒: {nextPutPeaceId}, 残り駒数: {availablePieces.Count}");
            
            // まず即座に勝てる手があるかチェック
            Position winPos = FindWinningPosition(board, nextPutPeaceId);
            if (winPos.X != -1 && winPos.Y != -1)
            {
                Debug.Log($"即勝手を発見: ({winPos.X}, {winPos.Y})");
                return winPos;
            }
            
            // 残りコマ数による戦略切り替え
            if (availablePieces.Count <= 8) // FULL_SEARCH_THRESHOLDと同じ値
            {
                Debug.Log($"残り駒数 {availablePieces.Count} - 全探索モード");
                int bestPosition = YoukanSelectPieceAlgorithm.FindBestPositionWithSearch(
                    board.getState(), 
                    nextPutPeaceId, 
                    availablePieces, 
                    6 // 探索深度
                );
                
                if (bestPosition != -1)
                {
                    Position result = new Position(bestPosition % 4, bestPosition / 4);
                    Debug.Log($"全探索結果: ({result.X}, {result.Y})");
                    return result;
                }
            }
            else
            {
                Debug.Log($"残り駒数 {availablePieces.Count} - 軽量配置モード");
                Position lightSearchResult = FindSafePositionWithLightSearch(board, nextPutPeaceId);
                Debug.Log($"軽量配置結果: ({lightSearchResult.X}, {lightSearchResult.Y})");
                return lightSearchResult;
            }
            
            // フォールバック: ランダム選択
            Debug.Log("フォールバック: ランダム配置");
            return GetRandomEmptyPosition(board);
        }
        catch (Exception e)
        {
            Debug.LogError($"YoukanPutPiece でエラー発生: {e.Message}");
            Debug.LogError($"スタックトレース: {e.StackTrace}");
            
            // エラー時のフォールバック処理
            try
            {
                Position fallbackPos = GetRandomEmptyPosition(board);
                Debug.Log($"エラー時フォールバック位置: ({fallbackPos.X}, {fallbackPos.Y})");
                return fallbackPos;
            }
            catch (Exception fallbackError)
            {
                Debug.LogError($"フォールバック処理もエラー: {fallbackError.Message}");
                return new Position(0, 0); // 最終フォールバック
            }
        }
    }
    
    /// <summary>
    /// 即座に勝利できる位置を探す（高速チェック）
    /// </summary>
    private Position FindWinningPosition(Board board, PieceId pieceId)
    {
        Piece[] state = board.getState();
        List<int> emptyPositions = YoukanSelectPieceAlgorithm.GetEmptyPositions(state);
        
        foreach (int position in emptyPositions)
        {
            Piece[] testState = YoukanSelectPieceAlgorithm.PlacePieceOnBoard(state, pieceId, position);
            if (YoukanSelectPieceAlgorithm.JudgeWinner(testState) != PlayerId.None)
            {
                return new Position(position % 4, position / 4);
            }
        }
        
        return new Position(-1, -1);
    }
    
    /// <summary>
    /// 軽量な安全配置探索（残りコマが多い時用）
    /// </summary>
    private Position FindSafePositionWithLightSearch(Board board, PieceId pieceId)
    {
        try
        {
            Debug.Log("軽量配置探索開始 - 危険位置の除外");
            Piece[] state = board.getState();
            List<int> emptyPositions = YoukanSelectPieceAlgorithm.GetEmptyPositions(state);
            
            // 相手にリーチを与えない位置を探す
            List<int> safePositions = new List<int>();
            
            foreach (int position in emptyPositions)
            {
                if (!DoesPositionCreateOpponentThreat(state, pieceId, position))
                {
                    safePositions.Add(position);
                }
            }
            
            Debug.Log($"安全配置候補数: {safePositions.Count} / {emptyPositions.Count}");
            
            // 安全な位置がある場合はランダム選択
            if (safePositions.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, safePositions.Count);
                int chosenPosition = safePositions[randomIndex];
                return new Position(chosenPosition % 4, chosenPosition / 4);
            }
            else
            {
                Debug.Log("安全な位置なし - 全位置からランダム選択");
                // 安全な位置がない場合は全位置からランダム
                int randomIndex = UnityEngine.Random.Range(0, emptyPositions.Count);
                int chosenPosition = emptyPositions[randomIndex];
                return new Position(chosenPosition % 4, chosenPosition / 4);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"軽量配置探索でエラー: {e.Message}");
            // エラー時はランダム配置にフォールバック
            return GetRandomEmptyPosition(board);
        }
    }
    
    /// <summary>
    /// 指定位置に駒を置くことで相手に脅威を与えてしまうかチェック
    /// </summary>
    private bool DoesPositionCreateOpponentThreat(Piece[] state, PieceId pieceId, int position)
    {
        try
        {
            // この位置に駒を置いた後の盤面をシミュレート
            Piece[] testState = YoukanSelectPieceAlgorithm.PlacePieceOnBoard(state, pieceId, position);
            
            // 相手にとって非常に有利な盤面になるかチェック
            return IsBoardDangerousForUs(testState);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"脅威チェックでエラー: {e.Message}");
            return false; // エラー時は安全と判定
        }
    }
    
    /// <summary>
    /// 盤面が我々にとって危険かどうか判定
    /// </summary>
    private bool IsBoardDangerousForUs(Piece[] state)
    {
        // 相手のリーチライン数をカウント
        int[][] lines = {
            new int[] {0, 1, 2, 3}, new int[] {4, 5, 6, 7}, new int[] {8, 9, 10, 11}, new int[] {12, 13, 14, 15},
            new int[] {0, 4, 8, 12}, new int[] {1, 5, 9, 13}, new int[] {2, 6, 10, 14}, new int[] {3, 7, 11, 15},
            new int[] {0, 5, 10, 15}, new int[] {3, 6, 9, 12}
        };
        
        int threatLines = 0;
        foreach (int[] line in lines)
        {
            if (IsLineThreateningToUs(state, line))
            {
                threatLines++;
            }
        }
        
        // 複数の脅威ラインがある場合は危険
        return threatLines >= 2;
    }
    
    /// <summary>
    /// ラインが我々にとって脅威かどうかチェック（3つ埋まって共通属性あり）
    /// </summary>
    private bool IsLineThreateningToUs(Piece[] state, int[] line)
    {
        List<Piece> pieces = new List<Piece>();
        int emptyCount = 0;
        
        for (int i = 0; i < 4; i++)
        {
            if (state[line[i]] == null)
            {
                emptyCount++;
            }
            else
            {
                pieces.Add(state[line[i]]);
            }
        }
        
        // 空きマス1つで3つの駒に共通属性がある場合は脅威
        if (emptyCount == 1 && pieces.Count == 3)
        {
            return YoukanSelectPieceAlgorithm.CanCompleteQuarto(pieces);
        }
        
        return false;
    }

    /// <summary>
    /// 盤面の全てのマスをチェックし、指定した条件を満たす最初の場所を返す
    /// 条件はFunc<Board, bool>で指定し、仮想的にピースを置いた後のBoardを渡す
    /// </summary>
    /// <param name="board">現在の盤面</param>
    /// <param name="pieceId">置くピースID</param>
    /// <param name="condition">条件（仮想盤面を受け取りboolを返す）</param>
    /// <returns>条件を満たす最初のPosition。なければ(-1,-1)</returns>
    private Position FindFirstPosition(Board board, PieceId pieceId, Func<Board, bool> condition)
    {
        // 盤面の全てのマスをチェック
        for (int i = 0; i < 16; i++)
        {
            // そのマスが空いているか確認
            if (board.getstate()[i] == null)
            {
                // 仮のボードを作成（ディープコピー）
                Board tempBoard = new Board();
                // 盤面の状態をコピー
                Piece[] currentState = board.getState();
                for (int j = 0; j < 16; j++)
                {
                    if (currentState[j] != null)
                    {
                        tempBoard.putPiece(currentState[j].getPieceId(), new Position(j % 4, j / 4));
                    }
                }
                // 選択可能な駒リストもコピー
                foreach (PieceId pid in board.getSelectablePieces())
                {
                    if (pid != pieceId)
                    {
                        // 既にputPieceで消費されていないものだけ追加
                        tempBoard.getSelectablePieces().Add(pid);
                    }
                }
                // 選択中のピースをセット
                tempBoard.setSelectedPiece(pieceId);

                // 仮のボードでピースを置く
                tempBoard.putPiece(pieceId, new Position(i % 4, i / 4));

                // 条件判定
                if (condition(tempBoard))
                {
                    return new Position(i % 4, i / 4);
                }
            }
        }
        return new Position(-1, -1);
    }

    /// <summary>
    /// 空いているマスの中からランダムに1つ選んで返す
    /// </summary>
    /// <param name="board">現在の盤面</param>
    /// <returns>ランダムな空きマスのPosition。なければ(-1,-1)</returns>
    private Position GetRandomEmptyPosition(Board board)
    {
        List<Position> emptyPositions = new List<Position>();
        for (int i = 0; i < board.getstate().Length; i++)
        {
            if (board.getstate()[i] == null)
            {
                emptyPositions.Add(new Position(i % 4, i / 4));
            }
        }
        if (emptyPositions.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, emptyPositions.Count);
            return emptyPositions[randomIndex];
        }
        return new Position(-1, -1);
    }
}