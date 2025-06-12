using UnityEngine;

public class FTCW : Piece3D
{
    protected override void Awake()
    {
        // piece3dIdフィールドに値を設定
        SetPieceId(PieceId.FTCW);
        
        // 親クラスのAwakeメソッドを呼び出す
        base.Awake();
    }

}
