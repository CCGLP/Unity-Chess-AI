using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMax
{



    private Piece pieceToMove;
    private Move bestMove;
    private int maxDepth = 3;
    private int vueltas = 0;



    public void RunMiniMax(Player maxPlayer, Player minPlayer, Square[,] board)
    {
        vueltas = 0;
        int maxValue = MiniMaxFunctionRoot(maxPlayer, minPlayer, board);
       
        Debug.Log("Número de tableros evaluados: " + vueltas.ToString());
        pieceToMove.Move(bestMove.Square.RelatedBehaviour);
        bestMove.RunCallback();

        GameController.Instance.ChangeTurns();
        GameController.Instance.EvaluatePlayers();
    }

    public int MiniMaxFunctionRoot(Player maxPlayer, Player minPlayer, Square[,] board)
    {
        int bestValue = int.MinValue;
        for (int i = 0; i < maxPlayer.Pieces.Count; i++)
        {
            for (int j = 0; j < maxPlayer.Pieces[i].PossibleMoves.Count; j++)
            {

                // Genero copias en el root para evaluar cada movimiento y se lo paso al MiniMax
                Square[,] boardCopy = Board.Instance.GenerateBoardCopy(board);
                Player maxCopy = new Player(maxPlayer, boardCopy);
                Player minCopy = new Player(minPlayer, boardCopy);

                maxCopy.Evaluate(boardCopy);
                minCopy.Evaluate(boardCopy);

                maxCopy.EvaluateCastlings(boardCopy, minCopy);
                minCopy.EvaluateCastlings(boardCopy, maxCopy);

                maxCopy.EvaluateCheckOffMoves(minCopy, boardCopy);
                minCopy.ResetPawnsState();

                maxCopy.Pieces[i].Move(maxCopy.Pieces[i].PossibleMoves[j].Square);
                maxCopy.Pieces[i].PossibleMoves[j].RunCallback();

                int value =  //-Negamax(minCopy, maxCopy, boardCopy, false, maxDepth - 1, int.MinValue, int.MaxValue);
                MiniMaxFunction(maxCopy, minCopy, boardCopy, false, maxDepth - 1, int.MinValue, int.MaxValue); //
                if (value >= bestValue)
                {
                    bestValue = value;
                    bestMove = maxPlayer.Pieces[i].PossibleMoves[j];
                    pieceToMove = maxPlayer.Pieces[i];

                }


            }
        }
        return bestValue;
    }

    public int MiniMaxFunction(Player maxPlayer, Player minPlayer, Square[,] board, bool maxTurn, int depth, int alpha, int beta)
    {
        if (depth == 0)
        {
            return EvaluateBoard(maxPlayer, minPlayer) ;
        }

        //Genero los movimientos posibles de cada jugador
        Player actualPlayer = maxTurn ? maxPlayer : minPlayer;
        Player otherPlayer = maxTurn ? minPlayer : maxPlayer;

        actualPlayer.Evaluate(board);
        otherPlayer.Evaluate(board);
        actualPlayer.EvaluateCastlings(board, otherPlayer);
        otherPlayer.EvaluateCastlings(board, actualPlayer);
        actualPlayer.EvaluateCheckOffMoves(otherPlayer, board);
        otherPlayer.ResetPawnsState();

        int bestValue = maxTurn ? int.MinValue : int.MaxValue;
        for (int i = 0; i < actualPlayer.Pieces.Count; i++)
        {
            var piece = actualPlayer.Pieces[i];
            var moves = actualPlayer.Pieces[i].PossibleMoves;
            for (int j = 0; j < actualPlayer.Pieces[i].PossibleMoves.Count; j++)
            {
                vueltas++;

                var previousPiece = moves[j].Square.PieceContainer;
                var previousSquare = piece.ActualSquare;
                //Muevo para luego deshacer el movimiento
                piece.Move(moves[j].Square);
                piece.PossibleMoves[j].RunCallback();

                Square[,] copyBoard = Board.Instance.GenerateBoardCopy(board);
      
                if (maxTurn) //Max
                {

                    bestValue = Mathf.Max(bestValue, MiniMaxFunction(new Player(actualPlayer, copyBoard), new Player(otherPlayer, copyBoard), copyBoard, !maxTurn, depth - 1, alpha, beta));
                    alpha = Mathf.Max(bestValue, alpha);
                }
                else //Min
                {
                    bestValue = Mathf.Min(bestValue, MiniMaxFunction(new Player(otherPlayer, copyBoard), new Player(actualPlayer, copyBoard), copyBoard, !maxTurn, depth - 1, alpha, beta));
                    beta = Mathf.Min(beta, bestValue);
                }

                //Deshago movimiento
                piece.Move(previousSquare);
                otherPlayer.AddPiece(previousPiece);
                if (previousPiece != null)
                {
                    previousPiece.Move(board[moves[j].Square.Y, moves[j].Square.X]);
                }
                piece.PossibleMoves[j].RunCallbackReset();

                //Podo
                if (beta <= alpha)
                {
                    return bestValue;
                }
               


            }
        }

        //Si no hay movimientos posibles, devuelvo bestValue, que es el mínimo score posible si estoy en max y el máximo en min (Jaque mate)
        return bestValue  ;
    }



    public int Negamax(Player actualPlayer, Player otherPlayer, Square[,] board, bool maxTurn, int depth, int alpha, int beta)
    {
        if (depth == 0)
        {
            return (EvaluateBoard(actualPlayer, otherPlayer)* (maxTurn? -1 : 1));
        }


        //Genero los movimientos posibles de cada jugador
        actualPlayer.Evaluate(board);
        otherPlayer.Evaluate(board);
        actualPlayer.EvaluateCastlings(board, otherPlayer);
        otherPlayer.EvaluateCastlings(board, actualPlayer);
        actualPlayer.EvaluateCheckOffMoves(otherPlayer, board);
        otherPlayer.ResetPawnsState();

        int bestValue =int.MinValue;
        beta = beta == int.MinValue ? int.MaxValue : beta;  //Int bug.
        for (int i = 0; i < actualPlayer.Pieces.Count; i++)
        {
            var piece = actualPlayer.Pieces[i];
            var moves = actualPlayer.Pieces[i].PossibleMoves;
            for (int j = 0; j < actualPlayer.Pieces[i].PossibleMoves.Count; j++)
            {
                vueltas++;

                var previousPiece = moves[j].Square.PieceContainer;
                var previousSquare = piece.ActualSquare;
                //Muevo para luego deshacer el movimiento
                piece.Move(moves[j].Square);
                piece.PossibleMoves[j].RunCallback();

                Square[,] copyBoard = Board.Instance.GenerateBoardCopy(board);
                int value =-Negamax(new Player(otherPlayer, copyBoard), new Player(actualPlayer, copyBoard), copyBoard, !maxTurn, depth - 1, -beta, -alpha);
                
                if (bestValue < value)
                {
                    bestValue = value; 
                }

                alpha = Mathf.Max(alpha, bestValue);



                //Deshacer movimiento
                piece.Move(previousSquare);
                otherPlayer.AddPiece(previousPiece);
                if (previousPiece != null)
                {
                    previousPiece.Move(board[moves[j].Square.Y, moves[j].Square.X]);
                }
                piece.PossibleMoves[j].RunCallbackReset();

                //Podar
                if (beta <= alpha)
                {
                    return bestValue;
                }
                
              

            }



        }

        //Si no hay movimientos posibles, devuelvo bestValue, que es el mínimo score posible si estoy en max y el máximo en min (Jaque mate)
        return bestValue;
    }







    public int EvaluateBoard(Player max, Player min)
    {
        return max.EvaluateBoardValue() - min.EvaluateBoardValue();
    }


}
