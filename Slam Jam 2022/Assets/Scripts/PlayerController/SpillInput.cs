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

    // Update is called once per frame
    public void SpillUpdate()
    {
        if(Input.GetKeyDown(input))
        {
            spill.Cast(Player.instance);
        }

        spill.UpdateSpell();
    }
}
