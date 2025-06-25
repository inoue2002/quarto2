using UnityEngine;
using TMPro;

public class SelectPiecePresenter : Presenter
{
    public GameObject UICanvas;
    public TMP_Text currentPlayerText;
    public override void handle(GameController gameController, Information information)
    {
        Board board = gameController.getBoard();
        Piece[] state = board.getState();

        SelectPieceInformation selectPieceInformation = (SelectPieceInformation)information;
        if(selectPieceInformation.currentPlayerId == PlayerId.Player1){
            currentPlayerText.text = "Player1";
        }else{
            currentPlayerText.text = "Player2";
        }
        if(selectPieceInformation.currentPlayerType == PlayerType.Cpu){
            currentPlayerText.text += " (CPU)";
        }
        else{
            currentPlayerText.text += " (Human)";
        }
       
        
        return;
    }
}
