using System;
using System.IO; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement; 

public class GameController : Singleton<GameController> {

    [Header("Pieces prefabs")]
    [SerializeField]
    private GameObject kingPrefab;
    [SerializeField] private GameObject queenPrefab, rookPrefab, bishopPrefab, knightPrefab, pawnPrefab;

    [SerializeField] private Material materialWhite, materialBlack;

    [SerializeField]
    private CanvasGroup cg;
    [SerializeField]
    private TextMeshProUGUI textEnd, textButtonChange;

    [SerializeField]
    private int queenValue; 
    [SerializeField]
    private SquareTableValues queenSquareTableValues;

    private Player playerOne, playerTwo;
    private Player actualPlayer, otherPlayer;

    
    private MiniMax miniMax;
    private List<PieceBehaviour> allPiecesBehaviour;

    
    private bool is3D = true;



    

    void Start () {

        playerOne = new Player(materialWhite, Player.PlayerType.HUMAN, 7);
        playerTwo = new Player(materialBlack, Player.PlayerType.AI);
        miniMax = new MiniMax(); 

        InstantiatePlayerOnePieces();
        InstantiatePlayerTwoPieces(); 

    
        actualPlayer = playerOne;
        otherPlayer = playerTwo;

        allPiecesBehaviour = new List<PieceBehaviour>(GameObject.FindObjectsOfType<PieceBehaviour>()); 

        EvaluatePlayers(); 
    }


    private void InstantiatePlayerOnePieces()
    {
        var board = Board.Instance.SquareBehaviourMatrix;

        for (int i = 0; i < 8; i++)
        {
            InstantiatePiece(PieceType.PAWN, board[1, i], playerOne);
        }
        InstantiatePiece(PieceType.ROOK, board[0, 0], playerOne);
        InstantiatePiece(PieceType.ROOK, board[0, 7], playerOne);

        InstantiatePiece(PieceType.KNIGHT, board[0, 1], playerOne);
        InstantiatePiece(PieceType.KNIGHT, board[0, 6], playerOne);

        InstantiatePiece(PieceType.BISHOP, board[0, 2], playerOne);
        InstantiatePiece(PieceType.BISHOP, board[0, 5], playerOne);

        InstantiatePiece(PieceType.QUEEN, board[0, 3], playerOne);
        InstantiatePiece(PieceType.KING, board[0, 4], playerOne);
    }

    private void InstantiatePlayerTwoPieces()
    {
        var board = Board.Instance.SquareBehaviourMatrix;
        for (int i = 0; i < 8; i++)
        {
            InstantiatePiece(PieceType.PAWN, board[6, i], playerTwo);
        }
        InstantiatePiece(PieceType.ROOK, board[7, 0], playerTwo);
        InstantiatePiece(PieceType.ROOK, board[7, 7], playerTwo);

        InstantiatePiece(PieceType.KNIGHT, board[7, 1], playerTwo);
        InstantiatePiece(PieceType.KNIGHT, board[7, 6], playerTwo);

        InstantiatePiece(PieceType.BISHOP, board[7, 2], playerTwo);
        InstantiatePiece(PieceType.BISHOP, board[7, 5], playerTwo);

        InstantiatePiece(PieceType.QUEEN, board[7, 3], playerTwo);
        InstantiatePiece(PieceType.KING, board[7, 4], playerTwo);
    }

    /// <summary>
    /// Instantiate a Piece of the given Type in a given square of the board. 
    /// </summary>
    /// <param name="type"></param>
    /// <param name="squareBehaviour"></param>
    /// <param name="player"></param>
    public void InstantiatePiece(PieceType type,SquareBehaviour squareBehaviour, Player player)
    {
        GameObject prefab = null;
        switch (type)
        {
            case PieceType.PAWN:
                prefab = pawnPrefab; 
                break;
            case PieceType.QUEEN:
                prefab = queenPrefab; 
                break;
            case PieceType.ROOK:
                prefab = rookPrefab; 
                break;
            case PieceType.KNIGHT:
                prefab = knightPrefab; 
                break;
            case PieceType.KING:
                prefab = kingPrefab; 
                break;
            case PieceType.BISHOP:
                prefab = bishopPrefab; 
                break; 
        }

       
            var piece = ((GameObject)Instantiate(prefab)).GetComponent<PieceBehaviour>();
            piece.InitGraphics(player.ID); 
            piece.ChangeMaterial(player.AsignedMaterial);
            piece.transform.position = new Vector3(squareBehaviour.transform.position.x, piece.transform.position.y, squareBehaviour.transform.position.z);
            squareBehaviour.Square.SetNewPiece(piece.Piece);
            piece.Piece.SetPropietary(player);
       
    }

    public void ChangeGraphicMode()
    {
        is3D = !is3D;
        if (is3D)
        {
            textButtonChange.text = "3D";
            Camera.main.transform.rotation = Quaternion.Euler(new Vector3(86, 0, 0));
            Camera.main.transform.position = new Vector3(-1.45f, -1.28f, -0.5f);
            Camera.main.orthographic = false; 
            
            for (int i = 0; i< allPiecesBehaviour.Count; i++)
            {
                if (allPiecesBehaviour[i] != null)
                {
                    allPiecesBehaviour[i].ChangeTo3D(); 
                }
            }
        }
        else
        {
            textButtonChange.text = "2D";
            Camera.main.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
            Camera.main.transform.position = new Vector3(-1.45f, -1.28f, 0.17f);
            Camera.main.orthographic = true;
            for (int i = 0; i< allPiecesBehaviour.Count; i++)
            {
                if (allPiecesBehaviour[i]!= null)
                {
                    allPiecesBehaviour[i].ChangeTo2D(); 
                }
            }
        }
    }


    public void Reset()
    {
        SceneManager.LoadScene(0); 
    }


    public void InstantiateQueen(Square square, Player player)
    {

        var piece = new Queen(queenValue, queenSquareTableValues, square, player); 

    }

    public void ChangeTurns()
    {
        if (actualPlayer== playerOne)
        {
            actualPlayer = playerTwo;
            otherPlayer = playerOne;
        }
        else
        {
            actualPlayer = playerOne;
            otherPlayer = playerTwo; 
        }
    }


    public void EvaluatePlayers()
    {
        actualPlayer.Evaluate(null);
        otherPlayer.Evaluate(null);
        actualPlayer.EvaluateCastlings(null, otherPlayer);
        otherPlayer.EvaluateCastlings(null, actualPlayer); 

        actualPlayer.EvaluateCheckOffMoves(otherPlayer);
        otherPlayer.ResetPawnsState();

        if (!actualPlayer.CheckIfCheckMate())
        {
            if (actualPlayer.Type == Player.PlayerType.AI)
            {
                miniMax.RunMiniMax(actualPlayer, otherPlayer, Board.Instance.SquareMatrix); 
            }
        }
        else
        {
            cg.DOFade(1, 0.4f);
            textEnd.text = "Player: " + otherPlayer.ID + ": " + otherPlayer.Type.ToString() + " wins"; 
        }
    }



    public Player ActualPlayer
    {
        get
        {
            return actualPlayer;
        }

    }
}

public enum PieceType
{
    PAWN,
    KING,
    QUEEN, 
    KNIGHT,
    BISHOP, 
    ROOK
}
