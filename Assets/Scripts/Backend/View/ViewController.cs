using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ViewController : MonoBehaviour
{
    // 全てのコンポーネントをここで取得
    public TMP_Dropdown[] CpuPutDropdowns;
    public TMP_Dropdown[] CpuSelectDropdowns;

    public Toggle[] CpuPutToggles;
    public Toggle[] CpuSelectToggles;

    public void Awake()
    {
        gameController =  new GameController();
        gameController.initialize();
        getInfo();
    }
    private GameController gameController;
    public void execute(Command command)
    {
        Result result = gameController.execute(command);
        //ここでバックエンドに情報を送って、バックエンドから情報を受け取る
        switch(result.currentGamePhase)
        {
            case GamePhaseType.SelectPlayer:
                getInfo();
                break;
            case GamePhaseType.SelectPieceByUser:
                break;
        }
    }

    public void getInfo()
    {
        Information information =  gameController.getInformation();
        // ここでバックエンドから取得した情報をもとにUIを更新する
        switch(information.type)
        {
            case GamePhaseType.SelectPlayer:
                // プレイヤー選択画面のUIを更新
                Debug.Log("プレイヤー選択画面のUIを更新");
                SelectPlayerInformation selectPlayerInformation = (SelectPlayerInformation)information;
                CpuPutDropdowns[0].options.Clear();
                foreach(string name in selectPlayerInformation.PutPieceAlgorithmNames)
                {
                    CpuPutDropdowns[0].options.Add(new TMP_Dropdown.OptionData(name));
                }
                CpuPutDropdowns[0].value = 0;
                CpuPutDropdowns[0].RefreshShownValue();
                CpuSelectDropdowns[0].options.Clear();
                foreach(string name in selectPlayerInformation.SelectPieceAlgorithmNames)
                {
                    CpuSelectDropdowns[0].options.Add(new TMP_Dropdown.OptionData(name));
                }
                CpuSelectDropdowns[0].value = 0;
                CpuSelectDropdowns[0].RefreshShownValue();
                break;
            case GamePhaseType.SelectPieceByUser:
                break;
            case GamePhaseType.SelectPieceByCpu:
                break;
            case GamePhaseType.PutPieceByUser:
                break;
            case GamePhaseType.PutPieceByCpu:
                break;
            case GamePhaseType.GameEnd:
                break;
        }

    }
}