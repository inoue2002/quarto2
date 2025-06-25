using UnityEngine;
using TMPro;

public class SelectPlayerExecuter : Executer
{
    
    int playerNumber = 0;


    
    public GameObject CanvasPlayer;
    public GameObject UICanvas;
    public TextMeshProUGUI PlayerNumber;

    public override void execute(GameController gameController, Result result)
    {
        if(playerNumber == 0)
        {
            playerNumber = 1;
            PlayerNumber.text = "Player2";
        }
        else
        {
           CanvasPlayer.SetActive(false);
           UICanvas.SetActive(true);
        // GameObject UICanvas = GameObject.Find("UICanvas").GetComponent<GameObject>();

        // if(gameController.getInformation().type == GamePhaseType.SelectPlayer){
        //     UICanvas.SetActive(true);
        // }
        // else{
        //     UICanvas.SetActive(false);
        // }
        playerNumber = 0;

        }
        
    }
    
}
