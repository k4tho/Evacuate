using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{
    private void OnEnable()
    {
        SoundSystem.RegisterObserver(this);
    }

    private void OnDisable()
    {
        SoundSystem.UnregisterObserver(this);
    }

    public void OnSoundHeardNearby(Vector3 soundPos)
    {
        Debug.Log(gameObject.name + ": Nearby sound detected");
    }

    public void OnSoundHeardSomewhere(Vector3 soundPos)
    {
        //Debug.Log(gameObject.name + ": Medium sound detected");
    }

    public void OnSoundHeardFarAway(Vector3 soundPos)
    {
        Debug.Log(gameObject.name + ": Far sound detected");
    }
}
