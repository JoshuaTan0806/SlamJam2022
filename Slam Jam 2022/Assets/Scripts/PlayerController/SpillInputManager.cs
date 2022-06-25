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
    SpillInput[] SpillArray = new SpillInput[4];

    private void Start()
    {
        InputCheck();
        SpillCheck();
        
        for (int i = 0; i < SpillArray.Length; i++)
        {
            SpillArray[i].Player = GetComponent<PlayerStats>();
        }
    }

    /// <summary>
    /// Checks through all spillInputs and removes overlapping inputs
    /// </summary>
    private void InputCheck()
    {
        //Saves a copy of every input that cannot be used again
        KeyCode[] inputsTaken = new KeyCode[SpillArray.Length];

        //Check for input overlaps and remove them
        for (int i = 0; i < SpillArray.Length; i++)
        {
            for(int e = 0; e < inputsTaken.Length; e++)
            {
                //Have we already used this input here?
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

    /// <summary>
    /// Ties the spills the player has equipped to an input
    /// </summary>
    private void SpillCheck()
    {
        var spillList = ItemInventory.GetSpills();

        //Loop through every spill the player has equipped
        foreach(var spill in spillList)
        {
            //Loop through every spillInput
            for (int i = 0; i < SpillArray.Length; i++)
            {
                //Have we already equipped that spill in this function?
                if (spill == SpillArray[i].Spill)
                    break;

                //Is there a spill equipped to that one already?
                if (SpillArray[i].Spill == null)
                {
                    SpillArray[i].Spill = spill;
                    break;
                }
            }
        }
    }
}
