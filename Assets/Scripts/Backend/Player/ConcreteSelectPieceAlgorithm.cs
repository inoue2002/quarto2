using System;
//using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Xml.Schema;
using UnityEngine;

public class ConcreteSelectPieceAlgorithm : SelectPieceAlgorithm//順番に駒を選ぶアルゴリズム
{
    private static int currentPieceIndex = 0; // 静的カウンター（次に選ぶ駒のインデックス）
    
    public override PieceId SelectPiece(Piece[] state)
    {
        // 全ての駒ID（0〜15）を順番に試す
        PieceId[] allPieceIds = {
            PieceId.FSCB, PieceId.FSCW, PieceId.FSSB, PieceId.FSSW,
            PieceId.FTCB, PieceId.FTCW, PieceId.FTSB, PieceId.FTSW,
            PieceId.HSCB, PieceId.HSCW, PieceId.HSSB, PieceId.HSSW,
            PieceId.HTCB, PieceId.HTCW, PieceId.HTSB, PieceId.HTSW
        };
        
        // 現在のインデックスから順番に選択可能な駒を探す
        for (int i = 0; i < allPieceIds.Length; i++)
        {
            int index = (currentPieceIndex + i) % allPieceIds.Length;
            PieceId candidatePiece = allPieceIds[index];
            
            // この駒が選択可能かチェック（盤面に配置されていない駒）
            bool isAvailable = true;
            for (int j = 0; j < state.Length; j++)
            {
                if (state[j] != null && state[j].getPieceId() == candidatePiece)
                {
                    isAvailable = false; // 既に配置済み
                    break;
                }
            }
            
            if (isAvailable)
            {
                currentPieceIndex = (index + 1) % allPieceIds.Length; // 次回用にインデックスを更新
                Debug.Log("CPUが駒を選択: " + candidatePiece);
                return candidatePiece;
            }
        }
        
        // 選択可能な駒がない場合（通常は発生しない）
        Debug.LogError("選択可能な駒がありません");
        return 0;
    }
}