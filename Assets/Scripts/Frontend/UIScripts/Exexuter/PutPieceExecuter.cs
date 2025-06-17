using UnityEngine;

public class PutPieceExecuter : Executer
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void execute(GameController gameController, Result result)
    {
        PutPieceResult putPieceResult = (PutPieceResult)result;
        GameObject piece = GameObject.Find(putPieceResult.pieceId.ToString());
        GameObject pieceCircle = GameObject.Find("pieceCircle" + ((4 - putPieceResult.position.X) * 4 + putPieceResult.position.Y) + "-black"); //これ変
        
        
        
        piece.GetComponent<Piece3D>().SetPosition(putPieceResult.position);
        
        float yPosition = 0f;
        if (piece.transform.localScale.y == 0.5f || putPieceResult.pieceId.ToString() == "FSSW" || putPieceResult.pieceId.ToString() == "FSSB")
        {
            yPosition = -0.5f;
        }
        piece.transform.SetPositionAndRotation(new Vector3(pieceCircle.transform.position.x, yPosition, pieceCircle.transform.position.z), piece.transform.rotation);
        
        
    }
}
