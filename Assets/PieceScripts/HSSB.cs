using UnityEngine;

public class HSSB : Piece3D
{
    // Awakeメソッドをオーバーライドして親クラスのpiece3dIdフィールドを設定
    protected override void Awake()
    {
        // piece3dIdフィールドに値を設定
        SetPieceId(PieceId.HSSB);
        
        // 親クラスのAwakeメソッドを呼び出す
        base.Awake();
    }
}
