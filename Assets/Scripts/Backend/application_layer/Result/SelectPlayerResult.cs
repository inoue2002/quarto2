public class SelectPlayerResult : Result
{
    public PlayerInfo playerInfo;
    public SelectPlayerResult(PlayerInfo playerInfo)
    {
        this.playerInfo = playerInfo;
    }
}