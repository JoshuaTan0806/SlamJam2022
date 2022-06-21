using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class StatData : ScriptableObject
{
    public Stat Stat;

    public float PercentValue;
    public float FlatValue;
    public float FinalMultiplier;


    [ReadOnly] public float TotalValue;
    [HideInInspector] public float CurrentValue;

    public virtual void CalculateTotal()
    {
        float newValue = FlatValue;
        newValue *= (1 + (PercentValue / 100));
        newValue *= FinalMultiplier;

        TotalValue = newValue;
    }

}
