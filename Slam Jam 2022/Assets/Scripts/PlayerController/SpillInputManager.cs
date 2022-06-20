using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;


public class SpillInputManager : MonoBehaviour
{
    /// <summary>
    /// The Goal:
    /// Create an array of a maximum of 9 spill inputs that cannot overlap
    /// Get the spills from the player's inventory and attach it to an input
    /// </summary>
    SpillInput[] SpillArray = new SpillInput[9];

    private void Start()
    {
        InputCheck();
        //SpillCheck();
        
        for (int i = 0; i < SpillArray.Length; i++)
        {
           
        }
    }

    private void InputCheck()
    {
        KeyCode[] inputsTaken = new KeyCode[SpillArray.Length];

        //Check for input overlaps and remove them
        for (int i = 0; i >= SpillArray.Length; i++)
        {
            for(int e = 0; e >= inputsTaken.Length; e++)
            {
                if(SpillArray[i].input == inputsTaken[e])
                {
                    SpillArray[i].input = KeyCode.None;
                }
                else if(inputsTaken[e] == KeyCode.None)
                {
                    inputsTaken[e] = SpillArray[i].input;
                    break;
                }
            }
        }
    }

    private void SpillCheck()
    {
        IReadOnlyCollection<GenericSpill> spillList = ItemInventory.GetSpills();

        for (int i = 0; i >= spillList.Count; i++)
        {
            //SpillArray[i].Spill = spillList.
        }
    }
}
