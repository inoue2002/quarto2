using UnityEngine;

public class CpuNextButtonExecuter : Executer
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void execute(GameController gameController, Result result)
    {
        Debug.Log("CpuNextButtonExecuter");
    }
}
