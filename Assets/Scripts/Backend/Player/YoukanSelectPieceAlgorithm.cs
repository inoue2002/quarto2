using System;
using System.Collections.Generic;
using UnityEngine;

public class YoukanSelectPieceAlgorithm : SelectPieceAlgorithm
{
    // === 全探索用の共通設定 ===
    private const int MAX_DEPTH = 8; // 探索の最大深度
    private const int WIN_SCORE = 10000; // 勝利時のスコア
    private const int LOSE_SCORE = -10000; // 敗北時のスコア
    private const int FULL_SEARCH_THRESHOLD = 8; // 残りコマ数がこの値以下で全探索開始
    
    // === ゲーム状態を表すクラス ===
    public class GameState
    {
        public Piece[] board;
        public List<PieceId> availablePieces;
        public PieceId selectedPiece;
        public PlayerId currentPlayer;
        public bool isSelectPhase; // true: 駒選択フェーズ, false: 駒配置フェーズ
        
        public GameState(Piece[] boardState, List<PieceId> available, PieceId selected, PlayerId player, bool selectPhase)
        {
            board = new Piece[16];
            Array.Copy(boardState, board, 16);
            availablePieces = new List<PieceId>(available);
            selectedPiece = selected;
            currentPlayer = player;
            isSelectPhase = selectPhase;
        }
        
        public GameState DeepCopy()
        {
            return new GameState(board, availablePieces, selectedPiece, currentPlayer, isSelectPhase);
        }
    }
    public override PieceId SelectPiece(Piece[] state)
    {
        try
        {
            Debug.Log("=== Youkan駒選択開始 ===");
            
            // 盤面に存在しない（未配置）の駒IDリストを返す
            List<PieceId> selectablePieces = GetSelectablePieces(state);
            Debug.Log($"選択可能駒数: {selectablePieces.Count}");
            
            if (selectablePieces.Count == 0)
            {
                Debug.LogError("選択可能な駒がありません！");
                return PieceId.FSCB; // フォールバック
            }
            
            // 残りコマ数による戦略切り替え
            if (selectablePieces.Count <= FULL_SEARCH_THRESHOLD)
            {
                Debug.Log($"残りコマ数 {selectablePieces.Count} - 全探索モード開始");
                PieceId bestPiece = FindBestPieceWithSearch(state, selectablePieces, 6);
                Debug.Log($"全探索結果: {bestPiece}");
                return bestPiece;
            }
            else
            {
                Debug.Log($"残りコマ数 {selectablePieces.Count} - 軽量探索モード");
                PieceId safePiece = FindSafePieceWithLightSearch(state, selectablePieces);
                Debug.Log($"軽量探索結果: {safePiece}");
                return safePiece;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"YoukanSelectPiece でエラー発生: {e.Message}");
            Debug.LogError($"スタックトレース: {e.StackTrace}");
            
            // エラー時のフォールバック処理
            List<PieceId> fallbackPieces = GetSelectablePieces(state);
            if (fallbackPieces.Count > 0)
            {
                PieceId randomPiece = fallbackPieces[UnityEngine.Random.Range(0, fallbackPieces.Count)];
                Debug.Log($"エラー時フォールバック駒: {randomPiece}");
                return randomPiece;
            }
            
            Debug.LogError("完全にフォールバック失敗 - デフォルト駒を返します");
            return PieceId.FSCB;
        }
    }

    /// <summary>
    /// 盤面に存在しない（未配置）の駒IDリストを返す
    /// </summary>
    private List<PieceId> GetSelectablePieces(Piece[] state)
    {
        PieceId[] allPieceIds = {
            PieceId.FSCB, PieceId.FSCW, PieceId.FSSB, PieceId.FSSW,
            PieceId.FTCB, PieceId.FTCW, PieceId.FTSB, PieceId.FTSW,
            PieceId.HSCB, PieceId.HSCW, PieceId.HSSB, PieceId.HSSW,
            PieceId.HTCB, PieceId.HTCW, PieceId.HTSB, PieceId.HTSW
        };

        List<PieceId> selectablePieces = new List<PieceId>();
        foreach (PieceId pid in allPieceIds)
        {
            if (IsPieceAvailable(state, pid))
            {
                selectablePieces.Add(pid);
            }
        }
        return selectablePieces;
    }

