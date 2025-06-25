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
        
        // 駒選択後にプレイヤー交代（正しいQuartoルール）
        // 駒を選択したプレイヤーから、その駒を配置するプレイヤーに交代
        board.changePlayer();
        
        result.success = true;
        result.pieceId = pieceId;

        return result;
    }
}