using UnityEngine;

public class HSSW : Piece3D
{
    protected override void Awake()
    {
        // piece3dIdフィールドに値を設定
        SetPieceId(PieceId.HSSW);
        
        // 親クラスのAwakeメソッドを呼び出す
        base.Awake();
    }
}
