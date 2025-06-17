using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 一人のプレイヤが手動か自動か選択するフェーズの後の処理
/// \n
/// なんでこの命名やねん
/// </summary>
public class SelectPlayerPhase : GamePhase
{
    private Dictionary<string,SelectPieceAlgorithm> selectPieceAlgorithms = new Dictionary<string, SelectPieceAlgorithm>();
    private Dictionary<string,PutPieceAlgorithm> putPieceAlgorithms = new Dictionary<string, PutPieceAlgorithm>();
    public SelectPlayerPhase()
    {
        type = GamePhaseType.SelectPlayer;
        //人が書いたアルゴリズムを手書きで追加
        selectPieceAlgorithms.Add("defo",new ConcreteSelectPieceAlgorithm());
        putPieceAlgorithms.Add("defo",new ConcretePutPieceAlgorithm());
    }
    public override Result execute(Command command, GameController gameController)
    {
        PlayerInfo playerInfo = ((SelectPlayerCommand)command).playerInfo;
        Result result = SelectPlayerUseCase.handle(playerInfo);
        gameController.setPlayerInfo(playerInfo);
        SelectPieceAlgorithm selectPieceAlgorithm = null;
        PutPieceAlgorithm putPieceAlgorithm = null;

        //playerのインスタンスを生成して，gameControllerにセット
        if(playerInfo.SelectPiece == PlayerType.Cpu)
        {
            Debug.Log("SelectPieceAlgorithmName: " + ((SelectPlayerCommand)command).SelectPieceAlgorithmName);
            selectPieceAlgorithm=selectPieceAlgorithms[ ((SelectPlayerCommand)command).SelectPieceAlgorithmName];
        }
        if(playerInfo.PutPiece == PlayerType.Cpu)
        {
            Debug.Log("PutPieceAlgorithmName: " + ((SelectPlayerCommand)command).PutPieceAlgorithmName);
            putPieceAlgorithm=putPieceAlgorithms[ ((SelectPlayerCommand)command).PutPieceAlgorithmName];
        }
        Player player = new Player(selectPieceAlgorithm,putPieceAlgorithm);
        gameController.setPlayer(player);
        //

        return result;
    }
    public override GamePhase getNextPhase(GameController gameController)
    {
        if(gameController.playerInfos.Count < 2)
        {
            return new SelectPlayerPhase();
        }
        else
        {
            Debug.Log("gameController.getPlayerType(ActionType.SelectPiece): " + gameController.getPlayerType(ActionType.SelectPiece));
            if(gameController.getPlayerType(ActionType.SelectPiece) == PlayerType.Cpu)
            {
                return new SelectPieceByCpuPhase();
            }
            else
            {
                return new SelectPieceByUserPhase();
            }
        }
    }
    public override Information getInformation(GameController gameController)
    {
        List<string> selectPieceAlgorithmNames = new List<string>();
        List<string> putPieceAlgorithmNames = new List<string>();
        foreach (var item in selectPieceAlgorithms)
        {
            selectPieceAlgorithmNames.Add(item.Key);
        }
        foreach (var item in putPieceAlgorithms)
        {
            putPieceAlgorithmNames.Add(item.Key);
        }

        return new SelectPlayerInformation(selectPieceAlgorithmNames,putPieceAlgorithmNames);
    }
}