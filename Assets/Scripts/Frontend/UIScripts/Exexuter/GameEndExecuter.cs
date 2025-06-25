using UnityEngine;
using TMPro;

public class GameEndExecuter : Executer
{

    public GameObject GameEndCanvas;
    public TextMeshProUGUI WinnerName;
    public GameObject UICanvas;

    public override void execute(GameController gameController, Result result)
    {
        Debug.Log("GameEndExecuter");


        GameEndCanvas.SetActive(true);
        WinnerName.text = ((PutPieceResult)result).winner.ToString()+"Win!!!";
        UICanvas.SetActive(false);
        
        // PutPieceExecuterと同様に最後に置いた駒の位置移動処理を実行
        PutPieceResult putPieceResult = (PutPieceResult)result;
        GameObject piece = GameObject.Find(putPieceResult.pieceId.ToString());
        
        // 計算過程をデバッグ
        int calculatedIndex = (int)(putPieceResult.position.Y * 4 + putPieceResult.position.X + 1);
        string pieceCircleName = "pieceCircle" + calculatedIndex + "-black";
        
        GameObject pieceCircle = GameObject.Find(pieceCircleName);
        
        if (pieceCircle == null)
        {
            Debug.LogError("pieceCircleが見つかりません: " + pieceCircleName);
        }
        else
        {
            Debug.Log("見つかったpieceCircle位置: (" + pieceCircle.transform.position.x + ", " + pieceCircle.transform.position.y + ", " + pieceCircle.transform.position.z + ")");
            
            // 駒のSetPosition呼び出し
            piece.GetComponent<Piece3D>().SetPosition(putPieceResult.position);
            
            // Y位置の調整（PutPieceExecuterと同じロジック）
            float yPosition = 0f;
            if (piece.transform.localScale.y == 0.5f || putPieceResult.pieceId.ToString() == "FSSW" || putPieceResult.pieceId.ToString() == "FSSB")
            {
                yPosition = -0.5f;
            }
            
            // 駒を正しい位置に移動
            Vector3 targetPosition = new Vector3(pieceCircle.transform.position.x, yPosition, pieceCircle.transform.position.z);
            piece.transform.SetPositionAndRotation(targetPosition, piece.transform.rotation);
            
            Debug.Log($"最後の駒 {putPieceResult.pieceId} を位置 ({putPieceResult.position.X}, {putPieceResult.position.Y}) に移動しました");
        }
        
       

        // 少し待ってからスタート画面に戻る処理は削除（ユーザーが手動で戻るため）
        // GameEndCanvas.SetActive(false);
        // GameStartCanvas.SetActive(true);
    }
}
