using UHFPS.Runtime;
using UnityEngine;

public class ZombieBodypart : Interactable
{
    ZombieHealth zombieHealth;

    private void Awake()
    {
        zombieHealth = GetComponentInParent<ZombieHealth>();
    }

    public void EnableInteractable()
    {
        gameObject.layer = LayerMask.NameToLayer("Interactable");
        interactMessage = "[E] Search Body";
    }

    public override void Interact()
    {
        zombieHealth.Interact();
    }
}
