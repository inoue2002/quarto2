using UnityEngine;

public class PutPieceByCpuPhase : GamePhase
{

    public bool endFlag = false;
    private bool success = false;
    public PutPieceByCpuPhase()
    {
        type = GamePhaseType.PutPieceByCpu;
    }
    public override Result execute(Command command, GameController gameController)
    {
        Board board = gameController.getBoard();
        PutPieceUseCase putPieceUseCase = new PutPieceUseCase();
        Player player = gameController.getPlayer();
        Position position = player.putPiece(board);
        PutPieceResult result = (PutPieceResult)putPieceUseCase.handle(board, board.getSelectedPieceId(), position);
        if (result.success)
        {
            success = true;
        }
        else
        {
            success = false;
        }
        if (result.winner == PlayerId.None)
        {
            endFlag = false;
        }
        else
        {
            endFlag = true;
        }
        return result;
    }
    public override GamePhase getNextPhase(GameController gameController)
    {
        Debug.Log("=== PutPieceByCpuPhase: 次フェーズ決定 ===");
        
        if (!success)
        {
            Debug.Log("CPU駒配置失敗 → PutPieceByCpuPhaseを継続");
            return new PutPieceByCpuPhase();
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
            Debug.Log($"CPU駒配置成功 - 現在のプレイヤー（交代後）: {currentPlayer}");
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
        return new Information();
    }
}