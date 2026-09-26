using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Antibiotics : HealthKit
{
    protected override void Awake()
    {
        base.Awake();
        itemName = "Antibiotics";
        gameObject.tag = "Antibiotics";

        // Physics
        posHeld = new Vector3(0.09342346f, 0.145332f, -0.03577848f);
        rotHeld = new Vector3(-176.769f, -13.108f, 437.707f);
        scaleHeld = new Vector3(0.42f, 0.42f, 0.42f);
        posDropped = new Vector3(0f, 0f, 0f);

        healthIncrease = 12;
        
    }
}