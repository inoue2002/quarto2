using System;
//using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Xml.Schema;
using System.Collections.Generic;

public class ConcreteSelectPieceAlgorithm : SelectPieceAlgorithm//�K���Ɏ���
{
    public override PieceId SelectPiece(Piece[] state)
    {
        List<int> choices = new List<int>(){0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15};
        for(int i = 0; i < state.Length; i++){
            if(state[i] != null){
                choices.Remove((int) state[i].getPieceId());
            }
        }
        return(PieceId) choices[0];
    }
}