using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectPlayerButton : MonoBehaviour
{
    public void OnHitoButtonClick()
    {
        // ViewController のインスタンスを取得
        ViewController viewController = ViewController.Instance;

        if (viewController != null)
        {
            // プレイヤー情報を設定するコマンドを作成
            SelectPlayerCommand command = new SelectPlayerCommand
            {
                playerInfo = new PlayerInfo
                {
                    SelectPiece = PlayerType.Player,
                    PutPiece = PlayerType.Player
                }
            };

            // ViewController を介して GameController にコマンドを送信
            viewController.CallGameController(command);

        }
        else
        {
            Debug.LogError("ViewController のインスタンスが見つかりません！");
        }
    }

    public void OnCPUButtonClick()
    {
        // ViewController のインスタンスを取得
        ViewController viewController = ViewController.Instance;

        if (viewController != null)
        {
            // プレイヤー情報を設定するコマンドを作成
            SelectPlayerCommand command = new SelectPlayerCommand
            {
                playerInfo = new PlayerInfo
                {
                    SelectPiece = PlayerType.Cpu,
                    PutPiece = PlayerType.Cpu
                }
            };

            // ViewController を介して GameController にコマンドを送信
            viewController.CallGameController(command);

        }
        else
        {
            Debug.LogError("ViewController のインスタンスが見つかりません！");
        }
    }

}
