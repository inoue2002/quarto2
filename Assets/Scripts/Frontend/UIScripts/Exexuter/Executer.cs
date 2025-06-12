using UnityEngine;

public abstract class Executer : MonoBehaviour
{
    public abstract void execute(GameController gameController, Result result);
}
