using System;
//using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Xml.Schema;

public class ConcreteSelectPieceAlgorithm : SelectPieceAlgorithm//�K���Ɏ���
{
    public override PieceId SelectPiece(Piece[] state)
    {

        return 0;//board.getSelectablePieces()[0];
    }
}