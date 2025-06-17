using UnityEngine;
/// <summary>
/// 
/// </summary>
public class SelectPieceByCpuPhase : GamePhase
{
    private bool success = false;
    public SelectPieceByCpuPhase()
    {
        type = GamePhaseType.SelectPieceByCpu;
    }
    public override Result execute(Command command, GameController gameController)
    {
        Player player = gameController.getPlayer();
        PieceId pieceId = player.selectPiece(gameController.getBoard());
        SelectPieceResult result = (SelectPieceResult)SelectPieceUseCase.handle(gameController.getBoard(), pieceId);
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
        Debug.Log("=== SelectPieceByCpuPhase: 次フェーズ決定 ===");
        
        if (!success)
        {
            Debug.Log("CPU駒選択失敗 → SelectPieceByCpuPhaseを継続");
            return new SelectPieceByCpuPhase();
        }
        else
        {
            // 駒選択後は、相手プレイヤーがPutPieceを行う
            PlayerId currentPlayer = gameController.getBoard().getPlayerId();
            PlayerId putPiecePlayer = (currentPlayer == PlayerId.Player1) ? PlayerId.Player2 : PlayerId.Player1;
            
            Debug.Log($"CPU駒選択成功 - 現在のプレイヤー: {currentPlayer}");
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
    public override Information getInformation(GameController gameController)
    {
        return new SelectPieceInformation(gameController.getBoard().getSelectablePieces());
    }
}