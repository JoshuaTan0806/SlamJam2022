using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp : ScriptableObject
{
    public string Name;

    public virtual void ApplyPowerUp()
    {

    }

    public virtual void UnapplyPowerUp()
    {

    }
}
