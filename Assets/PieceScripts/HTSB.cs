using UnityEngine;

public class HTSB : Piece3D
{
    protected override void Awake()
    {
        // piece3dIdフィールドに値を設定
        SetPieceId(PieceId.HTSB);
        
        // 親クラスのAwakeメソッドを呼び出す
        base.Awake();
    }
    
}
