using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Youkan2PutPieceAlgorithm : PutPieceAlgorithm
{
    public override Position putPiece(Board board){
        Youkan2SelectPieceAlgorithm.YoukanGameState nowState = new Youkan2SelectPieceAlgorithm.YoukanGameState(board.getState());
        Position result = nowState.getPutPosition(nowState.getBestPiece());
        return result;
    }
}