using System.Collections.Generic;
public class SelectPlayerInformation: Information
{
    public List<string> SelectPieceAlgorithmNames;
    public List<string> PutPieceAlgorithmNames;
    public SelectPlayerInformation(List<string> SelectPieceAlgorithmNames, List<string> PutPieceAlgorithmNames)
    {
        this.SelectPieceAlgorithmNames = SelectPieceAlgorithmNames;
        this.PutPieceAlgorithmNames = PutPieceAlgorithmNames;
    }
}