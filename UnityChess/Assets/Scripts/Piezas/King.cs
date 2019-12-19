using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    private bool isMoved = false;

    public King(PieceBehaviour behaviour, int pieceValue, SquareTableValues values) : base(behaviour, pieceValue, values)
    {
    }

    public King()
    {

    }

    

    public override void Move(SquareBehaviour destiny)
    {
        base.Move(destiny);
        isMoved = true; 
    }

    public override Piece Copy(Square copySquare)
    {
        King king = new King();
        copySquare.SetNewPiece(king);
        king.pieceValue = this.pieceValue;
        king.values = this.values;
        king.isMoved = this.isMoved;
        king.possibleMoves = new List<Move>(); 
        return king;
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

        int i = 1;

        if (rightUp) EvaluateBoardInPosition(board, actualX + i, actualY + i, out rightUp);
        if (rightDown) EvaluateBoardInPosition(board, actualX + i, actualY - i, out rightDown);
        if (leftDown) EvaluateBoardInPosition(board, actualX - i, actualY - i, out leftDown);
        if (leftUp) EvaluateBoardInPosition(board, actualX - i, actualY + i, out leftUp);

        if (right) EvaluateBoardInPosition(board, actualX + i, actualY, out right);
        if (left) EvaluateBoardInPosition(board, actualX - i, actualY, out left);
        if (up) EvaluateBoardInPosition(board, actualX, actualY + i, out up);
        if (down) EvaluateBoardInPosition(board, actualX, actualY - i, out down);




    }


    public void CastlingRound(Square[,] b, Player otherPlayer)
    {
        bool canDoCastling =true;
        Square[,] board = null; 
        if (b == null)
        {
            board = Board.Instance.SquareMatrix; 
        }
        else
        {
            board = b; 
        }



        if (!isMoved && otherPlayer != null && !otherPlayer.CheckIfSquareIsInMoves(this.actualSquare)) //Si no es jaque ahora
        {
            for (int i = actualSquare.X + 1; i < 7; i++)
            {
                if (board[actualSquare.Y, i].PieceContainer != null || otherPlayer.CheckIfSquareIsInMoves(board[actualSquare.Y, i]))
                {
                    canDoCastling = false;
                    break;
                }

            }


            if (!isMoved && canDoCastling && board[actualSquare.Y, 7].PieceContainer != null && board[actualSquare.Y, 7].PieceContainer is Rook)
            {
                Rook rook = board[actualSquare.Y, 7].PieceContainer as Rook;
                if (!rook.IsMoved)
                {
                    Move move = new Move(board[actualSquare.Y, actualSquare.X + 2]);
                    int y = ActualSquare.Y;
                    int x = ActualSquare.X + 1;
                    int previousX = rook.ActualSquare.X;
                    int previousY = rook.ActualSquare.Y;
                    move.RegisterCallback(() =>
                    {
                        if (b != null)
                        {
                            rook.Move(board[y, x]);
                        }
                        else
                        {
                            rook.Move(board[y, x].RelatedBehaviour);
                        }
                    });
                    move.RegisterCallbackToReset(() =>
                    {
                        rook.Move(board[previousY, previousX]);
                    });
                    possibleMoves.Add(move);
                }
            }


            canDoCastling = true; 

            for (int i = actualSquare.X - 1; i > 0 ; i--)
            {
                if (board[actualSquare.Y, i].PieceContainer != null || otherPlayer.CheckIfSquareIsInMoves(board[actualSquare.Y, i]))
                {
                    canDoCastling = false;
                    break;
                }

            }

            if (!isMoved && canDoCastling && board[actualSquare.Y, 0].PieceContainer != null && board[actualSquare.Y, 0].PieceContainer is Rook)
            {
                Rook rook = board[actualSquare.Y, 0].PieceContainer as Rook;
                if (!rook.IsMoved)
                {
                    Move move = new Move(board[actualSquare.Y, actualSquare.X - 2]);
                    int y = ActualSquare.Y;
                    int x = ActualSquare.X - 1;
                    int previousX = rook.ActualSquare.X;
                    int previousY = rook.ActualSquare.Y; 
                    move.RegisterCallback(() =>
                    {
                        if (b != null)
                        {
                            rook.Move(board[y, x]);
                        }
                        else
                        {
                            rook.Move(board[y, x].RelatedBehaviour);
                        }
                    });
                    move.RegisterCallbackToReset(() =>
                    {
                        rook.Move(board[previousY, previousX]); 
                    });


                    possibleMoves.Add(move);
                }
            }



        }


    }

}


