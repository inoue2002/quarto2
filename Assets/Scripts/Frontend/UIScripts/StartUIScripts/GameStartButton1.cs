using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class GameStartButton1 : MonoBehaviour
{
    public void OnClick()
    {
        // コントローラーを取得
        GameObject viewControllerObject = GameObject.Find("ViewController");
        ViewController viewController = viewControllerObject.GetComponent<ViewController>();

        // ドロップダウンメニューを取得
        TMP_Dropdown CpuPutDropdown =GameObject.Find("CpuPutDropdown").GetComponent<TMP_Dropdown>();
        TMP_Dropdown CpuSelectDropdown =GameObject.Find("CpuSelectDropdown").GetComponent<TMP_Dropdown>();
        // トグルボタンを取得
        Toggle Put_human =GameObject.Find("Put_human").GetComponent<Toggle>();
        Toggle Select_human =GameObject.Find("Select_human").GetComponent<Toggle>();
        // コマンドを作成
        SelectPlayerCommand selectPlayerCommand = new SelectPlayerCommand();
        
        // プレイヤータイプを取得
        PlayerType putPlayerType = PlayerType.Cpu;
        PlayerType selectPlayerType = PlayerType.Cpu;

        // プレイヤータイプを設定
        if(Put_human.isOn){
            putPlayerType = PlayerType.Player;
        }
        else{
            putPlayerType = PlayerType.Cpu;
            selectPlayerCommand.PutPieceAlgorithmName = CpuPutDropdown.options[CpuPutDropdown.value].text;
        }

        if(Select_human.isOn){
            selectPlayerType = PlayerType.Player;
        }
        else{
            selectPlayerType = PlayerType.Cpu;
            selectPlayerCommand.SelectPieceAlgorithmName = CpuSelectDropdown.options[CpuSelectDropdown.value].text;
        }

        // プレイヤー情報を設定
        PlayerInfo playerInfo = new PlayerInfo(selectPlayerType, putPlayerType);
        selectPlayerCommand.playerInfo = playerInfo;
        // コマンドを実行
        viewController.execute(selectPlayerCommand);
        Debug.Log("selectPlayerCommand: " + selectPlayerCommand.playerInfo.SelectPiece + " " + selectPlayerCommand.playerInfo.PutPiece);


    }
}
