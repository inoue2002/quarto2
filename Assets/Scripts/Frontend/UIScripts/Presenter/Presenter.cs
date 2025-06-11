using UnityEngine;

public  abstract class Presenter : MonoBehaviour
{
    public abstract void handle(GameController gameController, Information information);
}
