using UnityEngine;

public class SelectPieceExecuter : Executer
{
       public override void execute(GameController gameController, Command command)
    {
        //カメラの向きを変える
        Camera.main.transform.position = new Vector3(0, 0, 0);
        //commandのpieceIdのピースのオブジェクトを取得する
        PieceId pieceId = ((SelectPieceByUserCommand)command).pieceId;
        GameObject piece = GameObject.Find(pieceId.ToString());
        piece.GetComponent<Piece3D>().Select();
    }
}
