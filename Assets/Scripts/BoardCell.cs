using UnityEngine;

public class BoardCell : MonoBehaviour
{
    [SerializeField]
    private int x; // X座標（0-3）
    
    [SerializeField]
    private int y; // Y座標（0-3）
    
    // セルの位置を取得
    public Position GetPosition()
    {
        return new Position(x, y);
    }
    
    private void OnMouseDown()
    {
        // Notify the ViewController that this cell has been clicked
        Debug.Log($"BoardCell.OnMouseDown() called at position ({x}, {y})");
        //ViewController.Instance.OnBoardCellClicked(transform);
    }
}