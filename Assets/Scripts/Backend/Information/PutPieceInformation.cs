public class PutPieceInformation : Information
{
    public PlayerId currentPlayerId;
    public PlayerType currentPlayerType;
    public string algorithmName;
    public PutPieceInformation(PlayerId currentPlayerId, PlayerType currentPlayerType, string algorithmName = "")
    {
        this.currentPlayerId = currentPlayerId;
        this.currentPlayerType = currentPlayerType;
        this.algorithmName = algorithmName;
    }
}