using UnityEngine;

public class HTCB : Piece3D
{
    protected override void Awake()
    {
        // piece3dIdフィールドに値を設定
        SetPieceId(PieceId.HTCB);
        
        // 親クラスのAwakeメソッドを呼び出す
        base.Awake();
    }
}
