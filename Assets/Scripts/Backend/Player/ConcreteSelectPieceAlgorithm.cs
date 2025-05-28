public class ConcreteSelectPieceAlgorithm : SelectPieceAlgorithm//適当に実装
{
    public override PieceId SelectPiece(Piece[] state)
    {
        return state[0].getPieceId();
    }
}