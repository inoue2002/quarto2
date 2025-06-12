using System.Collections.Concurrent;

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
        if (!success)
        {
            return new PutPieceByUserPhase();
        }
        if (endFlag)
        {
            return new GameEndPhase();
        }
        else
        {
            if (gameController.getPlayerType(ActionType.SelectPiece) == PlayerType.Cpu)
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
        return new Information();
    }
}