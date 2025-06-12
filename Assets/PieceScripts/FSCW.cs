using UnityEngine;

public class FSCW : Piece3D
{
    protected override void Awake()
    {
        // piece3dIdフィールドに値を設定
        SetPieceId(PieceId.FSCW);
        
        // 親クラスのAwakeメソッドを呼び出す
        base.Awake();
    }
}
