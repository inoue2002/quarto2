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
            // 駒選択後は、相手プレイヤーがPutPieceを行う
            // 現在のプレイヤーがPlayer1なら、Player2がPutPieceする
            // 現在のプレイヤーがPlayer2なら、Player1がPutPieceする
            PlayerId currentPlayer = gameController.getBoard().getPlayerId();
            PlayerId putPiecePlayer = (currentPlayer == PlayerId.Player1) ? PlayerId.Player2 : PlayerId.Player1;
            
            Debug.Log($"駒選択成功 - 現在のプレイヤー: {currentPlayer}");
            Debug.Log($"次にPutPieceするプレイヤー: {putPiecePlayer}");
            
            // 相手プレイヤーのPutPieceタイプを確認
            PlayerType putPiecePlayerType = gameController.playerInfos[(int)putPiecePlayer].PutPiece;
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
        return new SelectPieceInformation(gameController.getBoard().getSelectablePieces());
    }
}