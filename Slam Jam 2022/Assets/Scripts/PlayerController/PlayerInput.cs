using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;


public class PlayerInput : MonoBehaviour
{
    /// <summary>
    /// The Goal:
    /// Create an array of a maximum of 9 spill inputs that cannot overlap
    /// Get the spills from the player's inventory and attach it to an input
    /// </summary>
    SpillInput[] SpillArray = new SpillInput[8];

    private void Start()
    {
        InputCheck();
        //SpillCheck();
    }

    private void InputCheck()
    {
        KeyCode[] inputsTaken = new KeyCode[SpillArray.Length];

        //Check for input overlaps and remove them
        for (int i = 0; i >= SpillArray.Length; i++)
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

    private void SpillCheck()
    {
        IReadOnlyCollection<GenericSpill> spillList = ItemInventory.GetSpills();
        for (int i = 0; i >= SpillArray.Length; i++)
        {
            //SpillArray[i].Spill
        }
    }
}
