public class PutPieceByCpuPhase : GamePhase
{
    public bool endFlag = false;
    
    public override Result execute(Command command, GameController gameController)
    {
        Board board = gameController.getBoard();
        PutPieceUseCase putPieceUseCase = new PutPieceUseCase();
        Player player = gameController.getPlayer();
        Position position = player.putPiece(board.getState(),board.getSelectedPiece());
        PutPieceResult result =(PutPieceResult)putPieceUseCase.handle(board,board.getSelectedPieceId(),position);
        
        // 勝敗判定
        endFlag = result.winner != PlayerId.None;
        
        return result;
    }
    public override GamePhase getNextPhase(GameController gameController)
    {
        throw new System.NotImplementedException();
    }
    public override Information getInformation()
    {
        return null;
    }
}