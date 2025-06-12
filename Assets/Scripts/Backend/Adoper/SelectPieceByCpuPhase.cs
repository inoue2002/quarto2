/// <summary>
/// 
/// </summary>
public class SelectPieceByCpuPhase : GamePhase
{
    private bool success = false;
    public SelectPieceByCpuPhase()
    {
        type = GamePhaseType.SelectPieceByCpu;
    }
    public override Result execute(Command command, GameController gameController)
    {
        Player player = gameController.getPlayer();
        PieceId pieceId = player.selectPiece(gameController.getBoard());
        SelectPieceResult result = (SelectPieceResult)SelectPieceUseCase.handle(gameController.getBoard(), pieceId);
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
            return new SelectPieceByCpuPhase();
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
    public override Information getInformation(GameController gameController)
    {
        return new SelectPieceInformation(gameController.getBoard().getSelectablePieces());
    }
}