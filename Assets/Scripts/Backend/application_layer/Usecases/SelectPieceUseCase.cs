using UnityEngine;

public class SelectPieceUseCase
{
    public static Result handle(Board board, PieceId pieceId)
    {
        SelectPieceResult result = new SelectPieceResult();
        bool canSelect = board.canSelectPiece(pieceId);
        if (!canSelect)
        {
            result.success = false;
            return result;
        }
        board.setSelectedPiece(pieceId);
        result.success = true;
        result.pieceId = pieceId;
        Debug.Log("SelectPieceUseCase: " + result.success);
        Debug.Log("SelectPieceUseCase: " + result.pieceId);
        Debug.Log("SelectPieceUseCase: " + result.currentGamePhase);
        return result;
    }
}