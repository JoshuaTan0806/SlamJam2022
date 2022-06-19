using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpillInput : MonoBehaviour
{
    public KeyCode input;
    GenericSpill spill;
    public GenericSpill Spill
    {
        get { return spill; }
        set { spill = value; }
    }

    PlayerStats player;

    private void Start()
    {
        player = GetComponent<PlayerStats>();
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
