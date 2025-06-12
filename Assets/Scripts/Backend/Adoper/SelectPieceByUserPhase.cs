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
            Debug.LogError($"SelectPieceByUserPhase: 間違ったコマンドタイプを受信しました: {command?.GetType().Name ?? "null"}");
            SelectPieceResult errorResult = new SelectPieceResult();
            errorResult.success = false;
            return errorResult;
        }
        
        Debug.Log("SelectPieceByUserPhase: " + selectPieceByUserCommand.pieceId);
        SelectPieceResult result = SelectPieceUseCase.handle(gameController.getBoard(), selectPieceByUserCommand.pieceId) as SelectPieceResult;
        
        Debug.Log($"SelectPieceByUserPhase: SelectPieceUseCase実行結果 = success: {result.success}");
        
        if (result.success)
        {
            success = true;
            Debug.Log("SelectPieceByUserPhase: ピース選択成功、successをtrueに設定");
        }
        else
        {
            success = false;
            Debug.Log("SelectPieceByUserPhase: ピース選択失敗、successをfalseに設定");
        }
        return result;
    }
    public override GamePhase getNextPhase(GameController gameController)
    {
        Debug.Log($"SelectPieceByUserPhase.getNextPhase: success = {success}");
        
        if (!success)
        {
            Debug.Log("SelectPieceByUserPhase.getNextPhase: ピース選択が失敗したため、SelectPieceByUserPhaseを継続");
            return new SelectPieceByUserPhase();
        }
        else
        {
            PlayerType putPiecePlayerType = gameController.getPlayerType(ActionType.PutPiece);
            Debug.Log($"SelectPieceByUserPhase.getNextPhase: PutPieceプレイヤータイプ = {putPiecePlayerType}");
            
            if (putPiecePlayerType == PlayerType.Cpu)
            {
                Debug.Log("SelectPieceByUserPhase.getNextPhase: CPUが駒を置くため、PutPieceByCpuPhaseに遷移");
                return new PutPieceByCpuPhase();
            }
            else
            {
                Debug.Log("SelectPieceByUserPhase.getNextPhase: ユーザーが駒を置くため、PutPieceByUserPhaseに遷移");
                return new PutPieceByUserPhase();
            }
        }
    }
    public override Information getInformation(GameController gameController)//選択できる駒を返してあげる
    {
        return new SelectPieceInformation(gameController.getBoard().getSelectablePieces());
    }
}