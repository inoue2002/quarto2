using UnityEngine;

public class GemeEndPresenter : Presenter
{

    public GameObject gameEndPanel;
    public override void handle(GameController gameController, Information information)
    {
        Debug.Log("GemeEndPresenter");
        gameEndPanel.SetActive(true);
    }
}
