using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TaigaPutPieceAlgorithm : PutPieceAlgorithm
{
    public override Position putPiece(Board board)
    {
        PieceId nextPutPeaceId = board.getSelectedPieceId();
        Debug.Log("Taiga: たいが来るよ");
        //1.自分の勝てる手を探してあれば置く
        List<int> puttablePosition = getPuttablePositions(board);
        foreach (int position in puttablePosition)
        {
            Piece[] tempPiece = PlacePieceOnBoard(board.getstate(), nextPutPeaceId, position);
            if (JudgeWinner(tempPiece) != PlayerId.None)
            {
                Position winPosition = new Position(position % 4, position / 4);
                Debug.Log($"Taiga: 勝利手を発見！{Youkan2SelectPieceAlgorithm.QuartoAILib.PieceIdToReadableString(nextPutPeaceId)}を位置({winPosition.X},{winPosition.Y})に配置");
                return winPosition;
            }
        }
        // 2.次にどの駒を選んでも積みにならないような「安全な手」を置く
        List<int> safePosition = new List<int>();
        foreach (int position in puttablePosition)
        {
            Piece[] tempBoard = PlacePieceOnBoard(board.getstate(), nextPutPeaceId, position);

            // この手で相手が必勝にならないかチェックする
            if (!IsLosingState(tempBoard, board))
            {
                safePosition.Add(position);
            }
        }

        // 3. 手を決定する
        int lastPosition;
        if (safePosition.Count > 0)
        {
            // 安全な手の中から、次に渡す駒がなくならない位置をランダムに選ぶ
            Debug.Log($"Taiga: 安全な手が{safePosition.Count}個あります");
            lastPosition = ChooseBestSafePosition(safePosition, board.getState(), nextPutPeaceId);
        }
        else
        {
            // 安全な手がなければ（= どの手を打っても積み）、諦めてランダムに選ぶ
            Debug.Log("Taiga: 安全な手がない！ランダムに選択");
            int randomIndex = new System.Random().Next(puttablePosition.Count);
            lastPosition = puttablePosition[randomIndex];
        }

        Position finalPosition = new Position(lastPosition % 4, lastPosition / 4);
        Debug.Log($"Taiga: {Youkan2SelectPieceAlgorithm.QuartoAILib.PieceIdToReadableString(nextPutPeaceId)}を位置({finalPosition.X},{finalPosition.Y})に配置");
        return finalPosition;
    }

    /// <summary>
    /// 安全な配置の中から、次に渡す駒がなくならない位置をランダムに選ぶ
    /// </summary>
    private int ChooseBestSafePosition(List<int> safePositions, Piece[] currentState, PieceId pieceToPlace)
    {
        List<int> positionsWithPiecesToGive = new List<int>();
        
        // 渡す駒がある位置をリストアップ
        foreach (int position in safePositions)
        {
            // この位置に駒を置いた後の盤面を作成
            Piece[] futureState = PlacePieceOnBoard(currentState, pieceToPlace, position);
            
            // 次に渡せる駒があるかチェック（Youkan2Selectのライブラリを使用）
            if (Youkan2SelectPieceAlgorithm.QuartoAILib.CanGivePieceAfterMove(futureState))
            {
                positionsWithPiecesToGive.Add(position);
            }
        }
        
        // 渡す駒がある位置からランダムに選択
        if (positionsWithPiecesToGive.Count > 0)
        {
            Debug.Log($"Taiga: 渡す駒がある位置が{positionsWithPiecesToGive.Count}個見つかりました");
            int randomIndex = new System.Random().Next(positionsWithPiecesToGive.Count);
            return positionsWithPiecesToGive[randomIndex];
        }
        
        // 全ての位置で渡す駒がなくなる場合は安全な位置からランダムに選ぶ
        Debug.Log("Taiga: 全ての位置で駒がなくなる...安全な位置からランダム選択");
        int fallbackIndex = new System.Random().Next(safePositions.Count);
        return safePositions[fallbackIndex];
    }

    /// <summary>
    /// その盤面が、次にどの駒を渡しても相手が勝ってしまう「積み状態」かどうかを判定する
    /// </summary>
    public bool IsLosingState(Piece[] currentState, Board board)
    {
        // まだ使われていない駒のリストを取得する
        List<PieceId> unusedPiece = getUnusedPiece(board);

        // 次に相手が置ける場所を探す
        List<int> nextPuttablePosition = new List<int>();
        for (int i = 0; i < currentState.Length; i++)
        {
            if (currentState[i] == null)
            {
                nextPuttablePosition.Add(i);
            }
        }

        // 盤面がすべて埋まった状態
        if (unusedPiece.Count == 0)
        {
            return false;
        }
        // 相手が勝てないかを判定
        foreach (PieceId givenPiece in unusedPiece)
        {
            bool opponentCanWin = false;
            // 相手が勝てる場所があるか探す
            foreach (int nextPosition in nextPuttablePosition)
            {
                Piece[] futureBoard = PlacePieceOnBoard(currentState, givenPiece, nextPosition);
                if (JudgeWinner(futureBoard) != PlayerId.None)
                {
                    opponentCanWin = true;
                    break;
                }
            }
            // もし、駒を渡しても相手が勝てないなら、この盤面は「必敗」ではない。
            if (!opponentCanWin)
            {
                return false;
            }
        }
        // どの駒を渡しても相手が勝ってしまう=積み状態
        return true;
    }

    /// <summary>
    /// まだ盤面と手駒で使われていない駒のリストを取得するヘルプメソッド
    /// </summary>
    public List<PieceId> getUnusedPiece(Board board)
    {
        List<PieceId> allPieces = new List<PieceId>();
        for (int i = 0; i < 16; i++)
        {
            allPieces.Add((PieceId)i);
        }

        // 盤面にある駒を除く
        foreach (Piece p in board.getstate())
        {
            if (p != null)
            {
                allPieces.Remove(p.getPieceId());
            }
        }

        // 現在選ばれている駒も除く
        allPieces.Remove(board.getSelectedPieceId());

        return allPieces;
    }
    //以下のコードは置いたら必勝する場合以外はランダムに置くという意味のコードである
    /* 置ける場所のリストからランダムに1つのインデックスを取得
    int randomIndex = new System.Random().Next(puttablePosition.Count);
    // そのインデックスを使って、ランダムな配置場所を取得
    int randomPosition = puttablePosition[randomIndex];

    return new Position(randomPosition % 4, randomPosition / 4);*/


    public List<int> getPuttablePositions(Board board)
    {
        // おける場所を探す
        List<int> puttablePositions = new List<int>();
        for (int i = 0; i < board.getstate().Length; i++)
        {
            if (board.getstate()[i] == null)
            {
                puttablePositions.Add(i);
            }
        }
        return puttablePositions;
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
}