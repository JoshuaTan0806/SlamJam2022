using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spill-Input", menuName = "SpillInput", order = 1)]
public class SpillInput : ScriptableObject
{
    public KeyCode input;
    GenericSpill spill;
    public GenericSpill Spill
    {
        get { return spill; }
        set { spill = value; }
    }

    PlayerStats player;
    public PlayerStats Player
    {
        get { return player; }
        set { player = value; }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(input))
        {
            spill.Cast(player);
        }
    }
}
