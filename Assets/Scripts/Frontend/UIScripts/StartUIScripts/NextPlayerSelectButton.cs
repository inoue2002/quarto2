using UnityEngine;

public class NextPlayerSelectButton : MonoBehaviour
{
    public void OnClick()
    {
       GameObject viewControllerObject = GameObject.Find("ViewController");
       ViewController viewController = viewControllerObject.GetComponent<ViewController>();
       viewController.execute(new NextPlayerSelectCommand());
    }
}
