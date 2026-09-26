using System.Collections;
using System.Collections.Generic;
using UHFPS.Runtime;
using UnityEngine;
using UnityEngine.AI;
using static UHFPS.Runtime.NPCHealth;

public class ZombieHealth : MonoBehaviour
{
    private GameObject player;
    private ZombieSounds sounds;
    private float hp;
    private float maxHp;

    // Dead zombie variables
    [SerializeField] public GameObject inventory;
    [SerializeField] private bool startDead = false;


    public struct BodySegment2
    {
        public Rigidbody Rigidbody;
        public Collider Collider;

        public BodySegment2(Rigidbody rigidbody, Collider collider)
        {
            Rigidbody = rigidbody;
            Collider = collider;
        }
    }

    public List<BodySegment2> BodySegments = new();
    public List<Component> DisableComponents = new();

    protected void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        sounds = GetComponent<ZombieSounds>();

        maxHp = 100f;
        hp = maxHp;

        if (inventory != null)
        {
            inventory.SetActive(false);
        }

        BodySegments.Clear();
        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            if (child == this.transform) continue; // skip self

            Collider col = child.GetComponent<Collider>();
            Rigidbody rb = child.GetComponent<Rigidbody>();

            if (rb != null)
            {
                BodySegments.Add(new(rb, col));
            }
        }

        DisableComponents.Add(GetComponent<Animator>());
        DisableComponents.Add(GetComponent<NavMeshAgent>());
        DisableComponents.Add(GetComponent<Collider>());
        DisableComponents.Add(GetComponent<NPCStateMachine>());

        if (startDead == true)
        {
            hp = 0;
            StartCoroutine(DelayedDie());
        }
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        sounds.UpdateZombieState(ZombieSounds.ZombieState.Hurt);

        if (hp <= 0)
        {
            Die();
        }
    }
    
    private void Die()
    {
        if (!startDead && sounds != null)
        {
            sounds.UpdateZombieState(ZombieSounds.ZombieState.Dead);
        }

        EnableRagdoll(true);
        StartCoroutine(WaitToEnableInteractable());
    }

    private IEnumerator DelayedDie()
    {
        yield return null; // Wait 1 frame to ensure all components are initialized
        Die();
    }


    private void EnableRagdoll(bool enabled)
    {
        foreach (var component in DisableComponents)
        {
            if (component is Behaviour behaviour)
                behaviour.enabled = false;
            else if (component is Collider collider)
                collider.enabled = false;
        }

        foreach (BodySegment2 bodyPart in BodySegments)
        {
            if (enabled)
            {
                bodyPart.Rigidbody.isKinematic = false;
                bodyPart.Rigidbody.useGravity = true;
                bodyPart.Collider.isTrigger = false;
            }
            else
            {
                bodyPart.Rigidbody.isKinematic = true;
                bodyPart.Rigidbody.useGravity = false;
                bodyPart.Collider.isTrigger = true;
            }
        }
    }

    public void Interact()
    {
        if (inventory != null)
        {
            player.GetComponent<PlayerController>().UpdateCommentary(player.GetComponent<PlayerInventory>().AddItem(inventory));
            inventory = null;
        }
        else
        {
            player.GetComponent<PlayerController>().UpdateCommentary("Nothing Found");
        }
    }

    IEnumerator WaitToEnableInteractable()
    {
        yield return new WaitForSeconds(2f);

        foreach (ZombieBodypart child in GetComponentsInChildren<ZombieBodypart>())
        {
            child.EnableInteractable();
        }
    }
}