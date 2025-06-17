using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameController
{
    public Board board;
    public GamePhase currentPhase;
    public List<Player> players=new List<Player>();
    public List<PlayerInfo> playerInfos=new List<PlayerInfo>();

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
        }
        catch (System.Exception e)
        {
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
        Debug.Log($"=== コマンド実行 ===");
        Debug.Log($"現在のフェーズ: {currentPhase.type}");
        Debug.Log($"実行コマンド: {command.GetType().Name}");
        
        Result result = currentPhase.execute(command,this);
        
        GamePhase oldPhase = currentPhase;
        currentPhase = currentPhase.getNextPhase(this);
        
        Debug.Log($"フェーズ遷移: {oldPhase.type} → {currentPhase.type}");
        
        result.currentGamePhase = currentPhase.type;

        return result;
    }
    
    // Resultの成功状態を取得するヘルパーメソッド
    private bool GetResultSuccess(Result result)
    {
        if (result is SelectPieceResult selectResult) return selectResult.success;
        if (result is PutPieceResult putResult) return putResult.success;
        if (result is SelectPlayerResult playerResult) return true; // SelectPlayerResultにはsuccessフィールドがない場合
        return false;
    }

    public void setPlayerInfo(PlayerInfo playerInfo)
    {
        this.playerInfos.Add(playerInfo);
        
        Debug.Log("=== プレイヤー情報設定 ===");
        Debug.Log($"プレイヤー{playerInfos.Count}: SelectPiece={playerInfo.SelectPiece}, PutPiece={playerInfo.PutPiece}");
        Debug.Log($"現在の登録プレイヤー数: {playerInfos.Count}");
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
        
        Debug.Log("=== プレイヤータイプ取得 ===");
        Debug.Log($"現在のプレイヤー: {currentPlayer} (インデックス: {(int)currentPlayer})");
        Debug.Log($"要求されたアクション: {actionType}");
        
        if(actionType == ActionType.SelectPiece)
        {
            PlayerType selectType = playerInfos[(int)currentPlayer].SelectPiece;
            Debug.Log($"SelectPieceタイプ: {selectType}");
            return selectType;
        }
        else
        {
            PlayerType putType = playerInfos[(int)currentPlayer].PutPiece;
            Debug.Log($"PutPieceタイプ: {putType}");
            return putType;
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
        Information information = currentPhase.getInformation(this);
        information.type = currentPhase.type;
        return information;
    }
    
}

