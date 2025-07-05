using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Youkan2PutPieceAlgorithm : PutPieceAlgorithm
{
    public override Position putPiece(Board board){
        Youkan2SelectPieceAlgorithm.YoukanGameState nowState = new Youkan2SelectPieceAlgorithm.YoukanGameState(board.getState());
        Position result = nowState.getPutPosition(nowState.getBestPiece());

        // ①即勝ちできるところを探してそこに置く
        // TODO
        // まず設置可能な場所を全部確認
        List<Position> puttablePositions = nowState.getPuttablePositions(nowState.getBestPiece());
        foreach(Position position in puttablePositions){
            // その場所に置いた時に即勝ちできるか確認

            // 勝てる場所がある場合はそのpositionを返す
        }

        // ②即勝ちできない場合は、1つ選び、その駒を置いた時に、相手に即勝ちさせてしまう駒がない場合そこに設置
        // TODO 


        // ③即勝ちできず、どこに置いても相手に渡す駒で負ける場合はランダム
        // TODO
        return result;
    }
}