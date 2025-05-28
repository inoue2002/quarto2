using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// トグルコンポーネントのisOnプロパティを常にfalseにするコンポーネント
/// </summary>
[RequireComponent(typeof(Toggle))]
public class ToggleIsOnReset : MonoBehaviour
{
    private Toggle toggle;
    
    private void Awake()
    {
        toggle = GetComponent<Toggle>();
    }
    
    private void OnEnable()
    {
        // トグルをリセット
        if (toggle != null)
        {
            ResetToggle();
        }
    }
    
    private void Start()
    {
        // 開始時にも実行して確実にリセット
        ResetToggle();
    }
    
    private void ResetToggle()
    {
        // isOnをfalseに設定
        toggle.isOn = false;
        
        // グラフィックも強制的に非表示
        if (toggle.graphic != null)
        {
            toggle.graphic.gameObject.SetActive(false);
        }
    }
    
    // インスペクターでの変更をキャッチするため
    private void OnValidate()
    {
        if (Application.isPlaying) return;

#if UNITY_EDITOR
        ToggleResetEditorUtil.ResetToggleInEditor(gameObject);
#endif
    }
}

#if UNITY_EDITOR
/// <summary>
/// エディタ専用のトグルリセットユーティリティ
/// </summary>
public static class ToggleResetEditorUtil
{
    public static void ResetToggleInEditor(GameObject toggleObject)
    {
        if (toggleObject == null) return;
        
        Toggle toggle = toggleObject.GetComponent<Toggle>();
        if (toggle != null && toggle.isOn)
        {
            // エディター上でもisOnをfalseに設定
            toggle.isOn = false;
            
            // グラフィックを非表示
            if (toggle.graphic != null)
            {
                toggle.graphic.gameObject.SetActive(false);
            }
            
            // 変更を保存
            UnityEditor.EditorUtility.SetDirty(toggle);
        }
    }
}
#endif 