using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : Piece
{
    private bool isMoved = false;

   

    public Rook(PieceBehaviour behaviour, int pieceValue, SquareTableValues values) : base(behaviour, pieceValue, values)
    {
    }
    public Rook()
    {

    }

    public override void Move(SquareBehaviour destiny)
    {
        base.Move(destiny);
        isMoved = true; 
    }
   
    public override Piece Copy(Square copySquare)
    {
        Rook rook = new Rook();
        copySquare.SetNewPiece(rook);
        rook.pieceValue = this.pieceValue;
        rook.values = this.values;
        rook.isMoved = this.isMoved;
        rook.possibleMoves = new List<Move>(); 
        return rook;
    }


    private void EvaluateBoardInPosition(Square[,] board, int x, int y, out bool flag)
    {
        flag = true;
        if (x >= 8)
            return;
        if (x < 0)
            return;
        if (y >= 8)
            return;
        if (y < 0)
            return;
        if (board[y, x].PieceContainer == null)
        {
            possibleMoves.Add(new Move(board[y, x]));
        }
        else
        {
            flag = false;
            if (!board[y, x].PieceContainer.Propietary.Equals(this.Propietary))
            {
                possibleMoves.Add(new Move(board[y, x]));
            }
        }

    }


    public override void Evaluate(Square[,] b)
    {
        Square[,] board = null;
        if (b == null)
        {
            board = Board.Instance.SquareMatrix;
        }
        else
        {
            board = b;
        }
        possibleMoves.Clear(); 
        int actualX = this.actualSquare.X;
        int actualY = this.actualSquare.Y;
        bool right = true, left = true, up = true, down = true;
        for (int i = 1; i < 8; i++)
        {

            if (right) EvaluateBoardInPosition(board, actualX + i, actualY, out right);
            if (left) EvaluateBoardInPosition(board, actualX - i, actualY, out left);
            if (up) EvaluateBoardInPosition(board, actualX, actualY + i, out up);
            if (down) EvaluateBoardInPosition(board, actualX, actualY - i, out down); 

        }

    }





    public bool IsMoved
    {
        get
        {
            return isMoved;
        }
    }

}
