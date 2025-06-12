using UnityEngine;

public class BoardCell : MonoBehaviour
{
    [SerializeField]
    protected int x; // X座標（0-3）
    
    [SerializeField]
    protected int y; // Y座標（0-3）
    
    // セルの位置を取得
    public Position GetPosition()
    {
        return new Position(x, y);
    }
    
    private void OnMouseDown()
    {
        // Notify the ViewController that this cell has been clicked
        Debug.Log($"BoardCell.OnMouseDown() called at position ({x}, {y})");
        
        GameObject viewControllerObject = GameObject.Find("ViewController");
        if (viewControllerObject == null)
        {
            Debug.LogError("ViewController not found!");
            return;
        }

        ViewController viewController = viewControllerObject.GetComponent<ViewController>();
        if (viewController == null)
        {
            Debug.LogError("ViewController component not found!");
            return;
        }

        // 現在のフェーズがPutPieceByUserの時のみクリックを許可
        if (viewController.gameController.currentPhase.type != GamePhaseType.PutPieceByUser)
        {
            Debug.LogWarning($"ボードのマスはピース選択後にクリックしてください。現在のフェーズ: {viewController.gameController.currentPhase.type}");
            return;
        }

        PutPieceByUserCommand putPieceByUserCommand = new PutPieceByUserCommand();
        putPieceByUserCommand.position = this.GetPosition();
        viewController.execute(putPieceByUserCommand);
        this.gameObject.transform.SetLocalPositionAndRotation(new Vector3(0.0f, this.gameObject.transform.position.y, 0.0f), this.gameObject.transform.rotation);
    }
}