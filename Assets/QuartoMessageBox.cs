using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuartoMessageBox : MonoBehaviour
{
    // public GameObject messageBoxPanel;
    // public TextMeshProUGUI messageText;
    // public Button closeButton;

    // private static QuartoMessageBox instance;

    // private void Awake()
    // {
    //     // シングルトンパターン
    //     if (instance == null)
    //     {
    //         instance = this;
    //         DontDestroyOnLoad(gameObject);
    //     }
    //     else
    //     {
    //         Destroy(gameObject);
    //         return;
    //     }

    //     // 初期状態では非表示
    //     if (messageBoxPanel != null)
    //     {
    //         messageBoxPanel.SetActive(false);
    //     }

    //     // 閉じるボタンのリスナーを設定
    //     if (closeButton != null)
    //     {
    //         closeButton.onClick.AddListener(HideMessage);
    //     }
    // }

    // // メッセージを表示する静的メソッド
    // public static void ShowQuartoMessage()
    // {
    //     if (instance != null && instance.messageBoxPanel != null)
    //     {
    //         instance.messageBoxPanel.SetActive(true);
    //         if (instance.messageText != null)
    //         {
    //             instance.messageText.text = "Quarto!";
    //         }
    //     }
    // }

    // // メッセージを非表示にする
    // public void HideMessage()
    // {
    //     if (messageBoxPanel != null)
    //     {
    //         messageBoxPanel.SetActive(false);
    //     }
    // }
} 