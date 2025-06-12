using UnityEngine;

public class HSCB : Piece3D
{
    protected override void Awake()
    {
        // piece3dIdフィールドに値を設定
        SetPieceId(PieceId.HSCB);
        
        // 親クラスのAwakeメソッドを呼び出す
        base.Awake();
    }
}
