public class PutPieceResult : Result
{
    public bool success;
    public PlayerId winner;
    public PieceId pieceId;
    public Position position;
    public PutPieceResult()
    {
        
    }
}