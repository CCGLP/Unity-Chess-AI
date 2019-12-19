using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : Singleton<Board> {

    private Square[,] squareMatrix;
    private SquareBehaviour[,] squareBehaviourMatrix;
    private SquareBehaviour activeSquare = null;

    private List<Move> possibleMovesActive = null; 


    protected override void Awake () {
        base.Awake(); 
        squareMatrix = new Square[8,8];
        squareBehaviourMatrix = new SquareBehaviour[8, 8]; 
        var gameSquares = GameObject.Find("Squares").transform; 
        int counter = 0; 
        for (int i = 0; i< 8; i++)
        {
            for (int j = 0; j< 8; j++)
            {
                squareBehaviourMatrix[i, j] = gameSquares.GetChild(counter).GetComponent<SquareBehaviour>(); 
                squareMatrix[i, j] = squareBehaviourMatrix[i,j].Square;
                squareMatrix[i, j].InitPosition(j, i); 
                counter++; 
            }
        }

	}
    

    public void SquareClicked(SquareBehaviour square)
    {
        if (square.Square.PieceContainer != null && square.Square.PieceContainer.Propietary.Equals(GameController.Instance.ActualPlayer) &&
            (activeSquare == null || activeSquare.Square.PieceContainer.Propietary.Equals(square.Square.PieceContainer.Propietary))
            && square.Square.PieceContainer.Propietary.Type == Player.PlayerType.HUMAN)
        {
            ResetMoves(); 
            activeSquare = square;
            possibleMovesActive = activeSquare.Square.PieceContainer.PossibleMoves; 
            for (int i = 0; i< possibleMovesActive.Count; i++)
            {
                possibleMovesActive[i].Square.RelatedBehaviour.MarkPossible(); 
            }
        }

        else if (activeSquare != null && square.BlockColor)
        {
            activeSquare.Square.PieceContainer.Move(square);
            GetMoveFromPossibles(square.Square).RunCallback(); 
            activeSquare = null;
            ResetMoves();
            GameController.Instance.ChangeTurns(); 
            GameController.Instance.EvaluatePlayers();
            
        }
    }

    private Move GetMoveFromPossibles (Square square)
    {
        for (int i = 0; i< possibleMovesActive.Count; i++)
        {
            if (possibleMovesActive[i].Square.Equals(square))
            {
                return possibleMovesActive[i]; 
            }
        }
        return null; 
    }

    private void ResetMoves()
    {
        if (possibleMovesActive != null)
        {
            for (int i = 0; i < possibleMovesActive.Count; i++)
            {
                possibleMovesActive[i].Square.RelatedBehaviour.ResetColor();
            }
        }
    }


    public Square[,] GenerateBoardCopy()
    {
        Square[,] copy = new Square[8, 8]; 
        for (int i = 0; i< 8; i++)
        {
            for (int j = 0; j< 8; j++)
            {
                copy[i, j] = new Square(squareMatrix[i, j]); 
            }
        }
        return copy; 
    }

    public Square[,]GenerateBoardCopy(Square[,] board)
    {
        Square[,] copy = new Square[8, 8];
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                copy[i, j] = new Square(board[i, j]);
            }
        }
        return copy;
    }





    public Square[,] SquareMatrix
    {
        get
        {
            return squareMatrix;
        }
    }

    public SquareBehaviour[,] SquareBehaviourMatrix
    {
        get
        {
            return squareBehaviourMatrix;
        }
    }
}
