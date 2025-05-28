public class GameEndPhase : GamePhase
{
    public override Result execute(Command command, GameController gameController)
    {
        return null;
    }
    public override GamePhase getNextPhase(GameController gameController)
    {
        return new SelectPlayerPhase();
    }
    public override Information getInformation()
    {
        return null;
    }
}