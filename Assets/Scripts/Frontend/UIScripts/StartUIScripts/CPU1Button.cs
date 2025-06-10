using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CPU1Button : SelectPlayerButton
{
    public GameController gameController; // GameController の参照

    void Start()
    {
        // GetComponent<Button>().onClick.AddListener(OnCPUButtonClick);
        // // 次のシーンをロード
        SceneManager.LoadScene("Player2SelectUI");
    }

    
}
