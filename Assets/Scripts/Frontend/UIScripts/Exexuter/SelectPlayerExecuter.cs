using UnityEngine;

public class SelectPlayerExecuter : Executer
{
    
    int playerNumber = 0;

    
    public GameObject CanvasPlayer;
    public override void execute(GameController gameController, Result result)
    {
        if(playerNumber == 0)
        {
            playerNumber = 1;
            
        }
        else
        {
           CanvasPlayer.SetActive(false);
        }
        
    }
    
}
