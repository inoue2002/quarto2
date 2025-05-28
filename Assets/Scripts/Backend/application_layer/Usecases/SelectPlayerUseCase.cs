public class SelectPlayerUseCase {
/// <summary>
/// ワンチャンなくてもいいかも
/// </summary>
/// <param name="playerInfo"></param>
/// <returns></returns>
    public static Result handle(PlayerInfo playerInfo)
    {
        return new SelectPlayerResult(playerInfo);
    }
    /// <summary>
    /// なんか初期化する（多分いらん）
    /// </summary>
    private void initialize(){

    }
}