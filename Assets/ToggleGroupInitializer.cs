using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// ToggleGroupを初期化し、グループ内で1つだけトグルを選択できるようにするコンポーネント
/// </summary>
public class ToggleGroupInitializer : MonoBehaviour
{
    // [Header("Toggle Groups")]
    // public GameObject putToggleGroup;  // Put用のトグルを格納する親オブジェクト
    // public GameObject giveToggleGroup; // Give用のトグルを格納する親オブジェクト
    
    // [Header("Toggle Initialization")]
    // public bool resetOnStart = true;   // 開始時にトグルをリセットするか
    
    // private List<Toggle> putToggles = new List<Toggle>();
    // private List<Toggle> giveToggles = new List<Toggle>();
    
    // private Toggle activePutToggle = null;
    // private Toggle activeGiveToggle = null;
    
    // private void Awake()
    // {
    //     // Put用トグルの収集と初期化
    //     if (putToggleGroup != null)
    //     {
    //         CollectToggles(putToggleGroup, putToggles);
    //         InitializeToggles(putToggles, ToggleType.Put);
    //     }
        
    //     // Give用トグルの収集と初期化
    //     if (giveToggleGroup != null)
    //     {
    //         CollectToggles(giveToggleGroup, giveToggles);
    //         InitializeToggles(giveToggles, ToggleType.Give);
    //     }
    // }
    
    // // トグルタイプを定義する列挙型
    // private enum ToggleType
    // {
    //     Put,
    //     Give
    // }
    
    // private void Start()
    // {
    //     if (resetOnStart)
    //     {
    //         ResetAllToggles();
    //     }
    // }
    
    // // 指定されたオブジェクト内のすべてのトグルを収集
    // private void CollectToggles(GameObject parent, List<Toggle> toggleList)
    // {
    //     toggleList.Clear();
    //     Toggle[] toggles = parent.GetComponentsInChildren<Toggle>(true);
    //     foreach (Toggle toggle in toggles)
    //     {
    //         toggleList.Add(toggle);
    //     }
    // }
    
    // // トグルを初期化し、グループ内で1つだけ選択できるように設定
    // private void InitializeToggles(List<Toggle> toggleList, ToggleType toggleType)
    // {
    //     foreach (Toggle toggle in toggleList)
    //     {
    //         // リスナーをクリア
    //         toggle.onValueChanged.RemoveAllListeners();
            
    //         // リスナーを追加
    //         Toggle capturedToggle = toggle; // クロージャのためにキャプチャ
    //         toggle.onValueChanged.AddListener((isOn) => {
    //             if (isOn)
    //             {
    //                 // このトグルがオンになった場合、同じグループの他のトグルをオフにする
    //                 SetActiveToggle(toggleList, capturedToggle, toggleType);
    //             }
    //         });
            
    //         // 初期状態はオフ
    //         toggle.isOn = false;
            
    //         // グラフィックも非表示に
    //         if (toggle.graphic != null)
    //         {
    //             toggle.graphic.gameObject.SetActive(false);
    //         }
    //     }
    // }
    
    // // アクティブなトグルを設定
    // private void SetActiveToggle(List<Toggle> toggleList, Toggle selectedToggle, ToggleType toggleType)
    // {
    //     // 他のトグルをオフにする
    //     foreach (Toggle toggle in toggleList)
    //     {
    //         if (toggle != selectedToggle && toggle.isOn)
    //         {
    //             // イベントを発生させないように一時的にリスナーを無効化
    //             toggle.onValueChanged.SetPersistentListenerState(0, UnityEngine.Events.UnityEventCallState.Off);
    //             toggle.isOn = false;
    //             toggle.onValueChanged.SetPersistentListenerState(0, UnityEngine.Events.UnityEventCallState.RuntimeOnly);
    //         }
    //     }
        
    //     // 新しいアクティブトグルを設定（refではなく直接プロパティに設定）
    //     if (toggleType == ToggleType.Put)
    //     {
    //         activePutToggle = selectedToggle;
    //     }
    //     else // Give
    //     {
    //         activeGiveToggle = selectedToggle;
    //     }
    // }
    
    // // すべてのトグルをリセット
    // public void ResetAllToggles()
    // {
    //     ResetToggles(putToggles);
    //     ResetToggles(giveToggles);
    //     activePutToggle = null;
    //     activeGiveToggle = null;
    // }
    
    // private void ResetToggles(List<Toggle> toggleList)
    // {
    //     foreach (Toggle toggle in toggleList)
    //     {
    //         // イベントを発生させないように一時的にリスナーを無効化
    //         toggle.onValueChanged.SetPersistentListenerState(0, UnityEngine.Events.UnityEventCallState.Off);
    //         toggle.isOn = false;
    //         toggle.onValueChanged.SetPersistentListenerState(0, UnityEngine.Events.UnityEventCallState.RuntimeOnly);
            
    //         // グラフィックも非表示に
    //         if (toggle.graphic != null)
    //         {
    //             toggle.graphic.gameObject.SetActive(false);
    //         }
    //     }
    // }
    
    // // 選択されているトグルを取得
    // public Toggle GetActivePutToggle()
    // {
    //     return activePutToggle;
    // }
    
    // public Toggle GetActiveGiveToggle()
    // {
    //     return activeGiveToggle;
    // }
    
    // // いずれかのトグルが選択されているかを確認
    // public bool HasSelection()
    // {
    //     return activePutToggle != null && activeGiveToggle != null;
    // }
} 