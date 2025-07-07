using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Youkan2SelectPieceAlgorithm : SelectPieceAlgorithm
{
    public override PieceId SelectPiece(Piece[] state)
    {
        // piecesを元にBoardを作成
        Board board = new Board();
        for (int i = 0; i < state.Length; i++)
        {
            if (state[i] != null)
            {
                board.putPiece(state[i].getPieceId(), new Position(i % 4, i / 4));
            }
        }

        PieceId result = new SelectPieceLib(board).getSelectBestPiece();
        Debug.Log("Youkan2:選択完了"+result.ToString());

        // なぜか渡せない
        return result;
    }

    public class SelectPieceLib
    {
        public Board board;
        private QuartoAILib quartoAILib;

        public SelectPieceLib(Board board)
        {
            this.board = board;
            this.quartoAILib = new QuartoAILib(board);
        }

        // 相手に渡す駒を選択するメイン関数
        // 方針：今の盤面に置いて、これを渡すと相手が置くところに最も困るようなピースを渡したい
        // それを渡した瞬間、即勝されてしまうようなものは絶対に渡したくない
        public PieceId getSelectBestPiece()
        {
            // AIライブラリのインスタンスを作成
            QuartoAILib quartoAILib = new QuartoAILib(board);

            // まず渡すことが可能な駒を取得
            List<PieceId> selectablePieces = quartoAILib.GetSelectablePieces();
            // 現状設置可能な場所を取得
            List<Position> puttablePositions = quartoAILib.GetPuttablePositions();
            
            // 空きマス数を計算
            int emptySpaces = puttablePositions.Count;
            Debug.Log($"Youkan2: 空きマス数 = {emptySpaces}");

            // 選択する駒*設置可能な場所の全パターンを作成する
            List<(PieceId pieceId, Position position)> allPatterns = new List<(PieceId pieceId, Position position)>();
            foreach (PieceId pieceId in selectablePieces)
            {
                foreach (Position position in puttablePositions)
                {
                    allPatterns.Add((pieceId, position));
                }
            }

            // 1つもない場合はエラー
            if (selectablePieces.Count == 0)
            {
                Debug.Log("Youkan2:渡すことが可能な駒がありません");
                throw new Exception("渡すことが可能な駒がありません");
            }

            // 即勝されてしまうような駒を取得し、排除する
            List<(PieceId pieceId, Position position)> immediateWinPieces = quartoAILib.GetImmediateWinPieces(selectablePieces, puttablePositions);
            
            // 即勝される駒を除外した安全な駒のリストを作成
            List<PieceId> safePieces = new List<PieceId>();
            foreach (PieceId pieceId in selectablePieces)
            {
                bool isSafe = true;
                foreach (Position position in puttablePositions)
                {
                    if (immediateWinPieces.Contains((pieceId, position)))
                    {
                        Debug.Log("Youkan2:即勝されてしまう駒がありますので候補から排除します" + pieceId.ToString());
                        isSafe = false;
                        break;
                    }
                }
                if (isSafe)
                {
                    safePieces.Add(pieceId);
                }
            }

            if(safePieces.Count == 0){
                // 安全な駒がない場合 = 全ての駒で相手が即勝ちできる
                Debug.Log("Youkan2:安全な駒がない！相手を詰み状態にできるかチェックします");
                
                // 相手にとって最も選択肢が少ない（詰みに近い）駒を選ぶ
                PieceId? checkmateMove = FindCheckmateMove(selectablePieces, puttablePositions);
                if (checkmateMove.HasValue)
                {
                    Debug.Log($"Youkan2:相手を詰みに追い込む駒を選択: {checkmateMove.Value}");
                    return checkmateMove.Value;
                }
                
                // それでも見つからない場合はランダム
                return selectablePieces[UnityEngine.Random.Range(0, selectablePieces.Count)];
            }

            // 空きマス8個以下で深い読みを開始
            if (emptySpaces <= 8)
            {
                Debug.Log("Youkan2:詳細読みモード");
                return SelectBestPieceWithDeepAnalysis(safePieces, puttablePositions);
            }
            else
            {
                Debug.Log("Youkan2:基本戦略モード");
                return SelectBestPieceWithBasicStrategy(safePieces, puttablePositions);
            }
        }

        private PieceId SelectBestPieceWithBasicStrategy(List<PieceId> safePieces, List<Position> puttablePositions)
        {
            Dictionary<PieceId, float> scores = new Dictionary<PieceId, float>();
            
            foreach (PieceId pieceId in safePieces)
            {
                float score = 0f;
                
                // 相手が置ける場所の数でペナルティ
                score += puttablePositions.Count * 10f;
                
                // 駒の属性バランスをチェック
                int pieceAttributes = (int)pieceId;
                int activeBits = 0;
                for (int i = 0; i < 4; i++)
                {
                    if ((pieceAttributes & (1 << i)) != 0) activeBits++;
                }
                // 極端な駒（すべて0またはすべて1）は危険
                if (activeBits == 0 || activeBits == 4) score -= 5f;
                
                scores[pieceId] = score;
            }
            
            return scores.OrderBy(x => x.Value).First().Key;
        }

        private PieceId SelectBestPieceWithDeepAnalysis(List<PieceId> safePieces, List<Position> puttablePositions)
        {
            Dictionary<PieceId, float> scores = new Dictionary<PieceId, float>();
            
            foreach (PieceId pieceId in safePieces)
            {
                float score = 0f;
                
                // 相手の安全な置き場所を制限する
                int opponentSafePositions = CountOpponentSafePositions(pieceId, puttablePositions);
                score += opponentSafePositions * 10f;
                
                // リーチ破壊の効果
                int reachBreaks = CountReachBreakingMoves(pieceId, puttablePositions);
                score -= reachBreaks * 5f;
                
                // 2手先の危険度
                float nextTurnRisk = CalculateNextTurnRisk(pieceId, puttablePositions);
                score += nextTurnRisk;
                
                scores[pieceId] = score;
            }
            
            return scores.OrderBy(x => x.Value).First().Key;
        }

        private int CountOpponentSafePositions(PieceId pieceId, List<Position> positions)
        {
            int safeCount = 0;
            
            foreach (Position pos in positions)
            {
                Board tempBoard = quartoAILib.getNextBoard(pieceId, pos);
                
                // TaigaのIsLosingStateロジックを使って安全性を判定
                bool isSafe = true;
                List<PieceId> remainingPieces = quartoAILib.GetSelectablePieces();
                remainingPieces.Remove(pieceId);
                
                foreach (PieceId nextPiece in remainingPieces)
                {
                    List<Position> nextPositions = new QuartoAILib(tempBoard).GetPuttablePositions();
                    var winningMoves = new QuartoAILib(tempBoard).GetImmediateWinPieces(
                        new List<PieceId> { nextPiece }, nextPositions);
                    
                    if (winningMoves.Count > 0)
                    {
                        isSafe = false;
                        break;
                    }
                }
                
                if (isSafe) safeCount++;
            }
            
            return safeCount;
        }

        private int CountReachBreakingMoves(PieceId pieceId, List<Position> positions)
        {
            int breakCount = 0;
            
            foreach (Position pos in positions)
            {
                if (DoesBreakReach(board.getState(), pos, pieceId))
                {
                    breakCount++;
                }
            }
            
            return breakCount;
        }

        /// <summary>
        /// リーチを崩すかどうかを判定
        /// </summary>
        private bool DoesBreakReach(Piece[] state, Position position, PieceId pieceToPlace)
        {
            int posIndex = (int)(position.Y * 4 + position.X);
            int[][] lines = {
                new int[] {0, 1, 2, 3}, new int[] {4, 5, 6, 7}, new int[] {8, 9, 10, 11}, new int[] {12, 13, 14, 15}, // 横
                new int[] {0, 4, 8, 12}, new int[] {1, 5, 9, 13}, new int[] {2, 6, 10, 14}, new int[] {3, 7, 11, 15}, // 縦
                new int[] {0, 5, 10, 15}, new int[] {3, 6, 9, 12} // 斜め
            };
            
            foreach (int[] line in lines)
            {
                if (!line.Contains(posIndex)) continue;
                
                // このラインの既存の駒を取得
                List<Piece> linePieces = new List<Piece>();
                int emptyCount = 0;
                
                foreach (int pos in line)
                {
                    if (state[pos] != null)
                    {
                        linePieces.Add(state[pos]);
                    }
                    else if (pos == posIndex)
                    {
                        // 置こうとしている場所
                        linePieces.Add(new Piece(pieceToPlace));
                    }
                    else
                    {
                        emptyCount++;
                    }
                }
                
                // 3つ揃っていて、4つ目を置く場合
                if (linePieces.Count == 4 && emptyCount == 0)
                {
                    // 元の3つに共通属性があり、4つ目で崩れるかチェック
                    List<Piece> originalThree = new List<Piece>();
                    for (int i = 0; i < line.Length; i++)
                    {
                        if (state[line[i]] != null)
                        {
                            originalThree.Add(state[line[i]]);
                        }
                    }
                    
                    if (originalThree.Count == 3 && 
                        TaigaPutPieceAlgorithm.HasCommonAttribute(
                            originalThree[0], originalThree[1], originalThree[2], originalThree[2]))
                    {
                        // 4つで共通属性がなくなったらリーチを崩す
                        if (!TaigaPutPieceAlgorithm.HasCommonAttribute(
                            linePieces[0], linePieces[1], linePieces[2], linePieces[3]))
                        {
                            return true;
                        }
                    }
                }
            }
            
            return false;
        }

        private float CalculateNextTurnRisk(PieceId pieceId, List<Position> positions)
        {
            float totalRisk = 0f;
            
            foreach (Position pos in positions)
            {
                Board tempBoard = quartoAILib.getNextBoard(pieceId, pos);
                List<PieceId> remainingPieces = quartoAILib.GetSelectablePieces();
                remainingPieces.Remove(pieceId);
                
                // 相手が次に選べる駒の中で、自分が危険になる駒の割合
                int dangerousPieces = 0;
                foreach (PieceId nextPiece in remainingPieces)
                {
                    List<Position> myNextPositions = new QuartoAILib(tempBoard).GetPuttablePositions();
                    bool hasSafeMove = false;
                    
                    foreach (Position myPos in myNextPositions)
                    {
                        Board futureBoard = new QuartoAILib(tempBoard).getNextBoard(nextPiece, myPos);
                        if (futureBoard.judge() == PlayerId.None)
                        {
                            hasSafeMove = true;
                            break;
                        }
                    }
                    
                    if (!hasSafeMove) dangerousPieces++;
                }
                
                totalRisk += (float)dangerousPieces / remainingPieces.Count * 20f;
            }
            
            return totalRisk / positions.Count;
        }

        private PieceId? FindCheckmateMove(List<PieceId> pieces, List<Position> positions)
        {
            foreach (PieceId piece in pieces)
            {
                bool isWinningPiece = true;
                
                // この駒を相手が全ての可能な場所に置いた場合をチェック
                foreach (Position pos in positions)
                {
                    // 相手がこの位置に駒を置いた盤面を作成
                    Board boardAfterOpponentMove = quartoAILib.getNextBoard(piece, pos);
                    
                    // 相手がこの手で勝ったら、そもそも次のターンがない
                    if (boardAfterOpponentMove.judge() != PlayerId.None)
                    {
                        isWinningPiece = false;
                        break;
                    }
                    
                    // 次に自分が選べる駒のリストを取得
                    List<PieceId> remainingPieces = new QuartoAILib(boardAfterOpponentMove).GetSelectablePieces();
                    List<Position> remainingPositions = new QuartoAILib(boardAfterOpponentMove).GetPuttablePositions();
                    
                    // 残り駒がない、または置ける場所がない場合は引き分け
                    if (remainingPieces.Count == 0 || remainingPositions.Count == 0)
                    {
                        isWinningPiece = false;
                        break;
                    }
                    
                    // 次に自分が選べる駒の中で、必ず勝てる駒があるかチェック
                    bool canGuaranteeWin = false;
                    foreach (PieceId nextPiece in remainingPieces)
                    {
                        bool allPositionsWinning = true;
                        
                        // この駒を相手に渡した時、相手がどこに置いても自分が勝てるかチェック
                        foreach (Position nextPos in remainingPositions)
                        {
                            Board finalBoard = new QuartoAILib(boardAfterOpponentMove).getNextBoard(nextPiece, nextPos);
                            if (finalBoard.judge() == PlayerId.None)
                            {
                                // この位置では勝てない
                                allPositionsWinning = false;
                                break;
                            }
                        }
                        
                        if (allPositionsWinning)
                        {
                            canGuaranteeWin = true;
                            break;
                        }
                    }
                    
                    // この配置で必勝できない場合、詰みではない
                    if (!canGuaranteeWin)
                    {
                        isWinningPiece = false;
                        break;
                    }
                }
                
                // 相手がどこに置いても次で必勝できる駒を発見
                if (isWinningPiece)
                {
                    Debug.Log($"Youkan2: 詰み手発見: {piece}");
                    return piece;
                }
            }
            
            return null;
        }
    }


    // 汎用的なquartoAIクラス
    public class QuartoAILib
    {
        public Board board;

        public QuartoAILib(Board board)
        {
            this.board = board;
        }

        /// <summary>
        /// 現在のボードに配置されている全ての駒のIDリストを取得する
        /// </summary>
        /// <returns>配置されている駒のIDリスト</returns>
        public List<PieceId> GetAllPuttedPieces()
        {
            List<PieceId> allPieces = new List<PieceId>();
            Piece[] boardState = board.getState();
            
            for (int i = 0; i < boardState.Length; i++)
            {
                if (boardState[i] != null)
                {
                    allPieces.Add(boardState[i].getPieceId());
                }
            }
            return allPieces;
        }

        /// <summary>
        /// このゲームの全ての駒のIDリストを取得する
        /// </summary>
        /// <returns>全ての駒のIDリスト</returns>
        public List<PieceId> GetAllPieces()
        {
            List<PieceId> allPieces = new List<PieceId>();
            for (int i = 0; i < 16; i++)
            {
                allPieces.Add((PieceId)i);
            }
            return allPieces;
        }

        // 元々の盤面,設置するピース,場所 から新しいボードを作成
        public Board getNextBoard(PieceId pieceId, Position position)
        {
            Board tempBoard = new Board();
            for (int i = 0; i < board.getState().Length; i++)
            {
                if (board.getState()[i] != null)
                {
                    tempBoard.putPiece(board.getState()[i].getPieceId(), new Position(i % 4, i / 4));
                }
            }
            tempBoard.putPiece(pieceId, position);
            return tempBoard;
        }


        /// <summary>
        /// 今のボードに新しく駒を置いた時に、即勝ちできる駒があるかどうかを判定し、あればその駒と位置を返す関数
        /// </summary>
        /// <param name="availablePieces">利用可能な駒のリスト</param>
        /// <param name="puttablePositions">配置可能な位置のリスト</param>
        /// <returns>即勝ちできる駒と位置のリスト</returns>
        public List<(PieceId pieceId, Position position)> GetImmediateWinPieces(List<PieceId> availablePieces, List<Position> puttablePositions)
        {
            List<(PieceId pieceId, Position position)> winningCombinations = new List<(PieceId pieceId, Position position)>();

            foreach (PieceId pieceId in availablePieces)
            {
                foreach (Position position in puttablePositions)
                {
                    // 元々の盤面,設置するピース,場所 から新しいボードを作成
                    Board tempBoard = new QuartoAILib(board).getNextBoard(pieceId, position);
                    // 新しいボードで即勝ちできるかどうかを判定
                    if (tempBoard.judge() != PlayerId.None)
                    {
                        winningCombinations.Add((pieceId, position));
                    }
                }
            }
            // 即勝ちできる駒の組み合わせのリストを返す
            return winningCombinations;
        }


        /// <summary>
        /// 現在のボードで駒を配置可能な全ての位置を取得する
        /// </summary>
        /// <returns>配置可能な位置のリスト</returns>
        public List<Position> GetPuttablePositions()
        {
            // おける場所を探す
            List<Position> puttablePositions = new List<Position>();
            for (int i = 0; i < board.getState().Length; i++)
            {
                if (board.getState()[i] == null)
                {
                    puttablePositions.Add(new Position(i % 4, i / 4));
                }
            }
            return puttablePositions;
        }
        /// <summary>
        /// 現在のボードで渡すことができる駒のリストを取得する
        /// </summary>
        /// <returns>渡すことができる駒のリスト</returns>
        public List<PieceId> GetSelectablePieces()
        {
            // 全部のピースの中から、すでにボードに置かれているものを除いたものを返す
            List<PieceId> allPieces = GetAllPieces();

            List<PieceId> alreadyPiceId = board.getState().Where(p => p != null).Select(p => p.getPieceId()).ToList();
            
            // 差分
            return allPieces.Except(alreadyPiceId).ToList();
        }
    }
}