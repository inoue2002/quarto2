using UnityEngine;

public class NextTurnButton : MonoBehaviour
{

    public ViewController viewController;
    public void OnClick()
    {
        if(viewController.getInfo().type == GamePhaseType.SelectPieceByCpu)
        {
            SelectPieceByCpuCommand selectPieceByCpuCommand = new SelectPieceByCpuCommand();
            viewController.execute(selectPieceByCpuCommand);
        }
        else if(viewController.getInfo().type == GamePhaseType.PutPieceByCpu)
        {
            PutPieceByCpuCommand putPieceByCpuCommand = new PutPieceByCpuCommand();
            viewController.execute(putPieceByCpuCommand);
        }
        else
        {
            Debug.Log("NextTurnButton clicked, but not in a valid phase");
        }


    }
}
