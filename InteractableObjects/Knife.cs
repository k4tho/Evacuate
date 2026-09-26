using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Knife : Weapon
{

    protected override void Awake()
    {
        base.Awake();

        itemName = "Knife";
        gameObject.tag = "Knife";
        energyExerted = 6f;

        // Physics
        posHeld = new Vector3(-0.02f, 0.07f, -0.07f);
        rotHeld = new Vector3(-66.42f, -82.71f, -19.1f);
        scaleHeld = new Vector3(1.16f, 1.16f, 1.16f);
        posDropped = new Vector3(0f, 0f, 0f);

        // Knife characteristics
        damageInflicted = 35f;
        attackSpeed = 1f;
        attackDelay = .1f;
        attackDistance = 2f;
        soundRadius = 5f;

        // Animation
        attackAnimations.Add("Knife Attack 1");
        attackAnimations.Add("Knife Attack 2");
        
    }
}