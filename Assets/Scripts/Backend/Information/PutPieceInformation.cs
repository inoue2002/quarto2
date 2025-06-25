public class PutPieceInformation : Information
{
    public PlayerId currentPlayerId;
    public PlayerType currentPlayerType;
    public PutPieceInformation(PlayerId currentPlayerId, PlayerType currentPlayerType)
    {
        this.currentPlayerId = currentPlayerId;
        this.currentPlayerType = currentPlayerType;
    }
}