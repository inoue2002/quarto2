using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class Player2UIManager : MonoBehaviour
{
    [Header("Toggle Groups")]
    public GameObject putToggleGroup;  // Put用のトグルグループ
    public GameObject giveToggleGroup; // Give用のトグルグループ

    [Header("UI Components")]
    public Button decisionButton;      // 決定ボタン
    
    private ToggleGroupInitializer toggleGroupInitializer;
    
    void Awake()
    {
        // ToggleGroupInitializerがなければ追加
        toggleGroupInitializer = GetComponent<ToggleGroupInitializer>();
        if (toggleGroupInitializer == null)
        {
            toggleGroupInitializer = gameObject.AddComponent<ToggleGroupInitializer>();
            toggleGroupInitializer.putToggleGroup = putToggleGroup;
            toggleGroupInitializer.giveToggleGroup = giveToggleGroup;
        }
        
        // 決定ボタンにリスナーを設定
        if (decisionButton != null)
        {
            decisionButton.onClick.RemoveAllListeners();
            decisionButton.onClick.AddListener(OnDecisionButtonClicked);
        }
    }
    
    void Start()
    {
        // すべてのトグルをリセット
        if (toggleGroupInitializer != null)
        {
            toggleGroupInitializer.ResetAllToggles();
        }
    }
    
    void OnEnable()
    {
        // 画面が表示されたときにもリセット
        if (toggleGroupInitializer != null)
        {
            toggleGroupInitializer.ResetAllToggles();
        }
    }
    
    // 決定ボタンクリック時の処理
    public void OnDecisionButtonClicked()
    {
        if (toggleGroupInitializer.HasSelection())
        {
            // 選択状態を保存
            SavePlayerSelections();
            
            // ゲーム画面へ遷移
            SceneManager.LoadScene("GameScene");
        }
        else
        {
            Debug.LogWarning("プレイヤー2: PutとGiveの両方を選択してください");
            // ここで選択を促すメッセージを表示することも可能
        }
    }
    
    // プレイヤーの選択を保存
    private void SavePlayerSelections()
    {
        Toggle putToggle = toggleGroupInitializer.GetActivePutToggle();
        Toggle giveToggle = toggleGroupInitializer.GetActiveGiveToggle();
        
        if (putToggle != null && giveToggle != null)
        {
            // 選択された内容をPlayerPrefsまたはGameManagerなどに保存
            GameManager.Instance.Player2PutSelection = putToggle.gameObject.name;
            GameManager.Instance.Player2GiveSelection = giveToggle.gameObject.name;
            
            Debug.Log($"Player2: Put={putToggle.gameObject.name}, Give={giveToggle.gameObject.name}");
        }
    }
} 