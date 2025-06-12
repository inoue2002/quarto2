using System.Collections.Generic;
public class SelectPieceInformation : Information//選択できる駒を表示するための情報
{
    public List<PieceId> pieces;
    public SelectPieceInformation(List<PieceId> pieces)
    {
        this.pieces = pieces;
    }

}