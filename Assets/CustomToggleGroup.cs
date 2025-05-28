using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Events;

public class CustomToggleGroup : MonoBehaviour
{
    [SerializeField]
    private List<Toggle> toggles = new List<Toggle>();
    
    [SerializeField] 
    private bool allowSwitchOff = false;
    
    [SerializeField]
    private bool resetTogglesOnStart = true;
    
    private Toggle activeToggle = null;
    
    void Start()
    {
        InitializeToggles();
        
        if (resetTogglesOnStart)
        {
            SetAllTogglesOff();
        }
    }
    
    public void InitializeToggles()
    {
        // 既存のトグルをクリア
        toggles.Clear();
        
        // このオブジェクトの子オブジェクトからトグルを探す
        Toggle[] childToggles = GetComponentsInChildren<Toggle>(true);
        
        foreach (Toggle toggle in childToggles)
        {
            RegisterToggle(toggle);
        }
    }
    
    public void RegisterToggle(Toggle toggle)
    {
        if (toggle == null || toggles.Contains(toggle))
            return;
            
        toggles.Add(toggle);
        
        // リスナーをいったんクリア
        toggle.onValueChanged.RemoveAllListeners();
        
        // 新しいリスナーを追加
        toggle.onValueChanged.AddListener((isOn) => {
            if (isOn)
            {
                // このトグルをアクティブに設定
                SetActiveToggle(toggle);
            }
            else if (!allowSwitchOff && toggle == activeToggle)
            {
                // スイッチオフが許可されていない場合、再びオンにする
                toggle.isOn = true;
            }
            else if (toggle == activeToggle)
            {
                activeToggle = null;
            }
        });
        
        // 初期状態で選択されている場合はアクティブトグルとして設定
        if (toggle.isOn)
        {
            SetActiveToggle(toggle);
        }
    }
    
    public void SetActiveToggle(Toggle toggle)
    {
        if (!toggles.Contains(toggle))
            return;
            
        // 前のアクティブトグルをオフにする
        if (activeToggle != null && activeToggle != toggle)
        {
            activeToggle.isOn = false;
        }
        
        activeToggle = toggle;
    }
    
    public Toggle GetActiveToggle()
    {
        return activeToggle;
    }
    
    // 全てのトグルをオフにする
    public void SetAllTogglesOff()
    {
        // 一時的にスイッチオフを許可
        bool oldAllowSwitchOff = allowSwitchOff;
        allowSwitchOff = true;
        
        // 既存のリスナーの前に一時的にリスナーを無効化して処理
        foreach (Toggle toggle in toggles)
        {
            if (toggle != null)
            {
                toggle.isOn = false;
            }
        }
        
        activeToggle = null;
        
        // 元の設定に戻す
        allowSwitchOff = oldAllowSwitchOff;
    }
} 