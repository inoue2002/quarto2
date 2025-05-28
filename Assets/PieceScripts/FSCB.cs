using UnityEngine;

public class FSCB : Piece3D
{
    // Awakeメソッドをオーバーライドして親クラスのpiece3dIdフィールドを設定
    protected override void Awake()
    {
        // piece3dIdフィールドに値を設定
        SetPieceId(PieceId.FSCB);
        
        // 親クラスのAwakeメソッドを呼び出す
        base.Awake();
    }
}
