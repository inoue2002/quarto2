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

        
        if (!success)
        {

            return new SelectPieceByUserPhase();
        }
        else
        {
            PlayerType putPiecePlayerType = gameController.getPlayerType(ActionType.PutPiece);

            
            if (putPiecePlayerType == PlayerType.Cpu)
            {

                return new PutPieceByCpuPhase();
            }
            else
            {

                return new PutPieceByUserPhase();
            }
        }
    }
    public override Information getInformation(GameController gameController)//選択できる駒を返してあげる
    {
        return new SelectPieceInformation(gameController.getBoard().getSelectablePieces());
    }
}