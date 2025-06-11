using UnityEngine;

public class SelectPlayerExecuter : Executer
{
    public GameObject CanvasPlayer;
    int playerNumber = 0;
    public override void execute(GameController gameController, Command command)
    {
        if(playerNumber == 0)
        {
            playerNumber = 1;
            Debug.Log("playerNumber: " + playerNumber);
        }
        else
        {
           CanvasPlayer.SetActive(false);
        }
    }
}
