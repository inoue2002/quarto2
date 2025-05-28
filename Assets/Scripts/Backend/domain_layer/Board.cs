using System.Collections.Generic;
using System.ComponentModel;
using System;
public class Board
{
    private Piece selectedPiece;
    private Piece[] state={null}; //16この配列
    private PlayerId currentPlayer;
    private Dictionary<PieceId, Piece> pieces;
    private List<int[]> lines=new List<int[]>();

    public Board()
    {
        currentPlayer = PlayerId.Player1;
        state = new Piece[16];
        pieces = new Dictionary<PieceId, Piece>();
        foreach(PieceId pieceId in Enum.GetValues(typeof(PieceId)))
        {
            pieces.Add(pieceId, new Piece(pieceId));
        }
        for(int i = 0; i < 4; i++)
        {
            lines.Add(new int[]{i*4,i*4+1,i*4+2,i*4+3});
            lines.Add(new int[]{i,i+4,i+8,i+12});
        }
        lines.Add(new int[]{0,5,10,15});
        lines.Add(new int[]{3,6,9,12});
        
    }

    public void setSelectedPiece(PieceId pieceId)
    {
        selectedPiece = pieces[pieceId];
    }

    public void putPiece(PieceId pieceId, Position position)
    {
        state[(int)(position.Y * 4 + position.X)] = pieces[pieceId];
        selectedPiece = null;
    }
    
    
    public void changePlayer(PlayerId player)
    {
        if(player == PlayerId.Player1)
        {
            currentPlayer = PlayerId.Player2;
        }
        else
        {
            currentPlayer = PlayerId.Player1;
        }
    }

    public PlayerId judge()
    {
        foreach(int[] line in lines)
        {
            if(state[line[0]] != null && state[line[1]]!=null && state[line[2]]!=null && state[line[3]]!=null)
            {
                if(state[line[0]].isQuarto(new Piece[]{state[line[1]],state[line[2]],state[line[3]]}))
                {
                    return currentPlayer;
                }
            }
        }
        return PlayerId.None;
    }
    public PlayerId getPlayerId()
    {
        return currentPlayer;
    }
    public bool canPutPiece(Position position)
    {
        return state[(int)(position.Y * 4 + position.X)] == null;
    }
    public PieceId canSelectPiece(PieceId pieceId)
    {
        return 0;
    }
    public Piece[] getState()
    {
        Piece[] state = new Piece[16];
        for(int i = 0; i < 16; i++)
        {
            state[i] = this.state[i];
        }
        return state;
    }
    public Piece getSelectedPiece()
    {
        return selectedPiece;
    }
    public PieceId getSelectedPieceId()
    {
        return selectedPiece.getPieceId();
    }
}