using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class KeypadPuzzle1 : Interactable
{
    // Item information
    public string itemName;

    private Rigidbody rb;
    private Collider collider;

    protected void Awake()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        audioSource = GetComponent<AudioSource>();

        audioSource.clip = null;
        interactMessage = "[F] Interact";
    }
}