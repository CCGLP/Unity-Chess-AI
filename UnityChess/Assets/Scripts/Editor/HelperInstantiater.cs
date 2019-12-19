using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; 

public class HelperInstantiater : MonoBehaviour {

    public static float step = 1.2f;

    public static Vector3 positionInitial = new Vector3(-5.629001f, -11.57f, -3.809f); 

    [MenuItem("Helper/InstantiateQuads")]
    public static void QuadInstantiate()
    {
        Vector3 position = positionInitial; 
        for (int i = 0; i< 8; i++)
        {
            for (int j = 0; j< 8; j++)
            {
                if (Selection.activeGameObject!= null)
                {
                    Instantiate(Selection.activeGameObject, position, Selection.activeGameObject.transform.rotation); 
                }
                position += Vector3.right * step; 
            }
            position = new Vector3(positionInitial.x, positionInitial.y, position.z+ step); 
        }
    }
}
