using UnityEngine;

public class PutPieceExecuter : Executer
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void execute(GameController gameController, Result result)
    {
        PutPieceResult putPieceResult = (PutPieceResult)result;
        GameObject piece = GameObject.Find(putPieceResult.pieceId.ToString());
        // 0-basedの座標系に合わせて計算を修正
        GameObject pieceCircle = GameObject.Find("pieceCircle" + (putPieceResult.position.Y * 4 + putPieceResult.position.X) + "-black");
        
        Debug.Log($"選択されたピース: ID={putPieceResult.pieceId}, 現在位置=({piece.transform.position.x}, {piece.transform.position.y}, {piece.transform.position.z})");
        Debug.Log($"配置先マス: 位置=({putPieceResult.position.X}, {putPieceResult.position.Y}), マス位置=({pieceCircle.transform.position.x}, {pieceCircle.transform.position.y}, {pieceCircle.transform.position.z})");
        
        piece.GetComponent<Piece3D>().SetPosition(putPieceResult.position);
        
        float yPosition = 0f;
        if (piece.transform.localScale.y == 0.5f || putPieceResult.pieceId.ToString() == "FSSW" || putPieceResult.pieceId.ToString() == "FSSB")
        {
            yPosition = -0.5f;
        }
        piece.transform.SetPositionAndRotation(new Vector3(pieceCircle.transform.position.x, yPosition, pieceCircle.transform.position.z), piece.transform.rotation);
        
        Debug.Log($"ピース配置完了: 新しい位置=({piece.transform.position.x}, {piece.transform.position.y}, {piece.transform.position.z}), PieceID={putPieceResult.pieceId}");
    }
}
