using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CollectibleSounds : MonoBehaviour
{
    public AudioSource AudioSource; 
    public List<AudioClip> SoundClips;

    private List<int> SoundQueue;
    private int lastSoundIndex = -1;
  
    
    public void PlayRandomSound()
    {
        int soundIndex = Random.Range(0, SoundClips.Count);
        while (soundIndex == lastSoundIndex)
            soundIndex = Random.Range(0, SoundClips.Count);
        AudioSource.PlayOneShot(SoundClips[soundIndex]);
        lastSoundIndex = soundIndex;
    }
    
    public void PlayRandomPickUpSoundOld()
    {
        Debug.Log("played sound");
        
        if (SoundQueue.Count == 0)
            InitializeSoundQueue();
        
        int indexToPlay = SoundQueue[0];
        SoundQueue.RemoveAt(0);

        AudioSource.clip = SoundClips[indexToPlay];
        AudioSource.Play();
    }

    private void InitializeSoundQueue()
    {
        List<int> numberPool = new List<int>();
        for (int i = 0; i < SoundClips.Count; i++)
            numberPool.Add(i);
        
        SoundQueue = new List<int>();
        for (int i = 0; i < SoundClips.Count; i++)
        {
            int randomIndex = Random.Range(0, numberPool.Count);
            SoundQueue.Add(numberPool[randomIndex]);
            numberPool.RemoveAt(randomIndex);
        }
    }
}
