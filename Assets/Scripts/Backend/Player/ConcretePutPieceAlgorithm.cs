

public class ConcretePutPieceAlgorithm : PutPieceAlgorithm//�K���Ɏ���
{
    public override Position putPiece(Board board)
    {
        
        for (int i = 0; i < board.getstate().Length; i++)
        {
            if (board.getstate()[i] == null)
            {
                return new Position(i % 4, i / 4);
                
            }
        }
        return new Position(-1, -1);
    }
}