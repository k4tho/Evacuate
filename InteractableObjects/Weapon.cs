using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : Collectible
{
    protected float damageInflicted;
    protected float attackDelay;        // How long after the attack is complete before attack is ready again
    protected float attackSpeed;        // How long it takes to complete the attack
    protected float energyExerted;
    protected float attackDistance;

    // Animation
    protected List<string> attackAnimations;
    protected int animationIndex;

    // Sound Effects
    public List<AudioClip> attackAudioClips;
    private int attackClipIndex;

    protected float soundRadius;        // How far the attack noise reaches.

    protected virtual void Awake()
    {
        base.Awake();

        attackAnimations = new List<string>();
        animationIndex = -1;
        attackClipIndex = -1;
    }

    public virtual void Attack(GameObject target)
    {
        // Only take damage if a target was hit
        if (target != null)
        {
            target.GetComponent<ZombieHealth>().TakeDamage(damageInflicted);
        }
    }

    public virtual float GetEnergyExerted()
    {
        return energyExerted;
    }

    public virtual float GetAttackSpeed()
    {
        return attackSpeed;
    }

    public virtual float GetAttackDelay()
    {
        return attackDelay;
    }

    public virtual float GetAttackDistance()
    {
        return attackDistance;
    }

    public virtual float GetSoundRadius()
    {
        return soundRadius;
    }

    public virtual string GetAttackAnimation()
    {
        if (attackAnimations.Count == 0) { return ""; }
        animationIndex++;

        if (animationIndex >= attackAnimations.Count)
        {
            animationIndex = 0;
            return attackAnimations[animationIndex];
        }
        else
        {
            return attackAnimations[animationIndex];
        }
    }

    protected virtual void PlayAttackSounds()
    {
        if (attackAudioClips.Count == 0)
        {
            Debug.Log("No attack AUdio to play for " + gameObject.name);
            return;
        }

        attackClipIndex++;

        if (attackClipIndex == attackAudioClips.Count)
        {
            attackClipIndex = 0;
        }

        audioSource.clip = attackAudioClips[attackClipIndex];
        audioSource.Play();
    }
}
