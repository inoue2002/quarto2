using UnityEngine;

public class HTCW : Piece3D
{
    protected override void Awake()
    {
        // piece3dIdフィールドに値を設定
        SetPieceId(PieceId.HTCW);
        
        // 親クラスのAwakeメソッドを呼び出す
        base.Awake();
    }
}