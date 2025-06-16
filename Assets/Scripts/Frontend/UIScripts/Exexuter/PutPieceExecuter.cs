using UnityEngine;

public class PutPieceExecuter : Executer
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void execute(GameController gameController, Result result)
    {
        PutPieceResult putPieceResult = (PutPieceResult)result;
        GameObject piece = GameObject.Find(putPieceResult.pieceId.ToString());
        GameObject pieceCircle = GameObject.Find("pieceCircle" + ((4 - putPieceResult.position.X) * 4 + putPieceResult.position.Y) + "-black"); //これ変
        // Debug.Log(piece.transform.position.x + " " + piece.transform.position.y + " " + piece.transform.position.z);
        // Debug.Log((4 - putPieceResult.position.X) * 4 + putPieceResult.position.Y);
        Debug.Log(pieceCircle.transform.position.x + " " + pieceCircle.transform.position.y + " " + pieceCircle.transform.position.z);
        piece.GetComponent<Piece3D>().SetPosition(putPieceResult.position);
        //Debug.Log(piece.transform.position);
        //Debug.Log(pieceCircle.transform.position.x + " " + pieceCircle.transform.position.y + " " + pieceCircle.transform.position.z);
        piece.transform.SetPositionAndRotation(new Vector3(pieceCircle.transform.position.x, piece.transform.position.y, pieceCircle.transform.position.z), piece.transform.rotation);
    }
}
