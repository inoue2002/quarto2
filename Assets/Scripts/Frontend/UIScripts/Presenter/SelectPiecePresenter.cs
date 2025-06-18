using UnityEngine;

public class SelectPiecePresenter : Presenter
{
     public GameObject UICanvas;
    public override void handle(GameController gameController, Information information)
    {


        Board board = gameController.getBoard();
        Piece[] state = board.getState();
        //CurrentPlayerがCPUのときのみ、UIを更新
        // GameObject UICanvas = GameObject.Find("UICanvas").GetComponent<GameObject>();
        // if(gameController.getBoard().getPlayerId() == PlayerId.CPU){
        //     UICanvas.SetActive(true);
        // }
        // else{
        //     UICanvas.SetActive(false);
        // }
        return;
    }
}