    /// <summary>
    /// 指定した駒IDが盤面に未配置かどうか判定
    /// </summary>
    private bool IsPieceAvailable(Piece[] state, PieceId pid)
    {
        for (int j = 0; j < state.Length; j++)
        {
            if (state[j] != null && state[j].getPieceId() == pid)
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 1手でクリアされてしまう駒を除外した安全な駒リストを返す
    /// </summary>
    private List<PieceId> GetSafePieces(Piece[] state, List<PieceId> selectablePieces)
    {
        List<PieceId> safePieces = new List<PieceId>();
        foreach (PieceId candidate in selectablePieces)
        {
            if (!CanWinInOneMove(state, candidate))
            {
                safePieces.Add(candidate);
            }
        }
        return safePieces;
    }

    /// <summary>
    /// 指定した駒を相手に渡したとき、相手が1手で勝てる場所があるかどうか判定
    /// </summary>
    private bool CanWinInOneMove(Piece[] state, PieceId candidate)
    {
        // 相手にこの駒を渡した後、相手がどこかに置いて勝てるかチェック
        for (int i = 0; i < 16; i++)
        {
            if (state[i] == null)
            {
                // この位置に駒を置いた時の盤面を作成
                Piece[] testState = PlacePieceOnBoard(state, candidate, i);
                PlayerId winner = JudgeWinner(testState);
                if (winner != PlayerId.None)
                {
                    Debug.Log($"危険駒発見: {candidate} を位置({i % 4}, {i / 4})に置くと相手が勝利");
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// 盤面状態から新しいBoardインスタンスを作成
    /// </summary>
    private Board CreateBoardFromState(Piece[] state)
    {
        Board board = new Board();
        for (int i = 0; i < state.Length; i++)
        {
            if (state[i] != null)
            {
                board.putPiece(state[i].getPieceId(), new Position(i % 4, i / 4));
            }
        }
        return board;
    }

    /// <summary>
    /// 候補リストからランダムに1つ選んで返す
    /// </summary>
    private PieceId SelectRandomPiece(List<PieceId> candidates, bool hasSafe)
    {
        if (candidates.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, candidates.Count);
            PieceId selected = candidates[randomIndex];
            Debug.Log("CPUが駒を選択: " + selected + (hasSafe ? "" : "（安全駒なし）"));
            return selected;
        }
        Debug.LogError("選択可能な駒がありません");
        return 0;
    }
    
    // === 全探索用の共通ライブラリ関数 ===
    
    /// <summary>
    /// 盤面状態から勝者を判定（共通ライブラリ）
    /// </summary>
    public static PlayerId JudgeWinner(Piece[] state)
    {
        // 勝利条件の定義（縦・横・斜めのライン）
        int[][] lines = {
            new int[] {0, 1, 2, 3}, new int[] {4, 5, 6, 7}, new int[] {8, 9, 10, 11}, new int[] {12, 13, 14, 15}, // 横
            new int[] {0, 4, 8, 12}, new int[] {1, 5, 9, 13}, new int[] {2, 6, 10, 14}, new int[] {3, 7, 11, 15}, // 縦
            new int[] {0, 5, 10, 15}, new int[] {3, 6, 9, 12} // 斜め
        };
        
        foreach (int[] line in lines)
        {
            // ラインの4つのマスが全て埋まっているかチェック
            bool allFilled = true;
            for (int i = 0; i < 4; i++)
            {
                if (state[line[i]] == null)
                {
                    allFilled = false;
                    break;
                }
            }
            
            if (allFilled)
            {
                // 4つの駒で共通する属性があるかチェック
                if (HasCommonAttribute(state[line[0]], state[line[1]], state[line[2]], state[line[3]]))
                {
                    return PlayerId.Player1; // 勝者がいる（現在のプレイヤーに関係なく勝利扱い）
                }
            }
        }
        return PlayerId.None;
    }
    
    /// <summary>
    /// 4つの駒に共通する属性があるかチェック
    /// </summary>
    public static bool HasCommonAttribute(Piece p1, Piece p2, Piece p3, Piece p4)
    {
        if (p1 == null || p2 == null || p3 == null || p4 == null) return false;
        
        int id1 = (int)p1.getPieceId();
        int id2 = (int)p2.getPieceId();
        int id3 = (int)p3.getPieceId();
        int id4 = (int)p4.getPieceId();
        
        // 各属性ビットで AND と OR を取って、共通属性をチェック
        for (int bit = 0; bit < 4; bit++)
        {
            int mask = 1 << bit;
            int and_result = (id1 & mask) & (id2 & mask) & (id3 & mask) & (id4 & mask);
            int or_result = (id1 & mask) | (id2 & mask) | (id3 & mask) | (id4 & mask);
            
            // 全て同じ属性（全て0または全て1）の場合
            if (and_result == mask || or_result == 0)
            {
                return true;
            }
        }
        return false;
    }
    
    /// <summary>
    /// 盤面の評価値を計算（共通ライブラリ）
    /// 正の値ほど有利、負の値ほど不利
    /// </summary>
    public static int EvaluateBoard(Piece[] state, PlayerId currentPlayer)
    {
        PlayerId winner = JudgeWinner(state);
        if (winner != PlayerId.None)
        {
            return currentPlayer == PlayerId.Player1 ? WIN_SCORE : LOSE_SCORE;
        }
        
        int score = 0;
        
        // 各ラインの危険度を評価
        int[][] lines = {
            new int[] {0, 1, 2, 3}, new int[] {4, 5, 6, 7}, new int[] {8, 9, 10, 11}, new int[] {12, 13, 14, 15},
            new int[] {0, 4, 8, 12}, new int[] {1, 5, 9, 13}, new int[] {2, 6, 10, 14}, new int[] {3, 7, 11, 15},
            new int[] {0, 5, 10, 15}, new int[] {3, 6, 9, 12}
        };
        
        foreach (int[] line in lines)
        {
            score += EvaluateLine(state, line);
        }
        
        return score;
    }
    
    /// <summary>
    /// 1つのラインの評価値を計算
    /// </summary>
    public static int EvaluateLine(Piece[] state, int[] line)
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
        
        // 空きマスが0の場合（ライン完成）
        if (emptyCount == 0)
        {
            return HasCommonAttribute(pieces[0], pieces[1], pieces[2], pieces[3]) ? WIN_SCORE : 0;
        }
        
        // 空きマスが1つの場合（リーチ状態）
        if (emptyCount == 1 && pieces.Count == 3)
        {
            return CanCompleteQuarto(pieces) ? 500 : -100;
        }
        
        // 空きマスが2つの場合
        if (emptyCount == 2 && pieces.Count == 2)
        {
            return HasPotentialQuarto(pieces) ? 50 : -10;
        }
        
        return 0;
    }
    
    /// <summary>
    /// 3つの駒でクアルトが完成可能かチェック
    /// </summary>
    public static bool CanCompleteQuarto(List<Piece> pieces)
    {
        if (pieces.Count != 3) return false;
        
        int id1 = (int)pieces[0].getPieceId();
        int id2 = (int)pieces[1].getPieceId();
        int id3 = (int)pieces[2].getPieceId();
        
        // 3つの駒で共通する属性があるかチェック
        for (int bit = 0; bit < 4; bit++)
        {
            int mask = 1 << bit;
            int and_result = (id1 & mask) & (id2 & mask) & (id3 & mask);
            int or_result = (id1 & mask) | (id2 & mask) | (id3 & mask);
            
            if (and_result == mask || or_result == 0)
            {
                return true;
            }
        }
        return false;
    }
    
    /// <summary>
    /// 2つの駒でクアルトの可能性があるかチェック
    /// </summary>
    public static bool HasPotentialQuarto(List<Piece> pieces)
    {
        if (pieces.Count != 2) return false;
        
        int id1 = (int)pieces[0].getPieceId();
        int id2 = (int)pieces[1].getPieceId();
        
        // 2つの駒で共通する属性があるかチェック
        for (int bit = 0; bit < 4; bit++)
        {
            int mask = 1 << bit;
            int and_result = (id1 & mask) & (id2 & mask);
            int or_result = (id1 & mask) | (id2 & mask);
            
            if (and_result == mask || or_result == 0)
            {
                return true;
            }
        }
        return false;
    }
    
    /// <summary>
    /// 指定した位置が空いているかチェック（共通ライブラリ）
    /// </summary>
    public static bool IsPositionEmpty(Piece[] state, int position)
    {
        return position >= 0 && position < 16 && state[position] == null;
    }
    
    /// <summary>
    /// 空いている位置のリストを取得（共通ライブラリ）
    /// </summary>
    public static List<int> GetEmptyPositions(Piece[] state)
    {
        List<int> emptyPositions = new List<int>();
        for (int i = 0; i < 16; i++)
        {
            if (state[i] == null)
            {
                emptyPositions.Add(i);
            }
        }
        return emptyPositions;
    }
    
    /// <summary>
    /// 盤面に駒を配置して新しい盤面を作成（共通ライブラリ）
    /// </summary>
    public static Piece[] PlacePieceOnBoard(Piece[] state, PieceId pieceId, int position)
    {
        Piece[] newState = new Piece[16];
        Array.Copy(state, newState, 16);
        newState[position] = new Piece(pieceId);
        return newState;
    }
    
    /// <summary>
    /// 利用可能な駒リストから指定した駒を除去した新しいリストを作成（共通ライブラリ）
    /// </summary>
    public static List<PieceId> RemovePieceFromAvailable(List<PieceId> availablePieces, PieceId pieceId)
    {
        List<PieceId> newList = new List<PieceId>(availablePieces);
        newList.Remove(pieceId);
        return newList;
    }
    
    // === 軽量探索（防御専用）の実装 ===
    
    /// <summary>
    /// 軽量探索で安全な駒を選択（残りコマが多い時用）
    /// </summary>
    private PieceId FindSafePieceWithLightSearch(Piece[] state, List<PieceId> selectablePieces)
    {
        try
        {
            Debug.Log("軽量探索開始 - 危険駒の除外処理");
            
            // 1手でクリアされてしまう駒を除外した安全な駒リストを返す
            List<PieceId> safePieces = GetSafePieces(state, selectablePieces);
            Debug.Log($"安全駒候補数: {safePieces.Count}");
            
            List<PieceId> finalCandidates = safePieces.Count > 0 ? safePieces : selectablePieces;
            
            // より高度な防御チェック：2手先まで軽く見る
            if (finalCandidates.Count > 1)
            {
                List<PieceId> extraSafePieces = GetExtraSafePieces(state, finalCandidates);
                if (extraSafePieces.Count > 0)
                {
                    finalCandidates = extraSafePieces;
                    Debug.Log($"2手先チェック後の安全駒数: {finalCandidates.Count}");
                }
            }
            
            return SelectRandomPiece(finalCandidates, safePieces.Count > 0);
        }
        catch (Exception e)
        {
            Debug.LogError($"軽量探索でエラー: {e.Message}");
            // エラー時は元の安全駒選択にフォールバック
            List<PieceId> safePieces = GetSafePieces(state, selectablePieces);
            List<PieceId> finalCandidates = safePieces.Count > 0 ? safePieces : selectablePieces;
            return SelectRandomPiece(finalCandidates, safePieces.Count > 0);
        }
    }
    
    /// <summary>
    /// より高度な安全性チェック（2手先まで軽く探索）
    /// </summary>
    private List<PieceId> GetExtraSafePieces(Piece[] state, List<PieceId> candidates)
    {
        List<PieceId> extraSafePieces = new List<PieceId>();
        
        foreach (PieceId candidate in candidates)
        {
            if (!IsVulnerableInTwoMoves(state, candidate))
            {
                extraSafePieces.Add(candidate);
            }
        }
        
        return extraSafePieces;
    }
    
    /// <summary>
    /// 2手先で相手に有利になりすぎないかチェック
    /// </summary>
    private bool IsVulnerableInTwoMoves(Piece[] state, PieceId candidate)
    {
        try
        {
            // この駒を渡した後、相手がどこに置いても2手先で危険になるかチェック
            List<int> emptyPositions = GetEmptyPositions(state);
            int dangerousPositions = 0;
            
            foreach (int position in emptyPositions)
            {
                Piece[] testState = PlacePieceOnBoard(state, candidate, position);
                
                // この盤面から相手にとって非常に有利な状況になるかチェック
                if (IsBoardTooFavorableForOpponent(testState))
                {
                    dangerousPositions++;
                }
            }
            
            // 半分以上の場所で危険な場合は脆弱と判定
            return dangerousPositions > emptyPositions.Count / 2;
        }
        catch (Exception e)
        {
            Debug.LogWarning($"2手先チェックでエラー: {e.Message}");
            return false; // エラー時は安全と判定
        }
    }
    
    /// <summary>
    /// 盤面が相手にとって非常に有利かどうか判定
    /// </summary>
    private bool IsBoardTooFavorableForOpponent(Piece[] state)
    {
        // 簡易評価：複数のリーチラインがあるかチェック
        int[][] lines = {
            new int[] {0, 1, 2, 3}, new int[] {4, 5, 6, 7}, new int[] {8, 9, 10, 11}, new int[] {12, 13, 14, 15},
            new int[] {0, 4, 8, 12}, new int[] {1, 5, 9, 13}, new int[] {2, 6, 10, 14}, new int[] {3, 7, 11, 15},
            new int[] {0, 5, 10, 15}, new int[] {3, 6, 9, 12}
        };
        
        int reachLines = 0;
        foreach (int[] line in lines)
        {
            if (IsLineNearComplete(state, line))
            {
                reachLines++;
            }
        }
        
        // 複数のリーチラインがある場合は危険
        return reachLines >= 2;
    }
    
    /// <summary>
    /// ラインが完成に近いかチェック（3つ埋まっていて共通属性あり）
    /// </summary>
    private bool IsLineNearComplete(Piece[] state, int[] line)
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
        
        // 空きマスが1つで、3つの駒に共通属性がある場合
        return emptyCount == 1 && pieces.Count == 3 && CanCompleteQuarto(pieces);
    }
    
    // === ミニマックス・アルファベータ探索の実装 ===
    
    /// <summary>
    /// アルファベータ探索で最適な駒選択を行う
    /// </summary>
    public static PieceId FindBestPieceWithSearch(Piece[] state, List<PieceId> availablePieces, int depth = 6)
    {
        try
        {
            Debug.Log($"駒選択全探索開始 - 深度: {depth}, 候補数: {availablePieces.Count}");
            
            int bestScore = int.MinValue;
            PieceId bestPiece = availablePieces[0];
            int evaluatedCount = 0;
            
            foreach (PieceId candidate in availablePieces)
            {
                evaluatedCount++;
                Debug.Log($"駒評価 {evaluatedCount}/{availablePieces.Count}: {candidate}");
                
                // 駒を相手に渡すことを想定した評価
                // 相手がこの駒でベストな手を打った場合の最悪スコアを求める
                int score = EvaluatePieceChoice(state, candidate, availablePieces, depth);
                
                Debug.Log($"駒 {candidate} のスコア: {score}");
                
                // スコアが高いほど良い（自分にとって有利）
                if (score > bestScore)
                {
                    bestScore = score;
                    bestPiece = candidate;
                    Debug.Log($"新ベスト駒更新: {bestPiece} (スコア: {bestScore})");
                }
            }
            
            Debug.Log($"駒選択完了 - 最適駒: {bestPiece}, 最終スコア: {bestScore}");
            return bestPiece;
        }
        catch (Exception e)
        {
            Debug.LogError($"駒選択全探索でエラー: {e.Message}");
            Debug.LogError($"スタックトレース: {e.StackTrace}");
            
            // エラー時は最初の候補を返す
            if (availablePieces.Count > 0)
            {
                Debug.Log($"エラー時フォールバック駒: {availablePieces[0]}");
                return availablePieces[0];
            }
            
            Debug.LogError("利用可能駒なし - デフォルト駒を返す");
            return PieceId.FSCB;
        }
    }
    
    /// <summary>
    /// 駒を相手に渡した場合の評価を行う
    /// </summary>
    private static int EvaluatePieceChoice(Piece[] state, PieceId candidate, List<PieceId> availablePieces, int depth)
    {
        try
        {
            // 相手がこの駒を使って最善手を打った場合の最悪のスコアを求める
            List<int> emptyPositions = GetEmptyPositions(state);
            int worstScore = int.MaxValue; // 相手にとって最善＝自分にとって最悪
            
            foreach (int position in emptyPositions)
            {
                // 相手がこの位置に駒を置いた場合の盤面
                Piece[] newState = PlacePieceOnBoard(state, candidate, position);
                
                // 即座に相手が勝利する場合は最悪スコア
                PlayerId winner = JudgeWinner(newState);
                if (winner != PlayerId.None)
                {
                    Debug.Log($"駒 {candidate} 位置 {position}: 相手即勝利 → 最悪スコア");
                    return LOSE_SCORE;
                }
                
                // この後のゲーム展開を評価
                List<PieceId> newAvailablePieces = RemovePieceFromAvailable(availablePieces, candidate);
                
                int score;
                if (newAvailablePieces.Count == 0)
                {
                    // ゲーム終了
                    score = EvaluateBoard(newState, PlayerId.Player1);
                }
                else
                {
                    // 次は自分の駒選択ターン（相手に渡す駒を選ぶ）
                    score = AlphaBetaSearch(newState, PieceId.FSCB, newAvailablePieces, depth - 1, int.MinValue, int.MaxValue, true, true);
                }
                
                // 相手にとっての最善手＝自分にとっての最悪
                worstScore = Math.Min(worstScore, score);
                
                Debug.Log($"駒 {candidate} 位置 {position}: スコア {score}");
            }
            
            Debug.Log($"駒 {candidate} の最終評価: {worstScore}");
            return worstScore;
        }
        catch (Exception e)
        {
            Debug.LogError($"駒評価でエラー: {e.Message}");
            return LOSE_SCORE; // エラー時は最悪スコア
        }
    }
    
    /// <summary>
    /// アルファベータ探索で最適な配置位置を求める
    /// </summary>
    public static int FindBestPositionWithSearch(Piece[] state, PieceId selectedPiece, List<PieceId> availablePieces, int depth = 6)
    {
        try
        {
            List<int> emptyPositions = GetEmptyPositions(state);
            Debug.Log($"配置位置全探索開始 - 深度: {depth}, 候補位置数: {emptyPositions.Count}");
            
            int bestScore = int.MinValue;
            int bestPosition = emptyPositions[0];
            int evaluatedCount = 0;
            
            foreach (int position in emptyPositions)
            {
                evaluatedCount++;
                Debug.Log($"位置評価 {evaluatedCount}/{emptyPositions.Count}: 位置 {position} ({position % 4}, {position / 4})");
                
                // 駒を配置した新しい盤面で評価
                Piece[] newState = PlacePieceOnBoard(state, selectedPiece, position);
                List<PieceId> newAvailablePieces = RemovePieceFromAvailable(availablePieces, selectedPiece);
                
                // 相手の手番でアルファベータ探索
                int score = AlphaBetaSearch(newState, PieceId.FSCB, newAvailablePieces, depth - 1, int.MinValue, int.MaxValue, true, true);
                
                Debug.Log($"位置 {position} のスコア: {score}");
                
                if (score > bestScore)
                {
                    bestScore = score;
                    bestPosition = position;
                    Debug.Log($"新ベスト位置更新: {bestPosition} ({bestPosition % 4}, {bestPosition / 4}) スコア: {bestScore}");
                }
            }
            
            Debug.Log($"配置位置選択完了 - 最適位置: {bestPosition} ({bestPosition % 4}, {bestPosition / 4}), 最終スコア: {bestScore}");
            return bestPosition;
        }
        catch (Exception e)
        {
            Debug.LogError($"配置位置全探索でエラー: {e.Message}");
            Debug.LogError($"スタックトレース: {e.StackTrace}");
            
            // エラー時は最初の空き位置を返す
            List<int> emptyPositions = GetEmptyPositions(state);
            if (emptyPositions.Count > 0)
            {
                Debug.Log($"エラー時フォールバック位置: {emptyPositions[0]}");
                return emptyPositions[0];
            }
            
            Debug.LogError("空き位置なし - 位置0を返す");
            return 0;
        }
    }
    
    /// <summary>
    /// アルファベータ探索のメイン関数
    /// </summary>
    private static int AlphaBetaSearch(Piece[] state, PieceId selectedPiece, List<PieceId> availablePieces, 
                                     int depth, int alpha, int beta, bool isSelectPhase, bool isMaximizing)
    {
        // 終了条件チェック
        PlayerId winner = JudgeWinner(state);
        if (winner != PlayerId.None)
        {
            // 自分（AI）が勝った場合は正のスコア、相手が勝った場合は負のスコア
            // isMaximizingがtrueの時は自分のターン、falseの時は相手のターン
            return isMaximizing ? WIN_SCORE + depth : LOSE_SCORE - depth;
        }
        
        if (depth <= 0 || availablePieces.Count == 0)
        {
            return EvaluateBoard(state, PlayerId.Player1);
        }
        
        if (isSelectPhase)
        {
            // 駒選択フェーズ
            if (isMaximizing)
            {
                int maxScore = int.MinValue;
                foreach (PieceId piece in availablePieces)
                {
                    int score = AlphaBetaSearch(state, piece, availablePieces, depth, alpha, beta, false, isMaximizing);
                    maxScore = Math.Max(maxScore, score);
                    alpha = Math.Max(alpha, score);
                    if (beta <= alpha) break; // アルファベータカット
                }
                return maxScore;
            }
            else
            {
                int minScore = int.MaxValue;
                foreach (PieceId piece in availablePieces)
                {
                    int score = AlphaBetaSearch(state, piece, availablePieces, depth, alpha, beta, false, isMaximizing);
                    minScore = Math.Min(minScore, score);
                    beta = Math.Min(beta, score);
                    if (beta <= alpha) break; // アルファベータカット
                }
                return minScore;
            }
        }
        else
        {
            // 駒配置フェーズ
            List<int> emptyPositions = GetEmptyPositions(state);
            List<PieceId> newAvailablePieces = RemovePieceFromAvailable(availablePieces, selectedPiece);
            
            if (isMaximizing)
            {
                int maxScore = int.MinValue;
                foreach (int position in emptyPositions)
                {
                    Piece[] newState = PlacePieceOnBoard(state, selectedPiece, position);
                    int score = AlphaBetaSearch(newState, PieceId.FSCB, newAvailablePieces, depth - 1, alpha, beta, true, !isMaximizing);
                    maxScore = Math.Max(maxScore, score);
                    alpha = Math.Max(alpha, score);
                    if (beta <= alpha) break; // アルファベータカット
                }
                return maxScore;
            }
            else
            {
                int minScore = int.MaxValue;
                foreach (int position in emptyPositions)
                {
                    Piece[] newState = PlacePieceOnBoard(state, selectedPiece, position);
                    int score = AlphaBetaSearch(newState, PieceId.FSCB, newAvailablePieces, depth - 1, alpha, beta, true, !isMaximizing);
                    minScore = Math.Min(minScore, score);
                    beta = Math.Min(beta, score);
                    if (beta <= alpha) break; // アルファベータカット
                }
                return minScore;
            }
        }
    }
}