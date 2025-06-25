using UnityEngine;

public class Player{
    private SelectPieceAlgorithm selectPieceAlgorithm;
    private PutPieceAlgorithm putPieceAlgorithm;
    public Player(SelectPieceAlgorithm selectPieceAlgorithm, PutPieceAlgorithm putPieceAlgorithm){
        this.selectPieceAlgorithm = selectPieceAlgorithm;
        this.putPieceAlgorithm = putPieceAlgorithm;
    }
    public PieceId selectPiece(Board board){
        return selectPieceAlgorithm.SelectPiece(board.getState());
    }
    public Position putPiece(Board board){
        Debug.Log("putPieceaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"+putPieceAlgorithm.ToString());
        return putPieceAlgorithm.putPiece(board);
    }


}