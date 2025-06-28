using UnityEngine;
using TMPro;

public class PutPiecePresenter : Presenter
{
    public TMP_Text currentPlayerText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void handle(GameController gameController, Information information)
    {
        PutPieceInformation putPieceInformation = (PutPieceInformation)information;
        if(putPieceInformation.currentPlayerId == PlayerId.Player1){
            currentPlayerText.text = "Player1";
        }else{
            currentPlayerText.text = "Player2";
        }
        if(putPieceInformation.currentPlayerType == PlayerType.Cpu){
            if (!string.IsNullOrEmpty(putPieceInformation.algorithmName))
            {
                currentPlayerText.text += " (CPU - " + putPieceInformation.algorithmName + ")";
            }
            else
            {
                currentPlayerText.text += " (CPU)";
            }
        }
        else{
            currentPlayerText.text += " (Human)";
        }
        return;
    }

    // Update is called once per frame
    
}
