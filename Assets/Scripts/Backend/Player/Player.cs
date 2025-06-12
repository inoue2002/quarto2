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
        return putPieceAlgorithm.putPiece(board);
    }


}