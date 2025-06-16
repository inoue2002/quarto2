using UnityEngine;

public class BoardCell : MonoBehaviour
{
    [SerializeField]
    protected int x; // X座標（1-4）
    
    [SerializeField]
    protected int y; // Y座標（1-4）
    
    // セルの位置を取得
    public Position GetPosition()
    {
        return new Position(x, y);
    }
    
    private void OnMouseDown()
    {
        // Notify the ViewController that this cell has been clicked

        
        GameObject viewControllerObject = GameObject.Find("ViewController");
        if (viewControllerObject == null)
        {
            return;
        }

        ViewController viewController = viewControllerObject.GetComponent<ViewController>();
        if (viewController == null)
        {
            return;
        }

        // 現在のフェーズがPutPieceByUserの時のみクリックを許可
        if (viewController.gameController.currentPhase.type != GamePhaseType.PutPieceByUser)
        {
            return;
        }


        PutPieceByUserCommand putPieceByUserCommand = new PutPieceByUserCommand();  //ここでコマンドを作成
        putPieceByUserCommand.position = this.GetPosition();    //ここでコマンドに位置を設定
        viewController.execute(putPieceByUserCommand);  //ここでコマンドを実行
        //this.gameObject.transform.SetLocalPositionAndRotation(new Vector3(0.0f, this.gameObject.transform.position.y, 0.0f), this.gameObject.transform.rotation);   //ここでオブジェクトを元の位置に戻す
    }
}