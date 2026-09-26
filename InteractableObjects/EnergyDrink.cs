using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyDrink : HealthKit
{
    protected override void Awake()
    {
        base.Awake();
        itemName = "EnergyDrink";
        gameObject.tag = "EnergyDrink";

        // Physics
        posHeld = new Vector3(-0.02f, .08f, -.08f);
        rotHeld = new Vector3(-38.76f, 90f, 0f);
        posDropped = new Vector3(0f, 0f, 0f);

        healthIncrease = 15;
    }
}
