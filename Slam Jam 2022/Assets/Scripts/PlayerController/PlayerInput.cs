using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public SpillInput[] SpillArray;
    KeyCode[] inputsTaken;

    private void Start()
    {
        inputsTaken = new KeyCode[SpillArray.Length];

        //Check for input overlaps and remove them
        for(int i = 0; i >= SpillArray.Length; i++)
        {
            for(int e = 0; e >= inputsTaken.Length; e++)
            {
                if(SpillArray[i].Input == inputsTaken[e])
                {
                    SpillArray[i].Input = KeyCode.None;
                }
                else if(inputsTaken[e] == KeyCode.None)
                {
                    inputsTaken[e] = SpillArray[i].Input;
                    break;
                }
            }
        }
    }
}
