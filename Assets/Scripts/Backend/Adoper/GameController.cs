using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Board board;
    public GamePhase currentPhase;
    public List<Player> players=new List<Player>();
    public List<PlayerInfo> playerInfos=new List<PlayerInfo>();

    private void Awake()
    {
        try
        {
            initialize();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"GameController初期化中にエラーが発生しました: {e.Message}");
        }
    }

    public void initialize()
    {
        try
        {
            // ボードの初期化
            board = new Board();
            
            // 初期フェーズをSelectPlayerに設定
            currentPhase = new SelectPlayerPhase();
            
            // プレイヤーリストとプレイヤー情報リストをクリア
            if (players == null) players = new List<Player>();
            else players.Clear();
            
            if (playerInfos == null) playerInfos = new List<PlayerInfo>();
            else playerInfos.Clear();
            
            Debug.Log("GameControllerが初期化されました");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"initialize()メソッドでエラーが発生しました: {e.Message}");
            throw;
        }
    }
    
    /// <summary>
    /// currentフェーズの処理を実行し，フェーズを再設定
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public Result execute(Command command)
    {
        Result result = currentPhase.execute(command,this);
        currentPhase=currentPhase.getNextPhase(this);
        result.currentGamePhase = currentPhase.type;
        return result;
    }
    public void setPlayerInfo(PlayerInfo playerInfo)
    {
        this.playerInfos.Add(playerInfo);
    }
    public void setPlayer(Player player)
    {
        this.players.Add(player);
    }
    /// <summary>
    /// playerが手動か自動かを返す
    /// </summary>
    /// <param name="actionType"></param>
    /// <returns></returns>
    public PlayerType getPlayerType(ActionType actionType)
    {
        PlayerId currentPlayer=board.getPlayerId();
        if(actionType == ActionType.SelectPiece)
        {
            return playerInfos[(int)currentPlayer].SelectPiece;
        }
        else
        {
            return playerInfos[(int)currentPlayer].PutPiece;
        }
    }
    public Board getBoard()
    {
        return board;
    }
    public Player getPlayer()
    {
        return players[(int)board.getPlayerId()];
    }
    public Information getInformation()
    {
        return currentPhase.getInformation();
    }
}

