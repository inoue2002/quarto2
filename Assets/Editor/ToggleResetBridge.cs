using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace QuartoEditor
{
    /// <summary>
    /// エディタスクリプトとランタイムスクリプト間のブリッジクラス
    /// </summary>
    public static class ToggleResetBridge
    {
        /// <summary>
        /// トグルにToggleIsOnResetコンポーネントを追加する
        /// </summary>
        public static void AddToggleResetComponent(Toggle toggle)
        {
            if (toggle == null) return;
            
            // ToggleIsOnResetコンポーネントを追加
            var type = System.Type.GetType("ToggleIsOnReset, Assembly-CSharp");
            if (type == null)
            {
                // 別の方法で検索
                foreach (var assembly in System.AppDomain.CurrentDomain.GetAssemblies())
                {
                    type = assembly.GetType("ToggleIsOnReset");
                    if (type != null) break;
                }
                
                if (type == null)
                {
                    Debug.LogError("ToggleIsOnResetコンポーネントが見つかりません。プロジェクトをコンパイルしてください。");
                    return;
                }
            }
            
            // すでにコンポーネントがあるか確認
            if (toggle.gameObject.GetComponent(type) == null)
            {
                // コンポーネントを追加
                Undo.AddComponent(toggle.gameObject, type);
                toggle.isOn = false;
                
                if (toggle.graphic != null)
                {
                    toggle.graphic.gameObject.SetActive(false);
                }
                
                EditorUtility.SetDirty(toggle.gameObject);
            }
        }
        
        /// <summary>
        /// トグルのisOnプロパティをfalseに設定する
        /// </summary>
        public static void ResetToggle(Toggle toggle)
        {
            if (toggle == null) return;
            
            toggle.isOn = false;
            if (toggle.graphic != null)
            {
                toggle.graphic.gameObject.SetActive(false);
            }
            
            EditorUtility.SetDirty(toggle.gameObject);
        }
    }
} 