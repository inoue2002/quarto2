using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

#if UNITY_EDITOR
/// <summary>
/// エディタツール: UIのトグルの設定を支援するヘルパークラス
/// </summary>
[ExecuteInEditMode]
public class ToggleSetupHelper : MonoBehaviour
{
//     [Header("Target Objects")]
//     public GameObject putTogglesParent;
//     public GameObject giveTogglesParent;
    
//     [Header("Toggle Prefab")]
//     public Toggle togglePrefab;
    
//     [Header("Labels")]
//     public string[] toggleLabels = new string[] { "Human", "CPU 1", "CPU 2", "CPU 3", "CPU 4", "CPU 5" };
    
//     [Header("Layout Settings")]
//     public float verticalSpacing = 60f;
//     public float horizontalSpacing = 200f;
    
//     [Header("Setup Actions")]
//     [SerializeField, HideInInspector]
//     private bool setupToggles;
    
//     [SerializeField, HideInInspector]
//     private bool resetToggles;
    
//     private void OnValidate()
//     {
//         if (setupToggles)
//         {
//             setupToggles = false;
//             SetupToggles();
//         }
        
//         if (resetToggles)
//         {
//             resetToggles = false;
//             ResetAllToggles();
//         }
//     }
    
//     /// <summary>
//     /// 全てのトグルを作成し配置する
//     /// </summary>
//     public void SetupToggles()
//     {
//         if (putTogglesParent == null || giveTogglesParent == null || togglePrefab == null)
//         {
//             Debug.LogError("Setup failed: Missing references");
//             return;
//         }
        
//         // Putトグルの作成
//         CreateToggleGroup(putTogglesParent, "Put", 0);
        
//         // Giveトグルの作成
//         CreateToggleGroup(giveTogglesParent, "Give", 1);
        
//         // カスタムトグルグループの追加
//         AddCustomToggleGroup(putTogglesParent);
//         AddCustomToggleGroup(giveTogglesParent);
        
//         Debug.Log("Toggle setup completed!");
//     }
    
//     /// <summary>
//     /// トグルグループを作成する
//     /// </summary>
//     private void CreateToggleGroup(GameObject parent, string prefix, int groupIndex)
//     {
//         // 既存の子オブジェクトをクリア
//         while (parent.transform.childCount > 0)
//         {
//             DestroyImmediate(parent.transform.GetChild(0).gameObject);
//         }
        
//         // トグルを作成
//         for (int i = 0; i < toggleLabels.Length; i++)
//         {
//             Toggle newToggle = Instantiate(togglePrefab, parent.transform);
//             newToggle.name = prefix + "_" + toggleLabels[i] + "_Toggle";
            
//             // 位置を設定
//             RectTransform rectTransform = newToggle.GetComponent<RectTransform>();
//             rectTransform.anchoredPosition = new Vector2(groupIndex * horizontalSpacing, -i * verticalSpacing);
            
//             // ラベルを設定
//             Text label = newToggle.GetComponentInChildren<Text>();
//             if (label != null)
//             {
//                 label.text = toggleLabels[i];
//             }
            
//             // 初期状態は全てオフ
//             newToggle.isOn = false;
            
//             // レイキャストターゲットを有効に
//             if (newToggle.targetGraphic != null)
//             {
//                 Image targetImage = newToggle.targetGraphic as Image;
//                 if (targetImage != null)
//                 {
//                     targetImage.raycastTarget = true;
//                 }
//             }
//         }
//     }
    
//     /// <summary>
//     /// カスタムトグルグループを追加する
//     /// </summary>
//     private void AddCustomToggleGroup(GameObject parent)
//     {
//         CustomToggleGroup group = parent.GetComponent<CustomToggleGroup>();
//         if (group == null)
//         {
//             group = parent.AddComponent<CustomToggleGroup>();
//         }
//     }
    
//     /// <summary>
//     /// 全てのトグルをリセットする
//     /// </summary>
//     public void ResetAllToggles()
//     {
//         ResetTogglesInParent(putTogglesParent);
//         ResetTogglesInParent(giveTogglesParent);
//         Debug.Log("All toggles reset to OFF state");
//     }
    
//     private void ResetTogglesInParent(GameObject parent)
//     {
//         if (parent == null) return;
        
//         Toggle[] toggles = parent.GetComponentsInChildren<Toggle>();
//         foreach (Toggle toggle in toggles)
//         {
//             toggle.isOn = false;
//         }
//     }
// }

// /// <summary>
// /// エディタ拡張: ToggleSetupHelperのカスタムインスペクター
// /// </summary>
// [CustomEditor(typeof(ToggleSetupHelper))]
// public class ToggleSetupHelperEditor : Editor
// {
//     public override void OnInspectorGUI()
//     {
//         DrawDefaultInspector();
        
//         ToggleSetupHelper helper = (ToggleSetupHelper)target;
        
//         EditorGUILayout.Space();
//         EditorGUILayout.LabelField("Actions", EditorStyles.boldLabel);
        
//         if (GUILayout.Button("Setup Toggles"))
//         {
//             helper.SetupToggles();
//         }
        
//         if (GUILayout.Button("Reset All Toggles"))
//         {
//             helper.ResetAllToggles();
//         }
//     }
}
#endif 