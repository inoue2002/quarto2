public class PutPieceUseCase{
    public Result handle(Board board, PieceId pieceId, Position position){
        PutPieceResult result = new PutPieceResult();
        bool canPut = board.canPutPiece(position);
        if(!canPut){
            result.success = false;
            result.winner = PlayerId.None;
            return result;
        }
        board.putPiece(pieceId, position);
        PlayerId winner = board.judge();
        result.success = true;
        result.winner = winner;
        return result;
    }
}