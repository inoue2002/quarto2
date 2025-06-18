using UnityEngine;

public class CpuNextButtonPresenter : Presenter
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   public GameObject cpuNextButton;
    public override void handle(GameController gameController, Information information)
    {
        Debug.Log("CpuNextButtonPresenter");
        cpuNextButton.SetActive(true);
    }
}
