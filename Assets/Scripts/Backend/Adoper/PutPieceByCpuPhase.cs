public class PutPieceByCpuPhase : GamePhase
{

    public bool endFlag = false;
    private bool success = false;
    public PutPieceByCpuPhase()
    {
        type = GamePhaseType.PutPieceByCpu;
    }
    public override Result execute(Command command, GameController gameController)
    {
        Board board = gameController.getBoard();
        PutPieceUseCase putPieceUseCase = new PutPieceUseCase();
        Player player = gameController.getPlayer();
        Position position = player.putPiece(board);
        PutPieceResult result = (PutPieceResult)putPieceUseCase.handle(board, board.getSelectedPieceId(), position);
        if (result.success)
        {
            success = true;
        }
        else
        {
            success = false;
        }
        if (result.winner == PlayerId.None)
        {
            endFlag = false;
        }
        else
        {
            endFlag = true;
        }
        return result;
    }
    public override GamePhase getNextPhase(GameController gameController)
    {
        if (!success)
        {
            return new PutPieceByCpuPhase();
        }
        if (endFlag)
        {
            return new GameEndPhase();
        }
        else
        {
            if (gameController.getPlayerType(ActionType.SelectPiece) == PlayerType.Cpu)
            {
                return new SelectPieceByCpuPhase();
            }
            else if (gameController.board.getState().Length == 16)
            {
                return new GameEndPhase();
            }
            else
            {
                return new SelectPieceByUserPhase();
            }
        }
    }
    public override Information getInformation(GameController gameController)
    {
        return new Information();
    }
}