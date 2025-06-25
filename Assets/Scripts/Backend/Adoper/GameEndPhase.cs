public class GameEndPhase : GamePhase
{
    public GameEndPhase()
    {
        type = GamePhaseType.GameEnd;
    }
    public override Result execute(Command command, GameController gameController)
    {
        // Board board = gameController.getBoard();
        // Position position = ((PutPieceByUserCommand)command).position;
        // PieceId pieceId = gameController.board.getSelectedPieceId();
        // Result result = new PutPieceResult();

        // ((PutPieceResult)result).position = position;
        // ((PutPieceResult)result).pieceId = pieceId;
        // ((PutPieceResult)result).success = true;
        // ((PutPieceResult)result).currentGamePhase = GamePhaseType.GameEnd;

        return null;
    }
    public override GamePhase getNextPhase(GameController gameController)
    {
        return new SelectPlayerPhase();
    }
    public override Information getInformation(GameController gameController)
    {
        return new Information();
    }
}