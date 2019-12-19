using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="SquareTableValue", menuName = "Chess", order = 0)]
public class SquareTableValues : ScriptableObject {

   
    public int[,] squareValues = new int[8,8];
    public TextAsset jsonData;


    public void Init()
    {
        squareValues = new int[8, 8];
        SquareTableJSONWrapper wrapper = JsonUtility.FromJson<SquareTableJSONWrapper>(jsonData.text);
        for (int i = 0; i< 8; i++)
        {
            squareValues[0, i] = wrapper.firstRow[i];
        }


        for (int i = 0; i < 8; i++)
        {
            squareValues[1, i] = wrapper.secondRow[i];
        }


        for (int i = 0; i < 8; i++)
        {
            squareValues[2, i] = wrapper.thirdRow[i];
        }


        for (int i = 0; i < 8; i++)
        {
            squareValues[3, i] = wrapper.fourthRow[i];
        }


        for (int i = 0; i < 8; i++)
        {
            squareValues[4, i] = wrapper.fifthRow[i];
        }



        for (int i = 0; i < 8; i++)
        {
            squareValues[5, i] = wrapper.sixthRow[i];
        }


        for (int i = 0; i < 8; i++)
        {
            squareValues[6, i] = wrapper.seventhRow[i];
        }


        for (int i = 0; i < 8; i++)
        {
            squareValues[7, i] = wrapper.eigthRow[i];
        }
    }
}






//Blame the Unity Serialization module, not me. 
[System.Serializable]
public class SquareTableJSONWrapper
{

    public int[] firstRow;
    public int[] secondRow;
    public int[] thirdRow;
    public int[] fourthRow;
    public int[] fifthRow;
    public int[] sixthRow;
    public int[] seventhRow;
    public int[] eigthRow; 
}