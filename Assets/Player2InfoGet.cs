using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player2InfoGet : MonoBehaviour
{
    public Toggle cpuToggle; // CPU トグルの参照
    public Button decideButton; // 決定ボタンの参照

    void Start()
    {
        // 決定ボタンにクリックイベントを追加
        decideButton.onClick.AddListener(OnDecideButtonClick);
    }

    void OnDecideButtonClick()
    {
        // プレイヤー情報を設定
        PlayerInfo playerInfo = new PlayerInfo
        {
            SelectPiece = PlayerType.Player,
            PutPiece = cpuToggle.isOn ? PlayerType.Cpu : PlayerType.Player
        };

        // プレイヤー情報を設定するコマンドを作成
        SelectPlayerCommand command = new SelectPlayerCommand
        {
            playerInfo = playerInfo
        };

        // ViewController を介して GameController にコマンドを送信
        ViewController.Instance.CallGameController(command);

         // 次のシーンをロード
        SceneManager.LoadScene("SampleScene");
    }
}