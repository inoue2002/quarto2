using UnityEngine;

public class GameEndExecuter : Executer
{
    public override void execute(GameController gameController, Result result)
    {
        Debug.Log("GameEndExecuter");
    }
}
