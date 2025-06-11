using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class ViewController : MonoBehaviour
{
    // 全てのコンポーネントをここで取得
    public Presenter[] presenters;
    public Executer[] executers;
    public Dictionary<GamePhaseType, int> GamePhaseIndex = new Dictionary<GamePhaseType, int>(){
        {GamePhaseType.SelectPlayer, 0},
        {GamePhaseType.SelectPieceByUser, 1},
        {GamePhaseType.SelectPieceByCpu, 2},
        {GamePhaseType.PutPieceByUser, 3},
        {GamePhaseType.PutPieceByCpu, 4},
    };


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
        executers[GamePhaseIndex[result.currentGamePhase]].execute(gameController, command);
        

    }

    public Information getInfo()
    {
        Information information =  gameController.getInformation();
        // ここでバックエンドから取得した情報をもとにUIを更新する
        presenters[GamePhaseIndex[information.type]].handle(gameController, information);

        return information;

    }
}