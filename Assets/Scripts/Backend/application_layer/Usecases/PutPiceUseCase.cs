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
        
        // 駒配置後にプレイヤー交代
        board.changePlayer();
        
        PlayerId winner = board.judge();
        result.success = true;
        result.winner = winner;
        result.pieceId = pieceId;
        result.position = position;
        return result;
    }
}