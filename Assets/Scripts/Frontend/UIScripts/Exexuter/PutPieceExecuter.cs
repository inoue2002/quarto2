using UnityEngine;

public class PutPieceExecuter : Executer
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void execute(GameController gameController, Result result)
    {
        PutPieceResult putPieceResult = (PutPieceResult)result;
        GameObject piece = GameObject.Find(putPieceResult.pieceId.ToString());
        
        // 計算過程をデバッグ
        int calculatedIndex = (int)(putPieceResult.position.Y * 4 + putPieceResult.position.X + 1);
        string pieceCircleName = "pieceCircle" + calculatedIndex + "-black";
        

        
        GameObject pieceCircle = GameObject.Find(pieceCircleName);
        
        if (pieceCircle == null)
        {
            Debug.LogError("pieceCircleが見つかりません: " + pieceCircleName);
            return;
        }
        
        Debug.Log("見つかったpieceCircle位置: (" + pieceCircle.transform.position.x + ", " + pieceCircle.transform.position.y + ", " + pieceCircle.transform.position.z + ")");
        
        piece.GetComponent<Piece3D>().SetPosition(putPieceResult.position);
        
        float yPosition = 0f;
        if (piece.transform.localScale.y == 0.5f || putPieceResult.pieceId.ToString() == "FSSW" || putPieceResult.pieceId.ToString() == "FSSB")
        {
            yPosition = -0.5f;
        }
        
        Vector3 targetPosition = new Vector3(pieceCircle.transform.position.x, yPosition, pieceCircle.transform.position.z);
        piece.transform.SetPositionAndRotation(targetPosition, piece.transform.rotation);
        

        
        
    }
}
