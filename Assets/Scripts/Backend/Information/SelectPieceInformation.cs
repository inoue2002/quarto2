using System.Collections.Generic;
public class SelectPieceInformation : Information//選択できる駒を表示するための情報
{
    public List<PieceId> pieces;
    
    public PlayerId currentPlayerId;
    public PlayerType currentPlayerType;
    public SelectPieceInformation(List<PieceId> pieces, PlayerId currentPlayerId, PlayerType currentPlayerType)
    {
        this.pieces = pieces;
        this.currentPlayerId = currentPlayerId;
        this.currentPlayerType = currentPlayerType;
    }

}