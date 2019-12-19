using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece {
    public Knight(PieceBehaviour behaviour, int pieceValue, SquareTableValues values) : base(behaviour, pieceValue, values)
    {
    }

    public Knight()
    {

    }


    public override Piece Copy(Square copySquare)
    {
        Knight knight = new Knight();
        copySquare.SetNewPiece(knight); 
        knight.pieceValue = this.pieceValue;
        knight.values = this.values;
        knight.possibleMoves = new List<Move>(); 
        return knight;
    }

    private void EvaluatePosition (Square[,] board, int x, int y)
    {
        if (x >= 8) return;
        if (x < 0) return;
        if (y >= 8) return;
        if (y < 0) return; 

        if (board[y,x].PieceContainer == null || !board[y, x].PieceContainer.Propietary.Equals(Propietary))
        {
            possibleMoves.Add(new Move(board[y, x]));
        }

    }

    public override void Evaluate(Square[,] b = null)
    {
        Square[,] board = null;
        if (b== null)
        {
            board = Board.Instance.SquareMatrix;
        }
        else
        {
            board = b; 
        }
        possibleMoves.Clear(); 

        int x = actualSquare.X;
        int y = actualSquare.Y;

        EvaluatePosition(board, x + 2, y + 1);
        EvaluatePosition(board, x + 2, y - 1); 
        EvaluatePosition(board, x - 2, y + 1); 
        EvaluatePosition(board, x - 2, y - 1); 
        EvaluatePosition(board, x + 1, y + 2); 
        EvaluatePosition(board, x - 1, y + 2); 
        EvaluatePosition(board, x - 1, y - 2); 
        EvaluatePosition(board, x + 1, y - 2); 


    }

}
