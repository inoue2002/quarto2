using UnityEngine;

public class ConcretePutPieceAlgorithm : PutPieceAlgorithm//適当に実装
{
    public override Position putPiece(Piece[] state, Piece piece)
    {
        for(int i = 0; i < state.Length; i++)
        {
            if(state[i] == null)
            {
                return new Position(i%4, i/4);
            }
        }
        return new Position(-1, -1);
    }
}