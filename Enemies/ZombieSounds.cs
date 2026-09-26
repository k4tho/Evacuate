using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSounds : MonoBehaviour
{
    private AudioSource mainAudioSource;
    private AudioSource secondaryAudioSource;

    public List<AudioClip> deathSounds;
    public List<AudioClip> groanSounds;
    public List<AudioClip> attackSounds;
    public List<AudioClip> chaseSounds;
    public List<AudioClip> hurtSounds;

    private bool isPlayingFirst = true;
    public float fadeDuration = 1.0f; // Time for crossfade
    private int currentClipIndex = 0;
    private Coroutine audioCoroutine;
    private ZombieState prevState = ZombieState.None;
    private ZombieState currState = ZombieState.None;

    public enum ZombieState { Idle, Chase, Hurt, Attack, Dead, None}

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainAudioSource = gameObject.AddComponent<AudioSource>();
        secondaryAudioSource = gameObject.AddComponent<AudioSource>();
        mainAudioSource.spatialBlend = 1f;
        mainAudioSource.maxDistance = 5f;
        secondaryAudioSource.spatialBlend = 1f;
        secondaryAudioSource.maxDistance = 5f;
        mainAudioSource.loop = false;
        secondaryAudioSource.loop = false;

        UpdateZombieState(ZombieState.Idle);
    }

    void Update()
    {
        // After hurt/attack sound is played (one time played), it goes back to previous state whether that's chasing or idle
        if ((currState == ZombieState.Hurt || currState == ZombieState.Attack) && !mainAudioSource.isPlaying)
        {
            UpdateZombieState(prevState);
        }
        // If zombie is dead and sound has finished, just remove this script
        if (currState == ZombieState.Dead && !mainAudioSource.isPlaying)
        {
            Destroy(this);
        }
    }

    public void UpdateZombieState(ZombieState state)
    {
        switch (state)
        {
            case ZombieState.Idle:
                if (currState == ZombieState.Idle) return;              // Don't need to play more zombie idle sounds if that is already its state
                currState = ZombieState.Idle;
                prevState = ZombieState.None;
                PlayIdleSounds();
                break;
            case ZombieState.Chase:
                currState = ZombieState.Idle;
                prevState = ZombieState.None;
                PlayChaseSounds();
                break;
            case ZombieState.Hurt:
                if ((currState != ZombieState.Attack) && (currState != ZombieState.Hurt))
                {
                    prevState = currState;                              // Save previous state
                }
                currState = ZombieState.Hurt;
                PlayHurtSounds();
                break;
            case ZombieState.Attack:
                if ((currState != ZombieState.Attack) && (currState != ZombieState.Hurt))
                {
                    prevState = currState;                              // Save previous state if it was idle or chasing
                }
                currState = ZombieState.Attack;
                PlayAttackSounds();
                break;
            case ZombieState.Dead:
                currState = ZombieState.Dead;
                PlayDeadSounds();
                break;
        }
    }

    private void PlayIdleSounds()
    {
        // Stop crossfade coroutine
        if (audioCoroutine != null)
        {
            StopCoroutine(audioCoroutine);
            audioCoroutine = null;
        }

        isPlayingFirst = true;
        currentClipIndex = 0;

        // Set default properties
        mainAudioSource.volume = 1f;
        secondaryAudioSource.volume = 0f; // Second one starts silent

        audioCoroutine = StartCoroutine(PlayNextIdleClip());
    }

    IEnumerator PlayNextIdleClip()
    {
        while (true)
        {
            // Determine which audio source is fading in and out
            AudioSource fadingOut = isPlayingFirst ? mainAudioSource : secondaryAudioSource;
            AudioSource fadingIn = isPlayingFirst ? secondaryAudioSource : mainAudioSource;

            // Set new clip and play it
            fadingIn.clip = groanSounds[currentClipIndex];
            fadingIn.Play();

            // Crossfade over fadeDuration
            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / fadeDuration;
                fadingOut.volume = Mathf.Lerp(1f, 0f, t);
                fadingIn.volume = Mathf.Lerp(0f, 1f, t);
                yield return null;
            }

            // Ensure volumes are set exactly at the end
            fadingOut.volume = 0f;
            fadingIn.volume = 1f;
            fadingOut.Stop(); // Stop previous track

            // Update index for next track
            currentClipIndex = (currentClipIndex + 1) % groanSounds.Count;
            isPlayingFirst = !isPlayingFirst; // Swap sources

            // Wait until the new clip is almost done before transitioning
            yield return new WaitForSeconds(fadingIn.clip.length - fadeDuration);
        }
    }

    private void PlayHurtSounds()
    {
        if (audioCoroutine != null)
        {
            StopCoroutine(audioCoroutine);
            audioCoroutine = null;
        }

        mainAudioSource.volume = 1;
        secondaryAudioSource.clip = null;
        mainAudioSource.clip = hurtSounds[Random.Range(0, hurtSounds.Count)];
        mainAudioSource.Play();
    }

    private void PlayDeadSounds()
    {
        if (audioCoroutine != null)
        {
            StopCoroutine(audioCoroutine);
            audioCoroutine = null;
        }

        mainAudioSource.volume = 1;
        secondaryAudioSource.clip = null;
        mainAudioSource.clip = deathSounds[Random.Range(0, deathSounds.Count)];
        mainAudioSource.Play();
    }

    private void PlayAttackSounds()
    {
        if (audioCoroutine != null)
        {
            StopCoroutine(audioCoroutine);
            audioCoroutine = null;
        }

        mainAudioSource.volume = 1;
        secondaryAudioSource.clip = null;
        mainAudioSource.clip = attackSounds[Random.Range(0, attackSounds.Count)];
        mainAudioSource.Play();
    }

    private void PlayChaseSounds()
    {
        // Stop crossfade coroutine
        if (audioCoroutine != null)
        {
            StopCoroutine(audioCoroutine);
            audioCoroutine = null;
        }

        isPlayingFirst = true;
        currentClipIndex = 0;

        // Set default properties
        mainAudioSource.volume = 1f;
        secondaryAudioSource.volume = 0f; // Second one starts silent

        audioCoroutine = StartCoroutine(PlayNextChaseClip());
    }

    IEnumerator PlayNextChaseClip()
    {
        while (true)
        {
            // Determine which audio source is fading in and out
            AudioSource fadingOut = isPlayingFirst ? mainAudioSource : secondaryAudioSource;
            AudioSource fadingIn = isPlayingFirst ? secondaryAudioSource : mainAudioSource;

            // Set new clip and play it
            fadingIn.clip = chaseSounds[currentClipIndex];
            fadingIn.Play();

            // Crossfade over fadeDuration
            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / fadeDuration;
                fadingOut.volume = Mathf.Lerp(1f, 0f, t);
                fadingIn.volume = Mathf.Lerp(0f, 1f, t);
                yield return null;
            }

            // Ensure volumes are set exactly at the end
            fadingOut.volume = 0f;
            fadingIn.volume = 1f;
            fadingOut.Stop(); // Stop previous track

            // Update index for next track
            currentClipIndex = (currentClipIndex + 1) % chaseSounds.Count;
            isPlayingFirst = !isPlayingFirst; // Swap sources

            // Wait until the new clip is almost done before transitioning
            yield return new WaitForSeconds(fadingIn.clip.length - fadeDuration);
        }
    }
}
