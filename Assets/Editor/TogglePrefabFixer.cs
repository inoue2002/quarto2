using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

namespace QuartoEditor
{
    public class TogglePrefabFixer : EditorWindow
    {
        private Toggle[] togglePrefabs;
        
        [MenuItem("Tools/UI/Toggle Prefab Fixer")]
        public static void ShowWindow()
        {
            GetWindow<TogglePrefabFixer>("Toggle Prefab Fixer");
        }
        
        private void OnGUI()
        {
            EditorGUILayout.LabelField("Toggle Prefab Fixer", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            
            EditorGUILayout.HelpBox("このツールはトグルプレハブのisOn状態をfalseに修正します。\n対象となるトグルプレハブを指定してください。", MessageType.Info);
            
            EditorGUILayout.Space();
            
            SerializedObject so = new SerializedObject(this);
            SerializedProperty togglesProp = so.FindProperty("togglePrefabs");
            EditorGUILayout.PropertyField(togglesProp, new GUIContent("Toggle Prefabs"), true);
            so.ApplyModifiedProperties();
            
            EditorGUILayout.Space();
            
            if (GUILayout.Button("Fix Selected Toggle Prefabs"))
            {
                FixTogglePrefabs();
            }
            
            if (GUILayout.Button("Fix All Toggle Prefabs in Project"))
            {
                FindAndFixAllTogglePrefabs();
            }
        }
        
        private void FixTogglePrefabs()
        {
            if (togglePrefabs == null || togglePrefabs.Length == 0)
            {
                EditorUtility.DisplayDialog("エラー", "トグルプレハブが選択されていません。", "OK");
                return;
            }
            
            int fixedCount = 0;
            
            foreach (Toggle toggle in togglePrefabs)
            {
                if (toggle == null) continue;
                
                // プレハブのパスを取得
                string prefabPath = AssetDatabase.GetAssetPath(toggle);
                if (string.IsNullOrEmpty(prefabPath)) continue;
                
                // プレハブアセットをロード
                GameObject prefabAsset = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                if (prefabAsset == null) continue;
                
                // プレハブのルートオブジェクトのトグルコンポーネントを取得
                Toggle rootToggle = prefabAsset.GetComponent<Toggle>();
                if (rootToggle != null)
                {
                    // isOnをfalseに設定
                    SerializedObject serializedObject = new SerializedObject(rootToggle);
                    SerializedProperty isOnProperty = serializedObject.FindProperty("m_IsOn");
                    
                    if (isOnProperty != null && isOnProperty.boolValue)
                    {
                        isOnProperty.boolValue = false;
                        serializedObject.ApplyModifiedProperties();
                        EditorUtility.SetDirty(prefabAsset);
                        fixedCount++;
                    }
                }
                
                // 子オブジェクトのトグルも修正
                foreach (Toggle childToggle in prefabAsset.GetComponentsInChildren<Toggle>(true))
                {
                    if (childToggle != rootToggle)
                    {
                        SerializedObject serializedObject = new SerializedObject(childToggle);
                        SerializedProperty isOnProperty = serializedObject.FindProperty("m_IsOn");
                        
                        if (isOnProperty != null && isOnProperty.boolValue)
                        {
                            isOnProperty.boolValue = false;
                            serializedObject.ApplyModifiedProperties();
                            EditorUtility.SetDirty(childToggle);
                            fixedCount++;
                        }
                    }
                }
            }
            
            if (fixedCount > 0)
            {
                AssetDatabase.SaveAssets();
                EditorUtility.DisplayDialog("完了", fixedCount + "個のトグルプレハブを修正しました。", "OK");
            }
            else
            {
                EditorUtility.DisplayDialog("情報", "修正が必要なトグルプレハブはありませんでした。", "OK");
            }
        }
        
        private void FindAndFixAllTogglePrefabs()
        {
            // プロジェクト内のすべてのトグルプレハブを検索
            string[] guids = AssetDatabase.FindAssets("t:Prefab");
            int fixedCount = 0;
            
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                
                if (prefab == null) continue;
                
                Toggle[] toggles = prefab.GetComponentsInChildren<Toggle>(true);
                bool prefabModified = false;
                
                foreach (Toggle toggle in toggles)
                {
                    SerializedObject serializedObject = new SerializedObject(toggle);
                    SerializedProperty isOnProperty = serializedObject.FindProperty("m_IsOn");
                    
                    if (isOnProperty != null && isOnProperty.boolValue)
                    {
                        isOnProperty.boolValue = false;
                        serializedObject.ApplyModifiedProperties();
                        EditorUtility.SetDirty(toggle);
                        fixedCount++;
                        prefabModified = true;
                    }
                }
                
                if (prefabModified)
                {
                    EditorUtility.SetDirty(prefab);
                }
            }
            
            if (fixedCount > 0)
            {
                AssetDatabase.SaveAssets();
                EditorUtility.DisplayDialog("完了", "プロジェクト内の" + fixedCount + "個のトグルを修正しました。", "OK");
            }
            else
            {
                EditorUtility.DisplayDialog("情報", "修正が必要なトグルプレハブはありませんでした。", "OK");
            }
        }
    }
} 