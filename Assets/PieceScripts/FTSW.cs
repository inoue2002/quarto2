using UnityEngine;

public class FTSW : Piece3D
{
    protected override void Awake()
    {
        // piece3dIdフィールドに値を設定
        SetPieceId(PieceId.FTSW);
        
        // 親クラスのAwakeメソッドを呼び出す
        base.Awake();
    }
}
