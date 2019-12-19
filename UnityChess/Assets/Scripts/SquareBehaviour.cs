using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareBehaviour : MonoBehaviour {
    private Square square;
    [SerializeField]
    private Color hoverColor, clickedColor, possibleColor;
    private Renderer rend;
    private Color originalColor;

    private bool blockColor = false; 

    private void Awake()
    {
        this.square = new Square(); 
    }
    void Start()
    {

        this.rend = this.GetComponent<Renderer>();
        originalColor = this.rend.sharedMaterial.GetColor("_BaseColor") ;

    }



    private void OnMouseEnter()
    {
        if (!blockColor)
        {
            rend.material.SetColor("_BaseColor", hoverColor);

        }
    }

    private void OnMouseExit()
    {
        if (!blockColor)
            ResetColor();
    }

    public void ResetColor()
    {
        rend.material.SetColor("_BaseColor", originalColor);
        blockColor = false; 
    }

    public void MarkPossible()
    {
        blockColor = true; 
        rend.material.SetColor("_BaseColor", possibleColor); 
    }

    private void OnMouseDown()
    {
        if (!blockColor)
            rend.material.SetColor("_BaseColor", clickedColor) ;
        Board.Instance.SquareClicked(this);
    }






    public Square Square
    {
        get
        {
            return square;
        }

    }

    public bool BlockColor
    {
        get
        {
            return blockColor;
        }

    }
}
