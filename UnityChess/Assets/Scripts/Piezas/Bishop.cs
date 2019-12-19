using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece {
    public Bishop(PieceBehaviour behaviour, int pieceValue, SquareTableValues values) : base(behaviour, pieceValue, values)
    {
    }

    public Bishop()
    {
    }

    public override Piece Copy(Square copySquare)
    {
        Bishop bishop = new Bishop();
        copySquare.SetNewPiece(bishop);
        bishop.pieceValue = this.pieceValue;
        bishop.values = this.values;
        bishop.possibleMoves = new List<Move>(); 
        return bishop;
    }


    private void EvaluateBoardInPosition(Square[,] board, bool checkRight, bool checkUp, int x, int y, out bool flag)
    {
        flag = true; 
        if (checkRight && x >= 8)
            return;
        if (!checkRight && x < 0)
            return;
        if (checkUp && y >= 8)
            return;
        if (!checkUp && y < 0)
            return; 
        if (board[y,x].PieceContainer == null)
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
        bool rightUp = true, rightDown = true, leftUp = true, leftDown = true; 
        for (int i = 1; i<8; i++)
        {
            
            if (rightUp) EvaluateBoardInPosition(board, true, true, actualX + i, actualY+i, out rightUp);
            if (rightDown) EvaluateBoardInPosition(board, true, false, actualX + i, actualY - i, out rightDown);
            if (leftDown) EvaluateBoardInPosition(board, false, false, actualX - i, actualY - i, out leftDown);
            if (leftUp) EvaluateBoardInPosition(board, false, true, actualX - i, actualY + i, out leftUp);

        }
    }


}
