using UnityEngine;
using UnityEditor;

/// <summary>
/// エディタフォルダを確立するためのダミースクリプト
/// </summary>
public class CreateEditorFolder : ScriptableObject
{
    // このスクリプトはエディタフォルダの存在を確立するためだけのものです
    
    // MenuItem属性を追加してエディタメニューに項目を表示
    [MenuItem("Help/About Quarto Editor", false, 100)]
    public static void ShowAboutWindow()
    {
        EditorUtility.DisplayDialog("Quartoエディタツール", 
            "このプロジェクトにはトグルプレハブ修正ツールが含まれています。\n\n" +
            "Tools > UI > Toggle Prefab Fixerからアクセスできます。", 
            "OK");
    }
} 