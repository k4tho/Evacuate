using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLevel : MonoBehaviour
{
    public GameObject player;
    private PlayerController playerController;

    public GameObject tutorialZombie;

    public AudioClip introAudio;
    public AudioClip exitAudio;
    public AudioClip findKnifeAudio;
    public AudioClip alrFoundKnifeAudio;
    public AudioClip findHealthKitAudio;
    public AudioClip alrFoundHealthKitAudio;
    public AudioClip searchEnemyAudio;
    public AudioClip alrFoundSearchAudio;
    public AudioClip firstPuzzleAudio;
    public AudioClip secondPuzzlePart1Audio;
    public AudioClip secondPuzzlePart2Audio;
    public AudioClip thirdPuzzleAudio;

    private AudioSource audioSource;

    private bool firstPuzzleIntroduced;
    private bool secondPuzzlePart1Introduced;
    private bool secondPuzzlePart2Introduced;
    private bool thirdPuzzleIntroduced;

    public enum TutorialState { Idk, Greeting, Exit, FirstPuzzle, SecondPuzzlePart1, SecondPuzzlePart2, ThirdPuzzle, FindWeapon, FindHealthKit, SearchEnemy, Done }
    private TutorialState currState;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        // Get player stats
        playerController = player.GetComponent<PlayerController>();
    }

    private void Start()
    {
        // Prevent player movement at the start of the game
        playerController.UpdateMovementPaused(true);

        // Set curr state to null to transition into the first
        currState = TutorialState.Idk;

        firstPuzzleIntroduced = false;
        secondPuzzlePart1Introduced = false;
        secondPuzzlePart2Introduced = false;
        thirdPuzzleIntroduced = false;
    }

    private void Update()
    {
        if (currState == TutorialState.Idk)
        {
            // Player wakes up unable to move until the greeting pa ends
            currState = TutorialState.Greeting;
            StartCoroutine(PlayAudioClipWithDelay(10f, introAudio));
            StartCoroutine(WaitToBeginGameplay());
        }
        else if (currState == TutorialState.Greeting)
        {
            // Check if the first pa has ended AND if the player has moved, then move onto the next
            if (!audioSource.isPlaying && playerController.IsMoving())
            {
                currState = TutorialState.Exit;
                StartCoroutine(PlayAudioClipWithDelay(2f, exitAudio));
            }
        }
        else if (currState == TutorialState.FirstPuzzle && !firstPuzzleIntroduced)
        {
            firstPuzzleIntroduced = true;
            PlayNewAudioClip(firstPuzzleAudio);
        }
        else if (currState == TutorialState.SecondPuzzlePart1 && !secondPuzzlePart1Introduced)
        {
            secondPuzzlePart1Introduced = true;
            PlayNewAudioClip(secondPuzzlePart1Audio);
        }
        else if (currState == TutorialState.SecondPuzzlePart2 && !secondPuzzlePart2Introduced)
        {
            secondPuzzlePart2Introduced = true;
            PlayNewAudioClip(secondPuzzlePart2Audio);
        }
        else if (currState == TutorialState.ThirdPuzzle && !thirdPuzzleIntroduced)
        {
            thirdPuzzleIntroduced = true;
            PlayNewAudioClip(thirdPuzzleAudio);
        }
        /*
        else if (currState == TutorialState.Exit)
        {
            // Checks if player has been hurt by the zombie AND IF THE PLAYER HAS A FLASHLIGHT WITH WEAPON
            if (playerManager.GetPlayerHealth() < 100)
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
            if (playerManager.GetPlayerHealth() == 100)
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
        else if (currState != TutorialState.Done) { 
        }
        */
    }

    public void UpdateState(TutorialState state)
    {
        currState = state;
    }

    public TutorialState GetCurrentState()
    {
        return currState;
    }


    private void PlayNewAudioClip(AudioClip newClip)
    {
        if (audioSource.clip != null)
        {
            audioSource.Stop();
        }

        audioSource.clip = newClip;
        audioSource.Play();

        // When the clip is done, then the player is allowed to move
    }

    IEnumerator PlayAudioClipWithDelay(float delay, AudioClip newClip)
    {
        yield return new WaitForSeconds(delay);
        PlayNewAudioClip(newClip);
    }

    IEnumerator WaitToBeginGameplay()
    {
        yield return new WaitForSeconds(20f);
        playerController.UpdateMovementPaused(false);
    }
}
