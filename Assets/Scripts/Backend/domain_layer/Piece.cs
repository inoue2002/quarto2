

public class Piece
{
    private PieceValue[] values; //いらない
    private PieceId pieceId;
    public Piece(PieceId pieceId)
    {
        this.pieceId = pieceId;
        
    }
    public PieceId getPieceId()
    {
        return pieceId;
    }

    public int calculateValue()//もう二度とつかわないゴミ
    {
        return 0;
    }

    public bool isQuarto(Piece[] othersPieces)//4つの特徴が全て同じかどうか
    {
        int idAnd=(int)this.pieceId;
        int notAnd=~(int)this.pieceId & 0b1111;
        foreach(Piece anotherpiece in othersPieces)
        {
            idAnd&=(int)anotherpiece.getPieceId();
            notAnd&=~(int)anotherpiece.getPieceId() & 0b1111;
        }
        if(idAnd!=0 || notAnd!=0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}