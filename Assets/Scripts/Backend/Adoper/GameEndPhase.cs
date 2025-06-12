public class GameEndPhase : GamePhase
{
    public GameEndPhase()
    {
        type = GamePhaseType.GameEnd;
    }
    public override Result execute(Command command, GameController gameController)
    {
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