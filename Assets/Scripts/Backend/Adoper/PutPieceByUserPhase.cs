using System.Collections.Concurrent;
using UnityEngine;

public class PutPieceByUserPhase : GamePhase
{

    public bool endFlag = false;
    private bool success=false;
    public PutPieceByUserPhase()
    {
        type = GamePhaseType.PutPieceByUser;
    }
    public override Result execute(Command command, GameController gameController)
    {
        Board board = gameController.getBoard();
        Position position = ((PutPieceByUserCommand)command).position;
        // PieceId pieceId = ((PutPieceByUserCommand)command).pieceId;
        PieceId pieceId = gameController.board.getSelectedPieceId();
        PutPieceUseCase putPieceUseCase = new PutPieceUseCase();
        PutPieceResult result =(PutPieceResult)putPieceUseCase.handle(board,pieceId,position);
        if(result.success){
            success = true;
        }
        else{
            success = false;
        }
        if(result.winner == PlayerId.None){
            endFlag = false;
        }
        else if(gameController.board.selectablePieces.Count == 0){
            endFlag = true;
        }
        else{
            endFlag = true;
        }
        return result;


    }
    public override GamePhase getNextPhase(GameController gameController)
    {
        Debug.Log("=== PutPieceByUserPhase: 次フェーズ決定 ===");
        
        if (!success)
        {
            Debug.Log("駒配置失敗 → PutPieceByUserPhaseを継続");
            return new PutPieceByUserPhase();
        }
        if (endFlag)
        {
            Debug.Log("ゲーム終了 → GameEndPhaseへ");
            return new GameEndPhase();
        }
        else
        {
            // PutPiece後はプレイヤーが交代済み
            // 現在のプレイヤーがSelectPieceを行う
            PlayerId currentPlayer = gameController.getBoard().getPlayerId();
            Debug.Log($"駒配置成功 - 現在のプレイヤー（交代後）: {currentPlayer}");
            Debug.Log($"次にSelectPieceするプレイヤー: {currentPlayer}");
            
            // 現在のプレイヤーのSelectPieceタイプを確認
            PlayerType selectPiecePlayerType = gameController.playerInfos[(int)currentPlayer].SelectPiece;
            Debug.Log($"SelectPieceプレイヤーのタイプ: {selectPiecePlayerType}");
            
            if (selectPiecePlayerType == PlayerType.Cpu)
            {
                Debug.Log("次はCPUのSelectPiece → SelectPieceByCpuPhaseへ");
                return new SelectPieceByCpuPhase();
            }
            else
            {
                Debug.Log("次は人間のSelectPiece → SelectPieceByUserPhaseへ");
                return new SelectPieceByUserPhase();
            }
        }
    }
    public override Information getInformation(GameController gameController)
    {
        PlayerId currentPlayerId = gameController.getBoard().getPlayerId();
        PlayerType currentPlayerType = gameController.playerInfos[(int)currentPlayerId].PutPiece;
        string algorithmName = "";
        
        if (currentPlayerType == PlayerType.Cpu)
        {
            algorithmName = gameController.getPlayer().PutPieceAlgorithmName;
        }
        
        return new PutPieceInformation(currentPlayerId, currentPlayerType, algorithmName);
    }
}