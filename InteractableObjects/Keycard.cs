using UnityEngine;

public class Keycard : Collectible
{
    protected override void Start()
    {
        base.Start();

        itemName = "Keycard";
        gameObject.tag = "Keycard";

        // Physics
        posHeld = new Vector3(0.073f, 0.08f, -0.063f);
        rotHeld = new Vector3(156.7f, -113.68f, -0.11f);
        scaleHeld = new Vector3(1.25f, 1.25f, 1.25f);
        posDropped = new Vector3(0f, 0f, 0f);
    }
}
