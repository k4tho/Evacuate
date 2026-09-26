using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class HealthKit : Collectible
{
    // protected PlayerHealth playerHealth;
    // protected PlayerEnergy playerEnergy;

    protected float healthIncrease;
    
    protected virtual void Awake()
    {
        base.Awake();
    }

    public virtual float UseMeds()
    {
        return healthIncrease;
    }
}