public class SelectPieceByUserPhase : GamePhase
{
    private bool success = false;
    public SelectPieceByUserPhase()
    {
        type = GamePhaseType.SelectPieceByUser;
    }
    public override Result execute(Command command, GameController gameController)
    {
        SelectPieceResult result = (SelectPieceResult)SelectPieceUseCase.handle(gameController.getBoard(), ((SelectPieceByUserCommand)command).pieceId);
        if (result.success)
        {
            success = true;
        }
        else
        {
            success = false;
        }
        return result;
    }
    public override GamePhase getNextPhase(GameController gameController)
    {
        if (!success)
        {
            return new SelectPieceByUserPhase();
        }
        else
        {
            if (gameController.getPlayerType(ActionType.PutPiece) == PlayerType.Cpu)
            {
                return new PutPieceByCpuPhase();
            }
            else
            {
                return new PutPieceByUserPhase();
            }
        }
    }
    public override Information getInformation(GameController gameController)//選択できる駒を返してあげる
    {
        return new SelectPieceInformation(gameController.getBoard().getSelectablePieces());
    }
}