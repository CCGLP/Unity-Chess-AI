using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Piece {

    public Queen(PieceBehaviour behaviour, int pieceValue, SquareTableValues values) : base(behaviour, pieceValue, values)
    {
    }

    public Queen()
    {

    }

    public Queen(int pieceValue, SquareTableValues values, Square square, Player propietary)
    {
        square.SetNewPiece(this);
        this.pieceValue = pieceValue;
        this.values = values;
        this.SetPropietary(propietary); 
        this.possibleMoves = new List<Move>();
    }

    public override Piece Copy(Square copySquare)
    {
        Queen queen = new Queen();
        copySquare.SetNewPiece(queen);
        queen.pieceValue = this.pieceValue;
        queen.values = this.values;
        queen.possibleMoves = new List<Move>(); 
        return queen;
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
        bool rightUp = true, rightDown = true, leftUp = true, leftDown = true;

        for (int i = 1; i < 8; i++)
        {

            if (rightUp) EvaluateBoardInPosition(board, actualX + i, actualY + i, out rightUp);
            if (rightDown) EvaluateBoardInPosition(board, actualX + i, actualY - i, out rightDown);
            if (leftDown) EvaluateBoardInPosition(board, actualX - i, actualY - i, out leftDown);
            if (leftUp) EvaluateBoardInPosition(board, actualX - i, actualY + i, out leftUp);

            if (right) EvaluateBoardInPosition(board, actualX + i, actualY, out right);
            if (left) EvaluateBoardInPosition(board, actualX - i, actualY, out left);
            if (up) EvaluateBoardInPosition(board, actualX, actualY + i, out up);
            if (down) EvaluateBoardInPosition(board, actualX, actualY - i, out down);


        }

    }



}
