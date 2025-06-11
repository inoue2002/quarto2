

public abstract class GamePhase
{
    public GamePhaseType type;
    public abstract Result execute(Command command, GameController gameController);
    public abstract GamePhase getNextPhase(GameController gameController);
    public abstract Information getInformation(GameController gameController);

}