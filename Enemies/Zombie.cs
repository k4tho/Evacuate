using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Windows;
using UnityEngine.XR;
using static UnityEngine.GraphicsBuffer;

public class Zombie : Interactable, IPausable
{
    public GameObject player;
    private PlayerManager2 playerManager;
    private BoxCollider collider;
    private AudioSource mainAudioSource;
    private AudioSource secondaryAudioSource;
    private float hp;

    private float moveSpeed = 1.5f;

    private float hitDistance;          // Distance for player to take damage
    private float sightRadius;          // How far the player can see out
    private float peripheralAngle;
    private float chaseTimerMax;
    private float chaseTimer;

    private float attackTimer;          // Time remaining until next attack
    private float attackRecharge;
    private bool readyToAttack;

    // Prevents additional movement while executing death sequence
    private bool inDeathMode;
    private bool isAlerted;
    private bool isChasingPlayer;

    public AudioClip deathAudio;
    public List<AudioClip> deathSounds;
    public List<AudioClip> groanSounds;
    public List<AudioClip> attackSounds;
    public List<AudioClip> hitSounds;

    private bool isMovementPaused;

    // Dead zombie variables
    public GameObject inventory;

    protected void Awake()
    {
        playerManager = player.GetComponent<PlayerManager2>();
        collider = GetComponent<BoxCollider>();
    }

    private void OnDestroy()
    {
        if (PauseManager.Instance != null)
        {
            PauseManager.Instance.UnregisterPausable(this);
        }
    }

    void Start()
    {
        // Initialize interactable variables
        base.Start();

        // Audio
        mainAudioSource = gameObject.AddComponent<AudioSource>();
        secondaryAudioSource = gameObject.AddComponent<AudioSource>();

        mainAudioSource.spatialBlend = 1f;
        mainAudioSource.maxDistance = 10f;
        secondaryAudioSource.spatialBlend = 1f;
        secondaryAudioSource.maxDistance = 10f;

        hp = 100f;

        sightRadius = 10f;
        peripheralAngle = 75f;

        attackRecharge = 1.5f;
        hitDistance = 2.25f;

        chaseTimerMax = 3f;
        chaseTimer = 0f;

        inDeathMode = false;
        isAlerted = false;
        isChasingPlayer = false;

        isMovementPaused = false;

        PauseManager.Instance.RegisterPausable(this);

        PlayIdleSounds();

        interactMessage = "[F] Search Body";

        if (inventory != null)
        {
            inventory.gameObject.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        if (isMovementPaused) { return; }
        if (inDeathMode) { return; }

        RunAttackTimer();

        if (isAlerted)
        {
            Alert(); // Not yet implemented
        }
        else
        {
            BaseMovement();
        }
    }


    private void BaseMovement()
    {
        if (SeesPlayer())
        {
            if (!isChasingPlayer)
            {
                PlayAttackSounds();
            }

            isChasingPlayer = true;
            MoveTowardsPlayer();
            AttackPlayer();
        }
        else
        {
            // This is for directly after SeesPlayer() goes from true to false.
            // The zombie should chase the player a bit after leaving field of vision. If the player eludes the zombie long enough, it stops chasing.
            if (isChasingPlayer)
            {
                chaseTimer += Time.deltaTime;

                // If player has eluded zombie long enough, stop the chase
                if (chaseTimer >= chaseTimerMax)
                {
                    isChasingPlayer = false;
                    chaseTimer = 0f;
                    PlayIdleSounds();
                }
                else
                {
                    MoveTowardsPlayer();
                    AttackPlayer();
                }
            }
            else
            {
                chaseTimer = 0f;
                isChasingPlayer = false;

                //Debug.Log("No player");
                // Walk around the area doing zombie shit idk
            }
        }
    }

    // Can be alerted by throwing an item or player running etc. 
    private void Alert()
    {
        // At the very least, zombie will look in that general vincinity
        LookAtPlayer();
        // Probability that the zombie will go to that location
        // If probability is yes, go there
        // If not, turn to a random direction
    }

    #region // Movement

    private void MoveTowardsPlayer()
    {
        // Only move towards player if far away
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance > hitDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        }
        LookAtPlayer();
    }

    private void LookAtPlayer()
    {
        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0; // Keeps the object facing horizontally
        transform.rotation = Quaternion.LookRotation(direction);
    }

    private void MoveTowardsArea(Vector3 targetArea)
    {
        Vector3.MoveTowards(transform.position, targetArea, moveSpeed * Time.deltaTime);
        LookAtArea(targetArea);
    }

    private void LookAtArea(Vector3 targetArea)
    {
        Vector3 direction = targetArea - transform.position;
        direction.y = 0; // Keeps the object facing horizontally
        transform.rotation = Quaternion.LookRotation(direction);
    }

    private void TurnToRandomDirection()
    {

    }

    #endregion

