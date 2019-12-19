using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceBehaviour : MonoBehaviour {
    [SerializeField]
    private PieceType type;
    [SerializeField]
    private int pieceValue;
    [SerializeField]
    private SquareTableValues values;
    private GameObject child3D, child2D;
    [SerializeField]
    private Sprite spriteWhite, spriteBlack;
    private SpriteRenderer spriteRend; 
    private Piece piece;

    public Piece Piece
    {
        get
        {
            return piece;
        }

    }


    void Awake () {
        values.Init(); 
        switch (type)
        {
            case PieceType.BISHOP:
                piece = new Bishop(this, pieceValue, values); 
                break;
            case PieceType.KING:
                piece = new King(this, pieceValue, values);
                break;

            case PieceType.KNIGHT:
                piece = new Knight(this, pieceValue, values); 
                break;

            case PieceType.PAWN:
                piece = new Pawn(this, pieceValue, values); 
                break;

            case PieceType.QUEEN:
                piece = new Queen(this, pieceValue, values); 
                break;

            case PieceType.ROOK:
                piece = new Rook(this, pieceValue, values); 
                break; 
        }
	}

    public void InitGraphics(int id) {
        child3D = this.transform.GetChild(0).gameObject;
        child2D = this.transform.GetChild(1).gameObject;
        spriteRend = this.GetComponentInChildren<SpriteRenderer>();
        spriteRend.sprite = id == 0 ? spriteWhite : spriteBlack; 
        child2D.SetActive(false);
    }
    public void ChangeTo3D()
    {
        child3D.SetActive(true);
        child2D.SetActive(false); 
    }
    public void ChangeTo2D()
    {
        child2D.SetActive(true);
        child3D.SetActive(false); 
    }

    public void ChangeMaterial(Material material)
    {
        var rends = this.GetComponentsInChildren<MeshRenderer>(); 
        for (int i= 0; i< rends.Length; i++)
        {
            rends[i].material = material; 
        }
    }

}
