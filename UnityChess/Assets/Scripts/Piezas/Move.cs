using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move {
    public delegate void MoveCallback();

    private MoveCallback callback;
    private MoveCallback callbackToReset; 
    private Square square;
    private bool isHarmMove;

    public Move(Square square, bool harmMove = true)
    {
        this.square = square;
        this.callback = null; 
        this.isHarmMove = harmMove; 
    }
    

    public void RegisterCallback(MoveCallback callback)
    {
        this.callback = callback; 
    }

    public void RegisterCallbackToReset(MoveCallback callback)
    {
        this.callbackToReset = callback; 
    }

    public void RunCallbackReset()
    {
        if (this.callbackToReset != null)
        {
            callbackToReset(); 
        }
    }

    public void RunCallback()
    {
        if (this.callback != null)
        {
            callback(); 
        }
    }

    public Square Square
    {
        get
        {
            return square;
        }

    
    }

    

    public bool IsHarmMove
    {
        get
        {
            return isHarmMove;
        }

    }
}