    #region // Taking Damage
    public void TakeDamage(float hp)
    {
        // Sounds
        if (hitSounds.Count > 0)
        {
            mainAudioSource.volume = 1;
            mainAudioSource.clip = hitSounds[Random.Range(0, hitSounds.Count)];
            mainAudioSource.Play();
        }

        this.hp += hp;
        CheckHp();
    }

    private void CheckHp()
    {
        if (hp < 0)
        {
            Die();
        }
        // Lower speed at different thresholds?
    }

    private void Die()
    {
        inDeathMode = true;

        // Sounds
        mainAudioSource.volume = 1;
        mainAudioSource.clip = deathSounds[Random.Range(0, deathSounds.Count)];
        mainAudioSource.Play();

        // Destroy zombie and Animation after sound is complete
        // Invoke("DestroyGameObject", mainAudioSource.clip.length);

        this.transform.Rotate(new Vector3(-90, 0, 0)); // Make zombie lie down. REPLACE THIS WITH DEATH ANIMATION
        EnableInteractable();
    }

    private void DestroyGameObject()
    {
        Destroy(gameObject);
    }
    #endregion

    #region // Attack

    private bool SeesPlayer()
    {
        Vector3 directionToPlayer = new Vector3(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y + .75f, player.transform.position.z - transform.position.z).normalized;

        //Debug.Log("direction to player" + directionToPlayer);

        // Step 1: Check if the player is inside the vision cone (FOV check)
        float angleToFacePlayer = Vector3.Angle(transform.forward, directionToPlayer);
        if (peripheralAngle < angleToFacePlayer) return false; // Outside FOV
        if (-peripheralAngle > angleToFacePlayer) return false;

        // Step 2: Use OverlapSphere to detect players within view distance
        Collider[] visibleTargets = Physics.OverlapSphere(transform.position, sightRadius, 1 << player.layer);
        bool playerInRange = false;

        //Debug.Log("playerInRang1" + playerInRange); //false

        foreach (Collider target in visibleTargets)
        {
            if (target.gameObject == player.gameObject)
            {
                playerInRange = true;
                //Debug.Log("playerInRang2" + playerInRange); //true
                break;
            }
        }

        if (!playerInRange) return false; // Player is not within view range

        //Debug.Log("playerInRang3" + playerInRange); //true

        // Step 3: Raycast to ensure no obstacles block view
        int wallLayer = LayerMask.NameToLayer("Walls");
        if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, sightRadius, ~wallLayer))
        {
            if (hit.collider.gameObject == player.gameObject)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return playerInRange;
    }

    private void AttackPlayer()
    {
        if (readyToAttack)
        {
            if (IsTouchingPlayer())
            {
                playerManager.TakeDamage(9f);
                ResetAttack();
            }
        }
    }

    private bool IsTouchingPlayer()
    {
        float ySize = collider.bounds.size.y;
        Vector3 origin = new Vector3(transform.position.x, transform.position.y + (ySize / 2), transform.position.z);
        if (Physics.Raycast(origin, transform.forward, out RaycastHit hit, hitDistance))
        {
            if (hit.collider.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }

    private void RunAttackTimer()
    {
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
        else
        {
            readyToAttack = true;
        }
    }

    private void ResetAttack()
    {
        readyToAttack = false;
        attackTimer = attackRecharge;
    }
    #endregion

    #region // Sounds

    private bool isPlayingFirst = true;
    public float fadeDuration = 1.0f; // Time for crossfade
    private int currentClipIndex = 0;
    private Coroutine audioCoroutine;

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

    private void PlayAttackSounds()
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

        audioCoroutine = StartCoroutine(PlayNextAttackClip());
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

    IEnumerator PlayNextAttackClip()
    {
        while (true)
        {
            // Determine which audio source is fading in and out
            AudioSource fadingOut = isPlayingFirst ? mainAudioSource : secondaryAudioSource;
            AudioSource fadingIn = isPlayingFirst ? secondaryAudioSource : mainAudioSource;

            // Set new clip and play it
            fadingIn.clip = attackSounds[currentClipIndex];
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
            currentClipIndex = (currentClipIndex + 1) % attackSounds.Count;
            isPlayingFirst = !isPlayingFirst; // Swap sources

            // Wait until the new clip is almost done before transitioning
            yield return new WaitForSeconds(fadingIn.clip.length - fadeDuration);
        }
    }

    #endregion

    #region // Interactable (Dead Zombie)

    private void EnableInteractable()
    {
        gameObject.layer = LayerMask.NameToLayer("Interactable");
    }

    public override void Interact()
    {
        if (inventory != null)
        {
            player.GetComponent<PlayerController>().UpdateCommentary(player.GetComponent<PlayerInventory>().AddItem(inventory));
            inventory = null;
        }
        else
        {
            player.GetComponent<PlayerController>().UpdateCommentary("Nothing Found");
        }
    }

    #endregion

    public void OnPause()
    {
        isMovementPaused = true;
    }

    public void OnResume()
    {
        isMovementPaused = false;
    }
}
