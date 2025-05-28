public class Player{
    private SelectPieceAlgorithm selectPieceAlgorithm;
    private PutPieceAlgorithm putPieceAlgorithm;
    public Player(SelectPieceAlgorithm selectPieceAlgorithm, PutPieceAlgorithm putPieceAlgorithm){
        this.selectPieceAlgorithm = selectPieceAlgorithm;
        this.putPieceAlgorithm = putPieceAlgorithm;
    }
    public PieceId selectedPiece(Piece[] state){
        return 0;
    }
    public Position putPiece(Piece[] state, Piece piece){
        throw new System.NotImplementedException();
    }


}