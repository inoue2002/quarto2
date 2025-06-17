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
        board.changePlayer(board.getPlayerId());
        board.setSelectedPiece(pieceId);
        result.success = true;
        result.pieceId = pieceId;

        return result;
    }
}