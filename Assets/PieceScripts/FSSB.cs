using UnityEngine;

public class FSSB : Piece3D
{
    protected override void Awake()
    {
        // piece3dIdフィールドに値を設定
        SetPieceId(PieceId.FSSB);
        
        // 親クラスのAwakeメソッドを呼び出す
        base.Awake();
    }
}
