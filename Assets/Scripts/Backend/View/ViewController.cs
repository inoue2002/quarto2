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
    public GameController gameController;
    public void execute(Command command)
    {
        Result result = gameController.execute(command);
        Debug.Log("result.currentGamePhase:" + result.currentGamePhase);
        //ここでバックエンドに情報を送って、バックエンドから情報を受け取る
        if(result is SelectPlayerResult){
            executers[0].execute(gameController, result);
        }
        else if(result is SelectPieceResult){
            executers[1].execute(gameController, result);
        }
        else if(result is PutPieceResult){
            executers[3].execute(gameController, result);
        }
        getInfo();
        
    }

    public Information getInfo()
    {
        Information information =  gameController.getInformation();
        // Debug.Log("gamePhaseType:" + gameController.currentPhase.type);
        // ここでバックエンドから取得した情報をもとにUIを更新する
        presenters[GamePhaseIndex[gameController.currentPhase.type]].handle(gameController, information);

        return information;

    }
}