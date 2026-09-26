using UnityEngine;

public class LoreNotes : Collectible
{
    public GameObject noteInUi;

    protected override void Awake()
    {
        base.Awake();

        itemName = "Note";
        gameObject.tag = "Note";

        posHeld = new Vector3(0.102f, 0.125f, -0.096f);
        rotHeld = new Vector3(131.8f, 20.57f, -37.43f);
        posDropped = new Vector3(0f, 0f, 0f);
    }

    protected override void Start()
    {
        base.Start();
    }

    public override void Interact()
    {
        base.Interact();

        noteInUi.SetActive(true);
    }
}
