using UnityEngine;

public class Player{
    private SelectPieceAlgorithm selectPieceAlgorithm;
    private PutPieceAlgorithm putPieceAlgorithm;
    public string SelectPieceAlgorithmName { get; private set; }
    public string PutPieceAlgorithmName { get; private set; }
    
    public Player(SelectPieceAlgorithm selectPieceAlgorithm, PutPieceAlgorithm putPieceAlgorithm, string selectPieceAlgorithmName = "", string putPieceAlgorithmName = ""){
        this.selectPieceAlgorithm = selectPieceAlgorithm;
        this.putPieceAlgorithm = putPieceAlgorithm;
        this.SelectPieceAlgorithmName = selectPieceAlgorithmName;
        this.PutPieceAlgorithmName = putPieceAlgorithmName;
    }
    public PieceId selectPiece(Board board){
        return selectPieceAlgorithm.SelectPiece(board.getState());
    }
    public Position putPiece(Board board){
        Debug.Log("putPieceaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"+putPieceAlgorithm.ToString());
        return putPieceAlgorithm.putPiece(board);
    }


}