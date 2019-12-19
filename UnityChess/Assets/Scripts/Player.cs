using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

public class Player  {

    public enum PlayerType
    {
        AI, 
        HUMAN
    }

    private static int playerID = 0;
    private int iD; 

    private PlayerType type;
    private List<Piece> pieces;
    private Material asignedMaterial; 
    private King king;
    private int modSquareTableValue = 0;
    private int boardValue = 0; 

    public Player(Material material, PlayerType type = PlayerType.HUMAN, int modSquareValue = 0)
    {
        iD = Player.playerID;
        Player.playerID++;
        this.type = type;
        this.asignedMaterial = material;
        this.modSquareTableValue = modSquareValue; 
        pieces = new List<Piece>(); 
    }

    public Player (Player other, Square[,] newBoard)
    {
        iD = other.iD;
        this.modSquareTableValue = other.modSquareTableValue; 
        this.type = other.type;
        pieces = new List<Piece>(); 
        for (int i = 0; i< other.pieces.Count; i++)
        {
            var newPiece = other.pieces[i].Copy(newBoard[other.pieces[i].ActualSquare.Y, other.pieces[i].ActualSquare.X]);
            newPiece.SetPropietary(this); 
        }
    }

    public void Evaluate(Square[,] board = null)
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            if (pieces[i] is King)
            {
               king = (pieces[i] as King);
            }
            pieces[i].Evaluate(board);
        }

    }

    public int EvaluateBoardValue()
    {
        boardValue = 0; 
        for (int i = 0; i < pieces.Count; i++)
        {
            boardValue += pieces[i].EvaluateBoardScore(); 
        }
        return boardValue; 
    }

    public void EvaluateCastlings(Square[,] board = null, Player otherPlayer= null)
    {
        king.CastlingRound(board, otherPlayer);
    }

    public void AddPiece(Piece piece)
    {
        if (piece != null)
        {
            pieces.Add(piece);
        }
        
    }

    public void AddPieceInNumber(Piece piece ,int number)
    {
        pieces.Insert(number, piece); 
    }

    public int ReturnNumberPiece(Piece piece)
    {
        return pieces.FindIndex((Piece p) => { return p == piece; }); 
    }

    public void DestroyPiece(Piece piece)
    {

        pieces.Remove(piece); 
       
    }

    public override bool Equals(object other)
    {
        Player otherPlayer = other as Player; 
        return otherPlayer != null && otherPlayer.iD == this.iD;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public void EvaluateCheckOffMoves(Player otherPlayer, Square[,] b = null)
    {
        
        var kingPosition = (from piece in this.pieces where piece is King select piece).ToArray()[0].ActualSquare;
        Square[,] actualBoard = null; 
        if (b == null)
        {
            actualBoard = Board.Instance.GenerateBoardCopy();
        }
        else
        {
            actualBoard = Board.Instance.GenerateBoardCopy(b); 
        }

        //Genero los movimientos de las copias
        Player copyMe = new Player(this, actualBoard);
        Player copyOther = new Player(otherPlayer, actualBoard);
        copyMe.Evaluate(actualBoard);
        copyOther.Evaluate(actualBoard);
        copyMe.EvaluateCastlings(actualBoard, copyOther);
        copyOther.EvaluateCastlings(actualBoard, copyMe);
        List<Square> movesToErase = new List<Square>();

        for (int i = 0; i< pieces.Count; i++)
        {
            var moves = pieces[i].PossibleMoves;
            var piece = copyMe.pieces[i]; 
            movesToErase.Clear(); 

           

            for (int j = 0; j < moves.Count; j++)
            {
                //Efectuo el movimiento y genero de nuevo los movimientos del otro jugador para ver si mi movimiento es válido
                var previousPiece = piece.PossibleMoves[j].Square.PieceContainer;
                var previousSquare = piece.ActualSquare; 
              
                piece.Move(actualBoard[moves[j].Square.Y, moves[j].Square.X]);
                piece.PossibleMoves[j].RunCallback();
                copyOther.Evaluate(actualBoard);



                if (piece is King) //Compruebo si mi movimiento es válido. SI mi pieza es el rey, compruebo si están en esta pieza, y si no compruebo la posición del rey que he guardado anteriormente
                {
                    if (copyOther.CheckIfSquareIsInMoves(piece.ActualSquare))
                    {
                        movesToErase.Add(moves[j].Square);
                    }
                }
                else
                {
                    if (copyOther.CheckIfSquareIsInMoves(kingPosition))
                    {
                        movesToErase.Add(moves[j].Square);
                    }
                }

                piece.Move(previousSquare);
                copyOther.AddPiece(previousPiece);
                if (previousPiece != null)
                    previousPiece.Move(actualBoard[moves[j].Square.Y, moves[j].Square.X]); 
                piece.PossibleMoves[j].RunCallbackReset(); 
            
            }

            for (int j= 0; j< movesToErase.Count; j++)
            {
                this.pieces[i].RemoveMove(movesToErase[j]); 
            }

            
        }
    }


    public bool CheckIfCheckMate()
    {
        for (int i =0; i<pieces.Count; i++)
        {
            for (int j = 0; j< pieces[i].PossibleMoves.Count; j++)
            {
                return false; 
            }
        }

        return true; 
    }

    public void DestroyLastPiece()
    {
        pieces[pieces.Count - 1].Destroy(); 
    }

    public bool CheckIfSquareIsInMoves(Square square)
    {
        for (int i = 0; i< this.pieces.Count; i++)
        {
            for (int j = 0; j< this.pieces[i].PossibleMoves.Count; j++)
            {
                if (this.pieces[i].PossibleMoves[j].IsHarmMove && this.pieces[i].PossibleMoves[j].Square.Equals(square))
                {
                  
                    return true; 
                }
            }
        }
        return false; 
    }

  
    public void ResetPawnsState()
    {
        for (int i = 0; i< pieces.Count; i++)
        {
            Pawn pawn = pieces[i] as Pawn; 
            if (pawn != null)
            {
                pawn.ResetJustMovedTwo(); 
            }
        }
    }

    public PlayerType Type
    {
        get
        {
            return type;
        }
    }

    public Material AsignedMaterial
    {
        get
        {
            return asignedMaterial;
        }

    }

    public int ID
    {
        get
        {
            return iD;
        }

    }

    public int ModSquareTableValue
    {
        get
        {
            return modSquareTableValue;
        }

      
    }

    public List<Piece> Pieces
    {
        get
        {
            return pieces;
        }

       
    }
}
