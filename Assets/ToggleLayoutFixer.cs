using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToggleLayoutFixer : MonoBehaviour
{
    // [Header("Put Toggles")]
    // public Toggle[] putToggles;
    
    // [Header("Give Toggles")]
    // public Toggle[] giveToggles;
    
    // [Header("Layout Settings")]
    // public float toggleSpacing = 60f; // トグル間の垂直間隔
    // public float groupSpacing = 100f; // PutグループとGiveグループの間隔
    // public Vector2 toggleSize = new Vector2(40f, 40f); // トグルの大きさ
    // public Vector2 labelOffset = new Vector2(50f, 0f); // ラベルのオフセット
    
    // void Start()
    // {
    //     FixToggleLayout();
    // }
    
    // public void FixToggleLayout()
    // {
    //     // Putトグルのレイアウト修正
    //     FixTogglesGroup(putToggles, 0);
        
    //     // Giveトグルのレイアウト修正
    //     FixTogglesGroup(giveToggles, 1);
    // }
    
    // private void FixTogglesGroup(Toggle[] toggles, int groupIndex)
    // {
    //     if (toggles == null || toggles.Length == 0) return;
        
    //     for (int i = 0; i < toggles.Length; i++)
    //     {
    //         if (toggles[i] == null) continue;
            
    //         RectTransform rectTransform = toggles[i].GetComponent<RectTransform>();
    //         if (rectTransform != null)
    //         {
    //             // トグルの位置を設定
    //             float yPos = -i * toggleSpacing;
    //             float xPos = groupIndex * groupSpacing;
                
    //             rectTransform.anchoredPosition = new Vector2(xPos, yPos);
                
    //             // トグルのサイズを設定
    //             rectTransform.sizeDelta = toggleSize;
                
    //             // トグルのヒットボックスを明確にする
    //             if (toggles[i].targetGraphic != null && toggles[i].targetGraphic.rectTransform != null)
    //             {
    //                 toggles[i].targetGraphic.rectTransform.sizeDelta = toggleSize * 0.8f;
    //             }
                
    //             // UGUIテキストの調整
    //             Text label = toggles[i].GetComponentInChildren<Text>();
    //             if (label != null)
    //             {
    //                 RectTransform labelRect = label.GetComponent<RectTransform>();
    //                 labelRect.anchoredPosition = labelOffset;
    //             }
                
    //             // TMProテキストの調整
    //             TMP_Text tmpLabel = toggles[i].GetComponentInChildren<TMP_Text>();
    //             if (tmpLabel != null)
    //             {
    //                 RectTransform labelRect = tmpLabel.GetComponent<RectTransform>();
    //                 labelRect.anchoredPosition = labelOffset;
    //             }
    //         }
    //     }
    // }
} 