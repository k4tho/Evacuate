using System.Collections;
using System.Collections.Generic;
using UHFPS.Runtime;
using UnityEngine;

public class TutorialLevel2 : MonoBehaviour
{
    public GameObject player;
    public AudioSource paSystem;

    private CharacterController characterController;
    private BaseHealthEntity playerHealth;

    public GameObject tutorialZombie;

    public AudioClip introAudio;
    public AudioClip exitAudio;
    public AudioClip findKnifeAudio;
    public AudioClip alrFoundKnifeAudio;
    public AudioClip findHealthKitAudio;
    public AudioClip alrFoundHealthKitAudio;
    public AudioClip searchEnemyAudio;
    public AudioClip alrFoundSearchAudio;

    enum TutorialState { Idk, Greeting, Exit, FindWeapon, FindHealthKit, SearchEnemy, Done }
    private TutorialState currState;

    private void Awake()
    {
        // Get player stats
        characterController = player.GetComponent<CharacterController>();
        playerHealth = player.GetComponent<BaseHealthEntity>();
    }

    private void Start()
    {
        // Prevent player movement at the start of the game
        characterController.enabled = false;

        // Set curr state to null to transition into the first
        currState = TutorialState.Idk;
    }

    private void Update()
    {
        if (currState == TutorialState.Idk)
        {
            // Player wakes up unable to move until the greeting pa ends
            currState = TutorialState.Greeting;
            StartCoroutine(PlayAudioClipWithDelay(2f, introAudio));
            StartCoroutine(WaitToBeginGameplay());
        }
        else if (currState == TutorialState.Greeting)
        {
            // Check if the first pa has ended AND if the player has moved, then move onto the next
            if (!paSystem.isPlaying && characterController.velocity != Vector3.zero)
            {
                currState = TutorialState.Exit;
                StartCoroutine(PlayAudioClipWithDelay(2f, exitAudio));
            }
        }
        else if (currState == TutorialState.Exit)
        {
            // Checks if player has been hurt by the zombie AND IF THE PLAYER HAS A FLASHLIGHT WITH WEAPON
            if (playerHealth.EntityHealth < playerHealth.MaxEntityHealth)
            {
                currState = TutorialState.FindWeapon;
                StartCoroutine(PlayAudioClipWithDelay(4f, findKnifeAudio));
            }
        }
        else if (currState == TutorialState.FindWeapon)
        {
            // Checks if zombie is dead
            if (tutorialZombie == null)
            {
                currState = TutorialState.FindHealthKit;
                PlayNewAudioClip(findHealthKitAudio);
            }
        }
        else if (currState == TutorialState.FindHealthKit)
        {
            // Check if player healed themselves
            if (playerHealth.EntityHealth == playerHealth.MaxEntityHealth)
            {
                currState = TutorialState.SearchEnemy;
                StartCoroutine(PlayAudioClipWithDelay(2f, searchEnemyAudio));
            }
        }
        else if (currState == TutorialState.SearchEnemy)
        {
            // Check if player collected gun from zombie
            List<GameObject> inventory = new List<GameObject>();
            foreach (GameObject item in inventory)
            {
                if (item.tag == "Pistol")
                {
                    currState = TutorialState.Done;
                }
            }
        }
        else if (currState != TutorialState.Done)
        {
        }
    }



    private void PlayNewAudioClip(AudioClip newClip)
    {
        if (paSystem.clip != null)
        {
            paSystem.Stop();
        }

        paSystem.clip = newClip;
        paSystem.Play();

        // When the clip is done, then the player is allowed to move
    }

    IEnumerator PlayAudioClipWithDelay(float delay, AudioClip newClip)
    {
        yield return new WaitForSeconds(delay);
        PlayNewAudioClip(newClip);
    }

    IEnumerator WaitToBeginGameplay()
    {
        yield return new WaitForSeconds(introAudio.length);
        characterController.enabled = true;
    }
}
