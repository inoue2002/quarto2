using System.Collections.Generic;
using UnityEngine;

public class SelectPieceByUserPhase : GamePhase
{

    private bool success = false;
    public SelectPieceByUserPhase()
    {
        type = GamePhaseType.SelectPieceByUser;
    }
    public override Result execute(Command command, GameController gameController)
    {

        SelectPieceByUserCommand selectPieceByUserCommand = command as SelectPieceByUserCommand;
        
        // nullチェックを追加
        if (selectPieceByUserCommand == null)
        {

            SelectPieceResult errorResult = new SelectPieceResult();
            errorResult.success = false;
            return errorResult;
        }
        

        SelectPieceResult result = SelectPieceUseCase.handle(gameController.getBoard(), selectPieceByUserCommand.pieceId) as SelectPieceResult;
        

        
        if (result.success)
        {
            success = true;

        }
        else
        {
            success = false;

        }
        return result;
    }
    public override GamePhase getNextPhase(GameController gameController)
    {
        Debug.Log("=== SelectPieceByUserPhase: 次フェーズ決定 ===");
        
        if (!success)
        {
            Debug.Log("駒選択失敗 → SelectPieceByUserPhaseを継続");
            return new SelectPieceByUserPhase();
        }
        else
        {
            // 駒選択後は現在のプレイヤー（交代済み）がPutPieceを行う
            PlayerId currentPlayer = gameController.getBoard().getPlayerId();
            
            Debug.Log($"駒選択成功 - 現在のプレイヤー（交代後）: {currentPlayer}");
            Debug.Log($"次にPutPieceするプレイヤー: {currentPlayer}");
            
            // 現在のプレイヤーのPutPieceタイプを確認
            PlayerType putPiecePlayerType = gameController.playerInfos[(int)currentPlayer].PutPiece;
            Debug.Log($"PutPieceプレイヤーのタイプ: {putPiecePlayerType}");
            
            if (putPiecePlayerType == PlayerType.Cpu)
            {
                Debug.Log("次はCPUのPutPiece → PutPieceByCpuPhaseへ");
                return new PutPieceByCpuPhase();
            }
            else
            {
                Debug.Log("次は人間のPutPiece → PutPieceByUserPhaseへ");
                return new PutPieceByUserPhase();
            }
        }
    }
    public override Information getInformation(GameController gameController)//選択できる駒を返してあげる
    {
        return new SelectPieceInformation(gameController.getBoard().getSelectablePieces(), gameController.getBoard().getPlayerId(), gameController.playerInfos[(int)gameController.getBoard().getPlayerId()].SelectPiece);
    }
}