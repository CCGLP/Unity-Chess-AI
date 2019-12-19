using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece {
    private bool positionInitial = true;
    private bool justMovedTwo = false; 
    private int yDirection = -1;
    private bool evaluatedYet = false; 

    public Pawn(PieceBehaviour behaviour, int pieceValue, SquareTableValues values) : base(behaviour, pieceValue, values)
    {

    }
    public Pawn()
    {
        
    }

    public override Piece Copy(Square copySquare)
    {
        Pawn pawn = new Pawn();
        copySquare.SetNewPiece(pawn); 
        pawn.pieceValue = this.pieceValue;
        pawn.values = this.values;
        pawn.evaluatedYet = this.evaluatedYet;
        pawn.yDirection = this.yDirection;
        pawn.justMovedTwo = this.justMovedTwo;
        pawn.positionInitial = this.positionInitial;
        pawn.possibleMoves = new List<Move>(); 
        return pawn;
    }

    public void ResetJustMovedTwo()
    {
        justMovedTwo = false; 
    }

    private void EvaluateFrontMoves(Square[,] board)
    {
        if (actualSquare.Y + yDirection < 8 && actualSquare.Y + yDirection >= 0 && board[actualSquare.Y + yDirection, actualSquare.X].PieceContainer == null)
        {
            possibleMoves.Add(new Move(board[actualSquare.Y + yDirection, actualSquare.X], false));
            CheckPromotionOnLastMove();
            if (((yDirection == 1 && actualSquare.Y == 1) || (yDirection == -1 && actualSquare.Y == 6)) && actualSquare.Y + yDirection * 2 < 8 && actualSquare.Y + yDirection * 2 >= 0
                && board[actualSquare.Y + yDirection * 2, actualSquare.X].PieceContainer == null)
            {
                possibleMoves.Add(new Move(board[actualSquare.Y + yDirection * 2, actualSquare.X], false));
                possibleMoves[possibleMoves.Count - 1].RegisterCallback(() =>
                {
                    this.justMovedTwo = true;
                });
                possibleMoves[possibleMoves.Count - 1].RegisterCallbackToReset(() =>
                {
                    this.justMovedTwo = false;
                });
            }
        }
    }

    private void EvaluateAttackPawn(Square[,] board)
    {

        if (actualSquare.Y + yDirection < 8 && actualSquare.Y + yDirection >= 0 && actualSquare.X + 1 < 8 && board[actualSquare.Y + yDirection, actualSquare.X + 1].PieceContainer != null &&
            !board[actualSquare.Y + yDirection, actualSquare.X + 1].PieceContainer.Propietary.Equals(Propietary))
        {
            possibleMoves.Add(new Move(board[actualSquare.Y + yDirection, actualSquare.X + 1]));
            CheckPromotionOnLastMove();
        }
        if (actualSquare.Y + yDirection < 8 && actualSquare.Y + yDirection >= 0 && actualSquare.X - 1 >= 0 && board[actualSquare.Y + yDirection, actualSquare.X - 1].PieceContainer != null &&
            !board[actualSquare.Y + yDirection, actualSquare.X - 1].PieceContainer.Propietary.Equals(Propietary))
        {
            possibleMoves.Add(new Move(board[actualSquare.Y + yDirection, actualSquare.X - 1]));
            CheckPromotionOnLastMove();
        }
    }
    
    private void EvaluateEnPassant(Square[,] board, int modX)
    {
        if (actualSquare.X + modX >= 0 && actualSquare.X + modX < 8 && board[actualSquare.Y, actualSquare.X +modX].PieceContainer != null &&
           !board[actualSquare.Y, actualSquare.X + modX].PieceContainer.Propietary.Equals(Propietary) &&
           board[actualSquare.Y, actualSquare.X + modX].PieceContainer is Pawn)
        {
            Pawn otherPawn = board[actualSquare.Y, actualSquare.X +modX].PieceContainer as Pawn;
            if (otherPawn.justMovedTwo)
            {
                possibleMoves.Add(new Move(board[ActualSquare.Y + yDirection, actualSquare.X +modX]));
                var previousSquare = otherPawn.actualSquare;
                Player previousPropietary = otherPawn.Propietary;
                possibleMoves[possibleMoves.Count - 1].RegisterCallback(() =>
                {
                    otherPawn.actualSquare.SetNewPiece(null);
                    otherPawn.Destroy();
                });
                possibleMoves[possibleMoves.Count - 1].RegisterCallbackToReset(() =>
                {
                    previousSquare.SetNewPiece(otherPawn);
                    previousPropietary.AddPiece(otherPawn);
                });
            }
        }
    }

    public override void Evaluate(Square[,] b)
    {
        if (!evaluatedYet)
        {
            evaluatedYet = true;
            yDirection = this.actualSquare.Y == 1 ? 1 : -1; 
        }
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


        EvaluateFrontMoves(board);
        EvaluateAttackPawn(board);
        EvaluateEnPassant(board, 1);
        EvaluateEnPassant(board, -1); 
       




    }

    private void CheckPromotionOnLastMove()
    {
        if (possibleMoves[possibleMoves.Count-1].Square.Y == 0 || possibleMoves[possibleMoves.Count-1].Square.Y == 7)
        {

            int number = Propietary.ReturnNumberPiece(this);
            possibleMoves[possibleMoves.Count - 1].RegisterCallback(() =>
            {
                if (this.behaviour != null)
                {
                    GameController.Instance.InstantiatePiece(PieceType.QUEEN, this.actualSquare.RelatedBehaviour, Propietary);
                    this.Destroy(); 
                }
                else
                {
                    GameController.Instance.InstantiateQueen(this.actualSquare, Propietary);
                    this.Destroy(); 
                }
            });

            possibleMoves[possibleMoves.Count - 1].RegisterCallbackToReset(() =>
            {
                this.Propietary.DestroyLastPiece();
                this.Propietary.AddPieceInNumber(this,number); 
            }); 
        }
    }

    public override void Move(SquareBehaviour destiny)
    {
        base.Move(destiny);
        positionInitial = false; 
    }
}
