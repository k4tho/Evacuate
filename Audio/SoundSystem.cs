using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSystem : MonoBehaviour
{
    private static List<Observer> observers = new List<Observer>();

    public static void RegisterObserver(Observer observer)
    {
        if (!observers.Contains(observer))
        {
            observers.Add(observer);
        }
    }

    public static void UnregisterObserver(Observer observer)
    {
        if (observers.Contains(observer))
        {
            observers.Remove(observer);
        }
    }

    public static void NotifySound(Vector3 soundPos, float hearingRadius)
    {
        foreach (Observer observer in observers)
        {
            float distance = Vector3.Distance(observer.transform.position, soundPos);
            if (distance <= (hearingRadius/2f)) // Notify observers within close range
            {
                observer.OnSoundHeardNearby(soundPos);
            }
            else if (distance <= (((hearingRadius/2f) + (hearingRadius/ 4f)))){
                observer.OnSoundHeardSomewhere(soundPos);
            }
            else if (distance <= hearingRadius)
            {
                observer.OnSoundHeardFarAway(soundPos);
            }
        }
    }
}
