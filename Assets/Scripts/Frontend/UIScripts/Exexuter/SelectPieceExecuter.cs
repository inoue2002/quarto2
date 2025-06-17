using UnityEngine;

public class SelectPieceExecuter : Executer
{
    public override void execute(GameController gameController, Result result)
    {
        //カメラの向きを変える
        // Camera.main.transform.position = new Vector3(0, 0, 0);
        //commandのpieceIdのピースのオブジェクトを取得する
        // Debug.Log("result: " + result.currentGamePhase);
        
        // 型チェックを追加
        SelectPieceResult selectPieceResult = result as SelectPieceResult;
        if (selectPieceResult == null)
        {
            Debug.LogError($"SelectPieceExecuter: Expected SelectPieceResult but got {result.GetType().Name}");
            return;
        }

        if(!selectPieceResult.success)
        {
            return;
        }
         
        PieceId pieceId = selectPieceResult.pieceId;
        
        GameObject piece = GameObject.Find(pieceId.ToString());
        // Debug.Log("pieceId: " + pieceId);
        
        if (piece != null)
        {

            Debug.Log("piece: " + piece);
            Piece3D piece3D = piece.GetComponent<Piece3D>();
            if (piece3D != null)
            {
                piece3D.Select();
                // Debug.Log($"Piece {pieceId} selected successfully");
            }
            else
            {
                Debug.LogError($"SelectPieceExecuter: Piece3D component not found on GameObject '{pieceId}'");
            }
        }
        else
        {
            Debug.LogError($"SelectPieceExecuter: GameObject with name '{pieceId}' not found");
        }
    }
}
