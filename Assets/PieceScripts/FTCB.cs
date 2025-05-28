using UnityEngine;

public class FTCB : Piece3D
{
    // Awakeメソッドをオーバーライドして親クラスのpiece3dIdフィールドを設定
    protected override void Awake()
    {
        // piece3dIdフィールドに値を設定
        SetPieceId(PieceId.FTCB);
        
        // 親クラスのAwakeメソッドを呼び出す
        base.Awake();
    }
}
