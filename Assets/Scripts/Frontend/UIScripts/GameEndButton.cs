using UnityEngine;
using UnityEngine.UI;

public class GameEndButton : MonoBehaviour
{

    public GameObject GameEndCanvas;

        
    public void OnClick()
    {
        Application.LoadLevel("SampleScene");

    }
}
