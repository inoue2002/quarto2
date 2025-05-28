public struct PlayerInfo
{
    public PlayerType SelectPiece;
    public PlayerType PutPiece;

    public PlayerInfo(PlayerType select,PlayerType put)
    {
        SelectPiece = select;
        PutPiece = put;
    }

        public override string ToString()
    {
        return $"({SelectPiece}, {PutPiece})";
    }
}
public enum ActionType
{
    SelectPiece,
    PutPiece,
}