using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bandages : HealthKit
{
    private int maxNumUses;
    private int remainingUses;

    protected override void Awake()
    {
        base.Awake();

        itemName = "First Aid Kit";
        gameObject.tag = "FirstAidKit";

        // Physics
        posHeld = new Vector3(0.032f, 0.251f, -0.038f);
        rotHeld = new Vector3(35.624f, -374.855f, -151.585f);
        scaleHeld = new Vector3(0.57f, 0.57f, 0.57f);
        posDropped = new Vector3(0f, 0f, 0f);

        healthIncrease = 10;

        maxNumUses = 3;
        remainingUses = maxNumUses;
    }

    public override float UseMeds()
    {
        remainingUses--;

        if (maxNumUses == 0)
        {
            Destroy(gameObject);
        }

        return base.UseMeds();
    }
}