using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Collectible : Interactable
{
    // Item information
    public string itemName;
    public Sprite itemImage;

    public Vector3 posHeld;
    public Vector3 rotHeld;

    public Vector3 scaleHeld;

    public Vector3 posDropped;

    public AudioClip pickUpAudio;

    private Rigidbody rb;
    private Collider collider;

    private MeshRenderer meshRenderer;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        audioSource = GetComponent<AudioSource>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    protected override void Start()
    {
        base.Start();

        audioSource.clip = null;
        interactMessage = "[E] Collect";
    }

    public void Collected()
    {
        rb.isKinematic = true;
        collider.isTrigger = true;
        UpdatePositionOfHeldItem();
    }

    public void PlayPickUpSound()
    {
        if (pickUpAudio != null)
        {
            audioSource.PlayOneShot(pickUpAudio);
        }
    }

    public virtual void DisableObject()
    {
        meshRenderer.enabled = false;
    }

    public virtual void EnableObject()
    {
        meshRenderer.enabled = true;
    }

    private void UpdatePositionOfHeldItem()
    {
        if (rb.isKinematic)
        {
            transform.localPosition = posHeld;
            transform.localRotation = Quaternion.Euler(rotHeld);
            transform.localScale = scaleHeld;
        }
    }

    public void Dropped()
    {
        rb.isKinematic = false;
        collider.isTrigger = false;

        gameObject.SetActive(true);
    }
}
