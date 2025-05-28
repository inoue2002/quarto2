using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;

namespace QuartoEditor
{
    public class ToggleResetHelper : EditorWindow
    {
        // ToggleIsOnResetクラスへの動的参照
        private System.Type toggleIsOnResetType;
        
        [MenuItem("Tools/UI/Toggle Reset Helper")]
        public static void ShowWindow()
        {
            GetWindow<ToggleResetHelper>("Toggle Reset Helper");
        }
        
        private void OnEnable()
        {
            // ToggleIsOnResetクラスを動的に検索
            toggleIsOnResetType = System.Type.GetType("ToggleIsOnReset, Assembly-CSharp");
            
            if (toggleIsOnResetType == null)
            {
                // 別の方法で検索
                foreach (Assembly assembly in System.AppDomain.CurrentDomain.GetAssemblies())
                {
                    toggleIsOnResetType = assembly.GetType("ToggleIsOnReset");
                    if (toggleIsOnResetType != null)
                        break;
                }
            }
        }
        
        private void OnGUI()
        {
            EditorGUILayout.LabelField("Toggle Reset Helper", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            
            if (toggleIsOnResetType == null)
            {
                EditorGUILayout.HelpBox("ToggleIsOnResetコンポーネントが見つかりません。\nプロジェクトをコンパイルしてから再試行してください。", MessageType.Error);
                return;
            }
            
            EditorGUILayout.HelpBox("このツールはシーン内のすべてのトグルまたは選択されたトグルに\nToggleIsOnResetコンポーネントを追加します。", MessageType.Info);
            
            EditorGUILayout.Space();
            
            if (GUILayout.Button("シーン内のすべてのトグルにリセッターを追加"))
            {
                AddResetterToAllToggles();
            }
            
            if (GUILayout.Button("選択されたトグルにリセッターを追加"))
            {
                AddResetterToSelectedToggles();
            }
            
            EditorGUILayout.Space();
            
            if (GUILayout.Button("シーン内のすべてのトグルを今すぐリセット"))
            {
                ResetAllTogglesInScene();
            }
            
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("このツールはトグルのPrefabも修正できます。\nPrefabにToggleIsOnResetコンポーネントを追加すると、\nそのプレハブを使用するすべてのトグルが初期状態で非選択になります。", MessageType.Info);
            
            EditorGUILayout.Space();
            
            if (GUILayout.Button("選択されたトグルプレハブを修正"))
            {
                FixSelectedTogglePrefabs();
            }
        }
        
        private void AddResetterToAllToggles()
        {
            if (toggleIsOnResetType == null)
            {
                EditorUtility.DisplayDialog("エラー", "ToggleIsOnResetコンポーネントが見つかりません。", "OK");
                return;
            }
            
            Toggle[] allToggles = Resources.FindObjectsOfTypeAll<Toggle>();
            int count = 0;
            
            foreach (Toggle toggle in allToggles)
            {
                // シーン内のオブジェクトのみを対象にする
                if (toggle.gameObject.scene.isLoaded)
                {
                    if (AddResetterToToggle(toggle))
                    {
                        count++;
                    }
                }
            }
            
            EditorUtility.DisplayDialog("完了", $"{count}個のトグルにToggleIsOnResetコンポーネントを追加しました。", "OK");
        }
        
        private void AddResetterToSelectedToggles()
        {
            if (toggleIsOnResetType == null)
            {
                EditorUtility.DisplayDialog("エラー", "ToggleIsOnResetコンポーネントが見つかりません。", "OK");
                return;
            }
            
            int count = 0;
            
            foreach (GameObject obj in Selection.gameObjects)
            {
                Toggle toggle = obj.GetComponent<Toggle>();
                if (toggle != null)
                {
                    if (AddResetterToToggle(toggle))
                    {
                        count++;
                    }
                }
                
                // 子オブジェクトのトグルも検索
                foreach (Toggle childToggle in obj.GetComponentsInChildren<Toggle>(true))
                {
                    if (AddResetterToToggle(childToggle))
                    {
                        count++;
                    }
                }
            }
            
            EditorUtility.DisplayDialog("完了", $"{count}個のトグルにToggleIsOnResetコンポーネントを追加しました。", "OK");
        }
        
        private bool AddResetterToToggle(Toggle toggle)
        {
            if (toggleIsOnResetType == null) return false;
            
            // すでにコンポーネントがあるかチェック
            if (toggle.gameObject.GetComponent(toggleIsOnResetType) == null)
            {
                // コンポーネントを追加
                Component resetter = Undo.AddComponent(toggle.gameObject, toggleIsOnResetType);
                
                // トグルをリセット
                toggle.isOn = false;
                if (toggle.graphic != null)
                {
                    toggle.graphic.gameObject.SetActive(false);
                }
                
                EditorUtility.SetDirty(toggle.gameObject);
                return true;
            }
            
            return false;
        }
        
        private void ResetAllTogglesInScene()
        {
            Toggle[] allToggles = Resources.FindObjectsOfTypeAll<Toggle>();
            int count = 0;
            
            foreach (Toggle toggle in allToggles)
            {
                // シーン内のオブジェクトのみを対象にする
                if (toggle.gameObject.scene.isLoaded)
                {
                    toggle.isOn = false;
                    if (toggle.graphic != null)
                    {
                        toggle.graphic.gameObject.SetActive(false);
                    }
                    
                    EditorUtility.SetDirty(toggle.gameObject);
                    count++;
                }
            }
            
            EditorUtility.DisplayDialog("完了", $"{count}個のトグルをリセットしました。", "OK");
        }
        
        private void FixSelectedTogglePrefabs()
        {
            int count = 0;
            List<GameObject> prefabsToSave = new List<GameObject>();
            
            foreach (GameObject obj in Selection.gameObjects)
            {
                // プレハブかどうかチェック
                if (PrefabUtility.IsPartOfAnyPrefab(obj))
                {
                    string prefabPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(obj);
                    if (!string.IsNullOrEmpty(prefabPath))
                    {
                        // プレハブアセットをロード
                        GameObject prefabAsset = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                        if (prefabAsset != null)
                        {
                            // トグルを検索
                            Toggle[] toggles = prefabAsset.GetComponentsInChildren<Toggle>(true);
                            foreach (Toggle toggle in toggles)
                            {
                                // isOnプロパティを変更
                                SerializedObject serializedObject = new SerializedObject(toggle);
                                SerializedProperty isOnProperty = serializedObject.FindProperty("m_IsOn");
                                
                                if (isOnProperty != null && isOnProperty.boolValue)
                                {
                                    isOnProperty.boolValue = false;
                                    serializedObject.ApplyModifiedProperties();
                                    EditorUtility.SetDirty(toggle);
                                    count++;
                                }
                                
                                // ToggleIsOnResetコンポーネントの確認は省略（プレハブでは直接追加できない）
                            }
                            
                            if (!prefabsToSave.Contains(prefabAsset))
                            {
                                prefabsToSave.Add(prefabAsset);
                            }
                        }
                    }
                }
            }
            
            // 変更を保存
            if (prefabsToSave.Count > 0)
            {
                AssetDatabase.SaveAssets();
                EditorUtility.DisplayDialog("完了", $"{count}個のトグルの初期値を変更しました。\n{prefabsToSave.Count}個のプレハブを保存しました。", "OK");
            }
            else
            {
                EditorUtility.DisplayDialog("情報", "修正が必要なトグルプレハブはありませんでした。", "OK");
            }
        }
    }
} 