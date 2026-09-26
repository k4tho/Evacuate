using UHFPS.Runtime;
using UnityEngine;

public class FuseDoor : Interactable
{
    FuseboxPuzzle puzzle;

    void Awake()
    {
        puzzle = GetComponentInParent<FuseboxPuzzle>();
    }

    public override void Interact()
    {
        base.Interact();
        puzzle.InteractStart();
    }
}
