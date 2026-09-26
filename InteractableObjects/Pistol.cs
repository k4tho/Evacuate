using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class Pistol : Weapon
{
    private MuzzleFlashTest muzzleFlash;

    // Audio
    public AudioClip reloadAudioClip;
    public AudioClip ammoOutAudioClip;

    public GameObject pistolUi; // Game object to enable and disable pistol ui
    private Text ammoText;

    private int maxBullets;
    private int currBullets;
    private float reloadTime;

    private MeshRenderer[] meshRenderers;
    private Rigidbody[] rbs;
    

    protected override void Awake()
    {
        base.Awake();
        muzzleFlash = GetComponent<MuzzleFlashTest>();
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
        rbs = GetComponentsInChildren<Rigidbody>();

        itemName = "Pistol";
        gameObject.tag = "Pistol";
        energyExerted = 0f;

        posHeld = new Vector3(.03879689f, .1683568f, -0.02982334f);
        rotHeld = new Vector3(-57.485f, -95.803f, -21.812f);
        scaleHeld = new Vector3(1f, 1f, 1f);
        posDropped = new Vector3(0f, 0f, 0f);

        // Pistol characteristics
        damageInflicted = 105f;
        attackSpeed = .4f;
        attackDelay = .005f;
        attackDistance = 100f;
        soundRadius = 20f;

        // Animation
        attackAnimations.Add("Pistol Shoot");

        // Ammo
        maxBullets = 8;
        currBullets = maxBullets;

        foreach (Rigidbody rb in rbs)
        {
            rb.isKinematic = true;
        }

        SetPistolActive(false); // Disable pistol ui at start
        ammoText = pistolUi.GetComponentInChildren<Text>();
    }

    protected override void Start()
    {
        base.Start();

        RuntimeAnimatorController controller = GameObject.FindGameObjectWithTag("PlayerArms").GetComponent<Animator>().runtimeAnimatorController;

        foreach (AnimationClip clip in controller.animationClips)
        {
            if (clip.name == "PistolShoot")
            {
                attackSpeed = clip.length;
            }
            else if (clip.name == "PistolReload")
            {
                reloadTime = clip.length;
            }
        }
    }

    public override void DisableObject()
    {
        foreach (MeshRenderer mr in meshRenderers)
        {
            mr.enabled = false;
        }
        foreach (Rigidbody rb in rbs)
        {
            rb.isKinematic = true;
        }
    }

    public override void EnableObject()
    {
        foreach (MeshRenderer mr in meshRenderers)
        {
            mr.enabled = true;
        }
        foreach (Rigidbody rb in rbs)
        {
            rb.isKinematic = false;
        }
    }

    public override string GetAttackAnimation()
    {
        if (currBullets > 0)
        {
            return base.GetAttackAnimation();
        }
        return "";
    }

    public override void Attack(GameObject target)
    {
        base.Attack(target);

        PlayAttackSounds();
        if (currBullets > 0)
        {
            muzzleFlash.PlayEffect();
            currBullets--;
            UpdateAmmoUI(currBullets);
        }
    }

    public void Reload()
    {
        currBullets = maxBullets;
        PlayReloadSounds();
        UpdateAmmoUI(currBullets);
    }

    public string GetReloadAnimation()
    {
        return "Pistol Reload";
    }

    public override float GetAttackDelay()
    {
        // There is no delay if there are no bullets to shoot
        if (currBullets <= 0)
        {
            return 0f;
        }
        return base.GetAttackDelay();
    }

    public override float GetAttackSpeed()
    {
        // There is no attack speed if there are no bullets to shoot
        if (currBullets <= 0)
        {
            return 0f;
        }
        return base.GetAttackSpeed();
    }

    public float GetReloadTime()
    {
        return reloadTime;
    }

    protected void UpdateAmmoUI(int bulletsRemaining)
    {
        ammoText.text = bulletsRemaining.ToString() + "/" + maxBullets.ToString();
    }

    protected override void PlayAttackSounds()
    {
        // Plays shooting sounds as long as there is still ammo
        if (currBullets > 0)
        {
            base.PlayAttackSounds();
        }
        else
        {
            audioSource.clip = ammoOutAudioClip;
            audioSource.Play();
        }
    }

    protected void PlayReloadSounds()
    {
        audioSource.clip = reloadAudioClip;
        audioSource.Play();
    }
    
    public void SetPistolActive(bool active) //Reveals the UI of the pistol icon once collected
    {
        pistolUi.SetActive(active);
    }
}
