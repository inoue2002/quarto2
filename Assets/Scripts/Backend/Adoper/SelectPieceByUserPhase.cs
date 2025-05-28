public class SelectPieceByUserPhase : GamePhase
{
    public override Result execute(Command command, GameController gameController)
    {
        Result result = SelectPieceUseCase.handle(gameController.getBoard(), ((SelectPieceByUserCommand)command).pieceId);






        return result;
    }
    public override GamePhase getNextPhase(GameController gameController)
    {
        if(gameController.getPlayerType(ActionType.PutPiece) == PlayerType.Cpu)
        {
            return new PutPieceByCpuPhase();
        }
        else
        {
            return new PutPieceByUserPhase();
        }
    }
    public override Information getInformation()//選択できる駒を返してあげる
    {
        return null;
    }
}