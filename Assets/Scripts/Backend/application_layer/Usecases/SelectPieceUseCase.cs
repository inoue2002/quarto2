public class SelectPieceUseCase{
    public static Result handle(Board board, PieceId pieceId){
        board.setSelectedPiece(pieceId); 
        return null;
    }
}