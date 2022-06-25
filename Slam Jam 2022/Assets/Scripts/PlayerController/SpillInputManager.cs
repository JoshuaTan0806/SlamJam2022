using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;


public static class SpillInputManager
{
    public static SpillInput[] SpillArray = new SpillInput[4];
    static SpillInput inputClone = null;

    private static void Start()
    {
        InputCheck();
        SpillCheck();

        for (int i = 0; i < SpillArray.Length; i++)
        {
            SpillArray[i].Player = Player.instance;
        }
    }

    public static void UpdateSpills()
    {
        if (!inputClone)
            inputClone = ScriptableObject.CreateInstance<SpillInput>();

        for (int i = 0; i < SpillArray.Length; i++)
        {
            if (SpillArray[i] != null)
                continue;

            SpillArray[i] = Object.Instantiate(inputClone);
        }

        InputCheck();
        SpillCheck();
    }

    /// <summary>
    /// Checks through all spillInputs and removes overlapping inputs
    /// </summary>
    private static void InputCheck()
    {
        //Saves a copy of every input that cannot be used again
        KeyCode[] inputsTaken = new KeyCode[SpillArray.Length];

        //Check for input overlaps and remove them
        for (int i = 0; i < SpillArray.Length; i++)
        {
            for (int e = 0; e < inputsTaken.Length; e++)
            {
                //Have we already used this input here?
                if (SpillArray[i].input == inputsTaken[e])
                {
                    SpillArray[i].input = KeyCode.None;
                }
                else if (inputsTaken[e] == KeyCode.None)
                {
                    inputsTaken[e] = SpillArray[i].input;
                    break;
                }
            }
        }

        //If all inputs are none, give default input bindings
        int noInput = 0;
        for (int i = 0; i < inputsTaken.Length; i++)
        {
            if (inputsTaken[i] == KeyCode.None)
                noInput++;

            if (noInput >= 4)
            {
                noInput = SpillArray.Length;
                for (int e = 0; e < SpillArray.Length; e++)
                {
                    switch (e)
                    {
                        case 0:
                            SpillArray[e].input = KeyCode.Q;
                            break;
                        case 1:
                            SpillArray[e].input = KeyCode.E;
                            break;
                        case 2:
                            SpillArray[e].input = KeyCode.R;
                            break;
                        case 3:
                            SpillArray[e].input = KeyCode.F;
                            break;
                    }

                }
            }

            if (noInput == 0)
                break;
        }
    }

    /// <summary>
    /// Ties the spills the player has equipped to an input
    /// </summary>
    private static void SpillCheck()
    {
        var spillList = ItemInventory.GetSpills();

        //Loop through every spill the player has equipped
        foreach (var spill in spillList)
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
