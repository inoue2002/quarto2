using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Hito2Button : SelectPlayerButton
{
    public GameController gameController; // GameController の参照

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // this.GetComponent<Button>().onClick.AddListener(OnHitoButtonClick);
        // 次のシーンへ移動
        SceneManager.LoadScene("SampleScene");
    }

}