using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Youkan2SelectPieceAlgorithm;

public class Youkan2PutPieceAlgorithm : PutPieceAlgorithm
{
    public override Position putPiece(Board board)
    {
        // 今から渡す駒
        PieceId selectedPieceId = board.getSelectedPieceId();
    
        Position result = new PutPieceLib(board).GetPutBestPosition(selectedPieceId);
        return result;
    }


    public class PutPieceLib
    {
        public Board board;

        public PutPieceLib(Board board)
        {
            this.board = board;
        }
        public Position GetPutBestPosition(PieceId pieceId)
        {
            // Youkan2SelectPieceAlgorithm.QuartoAILib nowState = new Youkan2SelectPieceAlgorithm.QuartoAILib(state);
            // Position result = nowState.getPutBestPosition(pieceId);
            // return result;
            // 現在の盤面でおく駒の配列を取得する

            List<Position> puttablePositions = new QuartoAILib(board).GetPuttablePositions();

            foreach(Position position in puttablePositions){
                Board tempBoard = new QuartoAILib(board).getNextBoard(pieceId, position);
                if(tempBoard.judge() != PlayerId.None){
                    return position;
                }
            }

            // 即勝できない場合
            
            // そこにおくことで、相手に渡す駒がなくなる場所を省く
            // puttablePositionsの中から、順番にforeachで回して、その場所に置いた時に、渡せる駒があるかどうかを確認し、おける配列を新しくsafePositionsに追加
            List<Position> safePositions = new List<Position>();

            // PiceIdをputtablePositionの順番に置いた場面に置いて、残りの渡せる駒で即勝ちの組みを確認し、1つもない場合は、safePositionsに追加
            foreach(Position position in puttablePositions){
                Board tempBoard = new QuartoAILib(board).getNextBoard(pieceId, position);
                List<PieceId> selectablePieces = new QuartoAILib(tempBoard).GetSelectablePieces();
                
                List<(PieceId pieceId, Position position)> immediateWinPieces = new QuartoAILib(tempBoard).GetImmediateWinPieces(selectablePieces, puttablePositions);
                if(immediateWinPieces.Count == 0){
                    safePositions.Add(position);
                }
            }

            if(safePositions.Count == 0){
                // 安全な場所がない
                return puttablePositions[UnityEngine.Random.Range(0, puttablePositions.Count)];
            }

            // Debug
            Debug.Log("Youkan2PutPieceAlgorithm: safePositions: " + safePositions.Count);

            // TODO - リーチを崩さないところに置きたい
            // それでもない場合はランダムにおく
            return safePositions[UnityEngine.Random.Range(0, safePositions.Count)];
        }
    }
}