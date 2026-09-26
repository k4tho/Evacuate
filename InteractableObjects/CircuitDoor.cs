using UHFPS.Runtime;
using UnityEngine;

public class CircuitDoor : Interactable
{
    ElectricalCircuitPuzzle puzzle;

    void Awake()
    {
        puzzle = GetComponentInParent<ElectricalCircuitPuzzle>();
    }

    protected override void Start()
    {
        base.Start();
        interactMessage = "[E] To Fix Electrical Circuit";
    }

    public override void Interact()
    {
        base.Interact();
        puzzle.InteractStart();
    }
}
