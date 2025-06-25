using UnityEngine;

public class FreeViewButton : MonoBehaviour
{
    public GameObject GameEndPnael;

    public void OnClick()
    {
        if(GameEndPnael.activeSelf){
            GameEndPnael.SetActive(false);
        }
        else{
            GameEndPnael.SetActive(true);
        }
    }
}
