using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DecisionButtonHandler : MonoBehaviour
{
    // [Header("Player 1 Settings")]
    // public Toggle player1PutHumanToggle;
    // public Toggle player1PutCPUToggle;
    // public Toggle player1GiveHumanToggle;
    // public Toggle player1GiveCPUToggle;

    // [Header("Player 2 Settings")]
    // public Toggle player2PutHumanToggle;
    // public Toggle player2PutCPUToggle;
    // public Toggle player2GiveHumanToggle;
    // public Toggle player2GiveCPUToggle;

    // // Start is called once before the first execution of Update after the MonoBehaviour is created
    // void Start()
    // {
    //     // 現在のシーン名を取得
    //     string currentSceneName = SceneManager.GetActiveScene().name;
        
    //     // プレイヤー1の画面の場合、プレイヤー2の設定は非表示にする
    //     if (currentSceneName == "Player1SelectUI")
    //     {
    //         if (player2PutHumanToggle != null) player2PutHumanToggle.gameObject.SetActive(false);
    //         if (player2PutCPUToggle != null) player2PutCPUToggle.gameObject.SetActive(false);
    //         if (player2GiveHumanToggle != null) player2GiveHumanToggle.gameObject.SetActive(false);
    //         if (player2GiveCPUToggle != null) player2GiveCPUToggle.gameObject.SetActive(false);
    //     }
    // }

    // public void OnDecisionButtonClick()
    // {
    //     // 現在のシーン名を取得
    //     string currentSceneName = SceneManager.GetActiveScene().name;
        
    //     if (currentSceneName == "Player1SelectUI")
    //     {
    //         // プレイヤー1の情報を設定
    //         PlayerInfo player1Info = new PlayerInfo
    //         {
    //             PutPiece = player1PutHumanToggle.isOn ? PlayerType.Player : PlayerType.Cpu,
    //             SelectPiece = player1GiveHumanToggle.isOn ? PlayerType.Player : PlayerType.Cpu
    //         };

    //         // プレイヤー情報を設定するコマンドを作成
    //         SelectPlayerCommand command = new SelectPlayerCommand
    //         {
    //             playerInfo = player1Info
    //         };

    //         // ViewController を介して GameController にコマンドを送信
    //         // if (ViewController.Instance != null)
    //         // {
    //         //     ViewController.Instance.CallGameController(command);
    //         // }

    //         // プレイヤー2の選択画面に遷移
    //         SceneManager.LoadScene("Player2SelectUI");
    //     }
    //     else if (currentSceneName == "Player2SelectUI")
    //     {
    //         // プレイヤー2の情報を設定
    //         PlayerInfo player2Info = new PlayerInfo
    //         {
    //             PutPiece = player2PutHumanToggle.isOn ? PlayerType.Player : PlayerType.Cpu,
    //             SelectPiece = player2GiveHumanToggle.isOn ? PlayerType.Player : PlayerType.Cpu
    //         };

    //         // プレイヤー情報を設定するコマンドを作成
    //         SelectPlayerCommand command = new SelectPlayerCommand
    //         {
    //             playerInfo = player2Info
    //         };

    //         // ViewController を介して GameController にコマンドを送信
    //         // if (ViewController.Instance != null)
    //         // {
    //         //     ViewController.Instance.CallGameController(command);
    //         // }

    //         // Quartoゲーム画面に遷移
    //         SceneManager.LoadScene("SampleScene");
    //     }
    // }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }
}
