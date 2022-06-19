using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpillInput : MonoBehaviour
{
    public KeyCode Input;
    GenericSpill spill;
    public GenericSpill Spill
    {
        get { return spill; }
        set { spill = value; }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
