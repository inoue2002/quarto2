using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectPlayerPresenter : Presenter
{
    public TMP_Dropdown CpuPutDropdown;
    public TMP_Dropdown CpuSelectDropdown;

    public Toggle CpuPutToggle;
    public Toggle CpuSelectToggle;

    public GameObject CanvasPlayer;


    public void Awake()
    {

    }
    public override void handle(GameController gameController, Information information){

                    // プレイヤー選択画面のUIを更新
            SelectPlayerInformation selectPlayerInformation = (SelectPlayerInformation)information;
            CpuPutDropdown.options.Clear();
            foreach(string name in selectPlayerInformation.PutPieceAlgorithmNames)
            {
                CpuPutDropdown.options.Add(new TMP_Dropdown.OptionData(name));
            }
            CpuPutDropdown.value = 0;
            CpuPutDropdown.RefreshShownValue();
            CpuSelectDropdown.options.Clear();
            foreach(string name in selectPlayerInformation.SelectPieceAlgorithmNames)
            {
                CpuSelectDropdown.options.Add(new TMP_Dropdown.OptionData(name));
            }
            CpuSelectDropdown.value = 0;
            CpuSelectDropdown.RefreshShownValue();


    }
}
