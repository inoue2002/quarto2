using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameStartButton : MonoBehaviour
{
    public TextMeshProUGUI playerTurnText; // プレイヤーターンを表示するTextオブジェクト
    public Button gameStartButton; // GameStartボタン

    void Start()
    {
        // playerTurnText と gameStartButton が設定されているか確認
        if (playerTurnText == null)
        {
            Debug.LogError("Player Turn Text が設定されていません。インスペクターで設定してください。");
        }
        if (gameStartButton == null)
        {
            Debug.LogError("Game Start Button が設定されていません。インスペクターで設定してください。");
        }

        // ボタンがクリックされたときのリスナーを追加
        gameStartButton.onClick.AddListener(OnGameStartButtonClick);

        // プレイヤーのターンテキストを非表示にする
        playerTurnText.gameObject.SetActive(false);
    }

    void OnGameStartButtonClick()
    {
        // プレイヤーのターンテキストを表示し、内容を設定
        playerTurnText.gameObject.SetActive(true);
        playerTurnText.text = "Player1";

        // GameStartボタンを非表示にする
        gameStartButton.gameObject.SetActive(false);

        // 必要に応じて、ゲーム開始のロジックをここに追加
    }

    public void ChangePlayerTurnText(int player)
    {
        playerTurnText.text = $"Player{player}";
    }

    
}
