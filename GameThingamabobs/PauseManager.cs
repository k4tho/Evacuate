using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject quitGameUI; 
    public GameObject returnToMenuUI;
    public GameObject settingsMenuUI;

    private bool isPaused = false;
    private bool playerPausedMovement;              // Saves player's movement state prior to pausing

    public static PauseManager Instance { get; private set; }
    private List<IPausable> pausableObjects = new List<IPausable>();
    private static List<AudioSource> activeAudioSources = new List<AudioSource>();

    private GameManager2 gameManager;
    
    public SettingsMenuManager settingsMenuManager;
    public PlayerController playerController;

    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager2>();

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Multiple PauseManager instances detected!");
        }
    }

    private void Start()
    {
        LockAndHideCursor();
        Time.timeScale = 1f;
    }

    public void EscapePressed()
    {
        if (settingsMenuUI.activeSelf)
        {
            settingsMenuManager.OnClickCloseSettings();
        }
        else if (quitGameUI.activeSelf)
        {
            CloseQuitConfirmation();
        }
        else if (returnToMenuUI.activeSelf)
        {
            CloseReturnToMenuConfirmation();
        }
        else if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        gameManager.ShowPanel(GameManager2.PanelType.PausePanel);
        playerPausedMovement = playerController.IsMovementPaused();
        playerController.UpdateMovementPaused(true);
        EnablePauseMenu();
        FreezeGameTime();
        PauseAllAudio();
    }

    public void ResumeGame()
    {
        gameManager.ShowPanel(GameManager2.PanelType.MainPanel);
        playerController.UpdateMovementPaused(playerPausedMovement);
        DisablePauseMenu();
        UnfreezeGameTime();
        ResumeAllAudio();
    }

    #region // Handle pause menu
    public void OnClickResume()
    {
        ResumeGame();
    }

    public void OnClickMainMenu()
    {
        EnableReturnToMenuConfirmation();
    }

    public void OnClickCancelMainMenu()
    {
        CloseReturnToMenuConfirmation();
    }

    public void OnClickQuit()
    {
        EnableQuitConfirmation();
    }

    public void OnClickConfirmQuit()
    {
        Application.Quit();
        Debug.Log("Quit.");
    }

    public void OnClickCancelQuit()
    {
        CloseQuitConfirmation();
    }

    private void FreezeGameTime()
    {
        Time.timeScale = 0f;
        isPaused = true;
    }

    private void UnfreezeGameTime()
    {
        Time.timeScale = 1f;
        isPaused = false;
    }

    private void EnablePauseMenu()
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(true);
    }

    private void PauseAllAudio()
    {
        activeAudioSources.Clear();
        AudioSource[] allSources = FindObjectsOfType<AudioSource>();

        foreach (AudioSource source in allSources)
        {
            if (source.isPlaying)
            {
                activeAudioSources.Add(source);
                source.Pause();
            }
        }
    }

    private void ResumeAllAudio()
    {
        foreach (AudioSource source in activeAudioSources)
        {
            if (source != null)
                source.UnPause();
        }
        activeAudioSources.Clear();
    }

    private void DisablePauseMenu()
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);
    }

    private void EnableQuitConfirmation()
    {
        if (quitGameUI != null)
            quitGameUI.SetActive(true);
    }

    private void CloseQuitConfirmation()
    {
        if (quitGameUI != null)
            quitGameUI.SetActive(false);
    }

    private void EnableReturnToMenuConfirmation()
    {
        if (returnToMenuUI != null)
            returnToMenuUI.SetActive(true);
    }

    private void CloseReturnToMenuConfirmation()
    {
        if (returnToMenuUI != null)
            returnToMenuUI.SetActive(false);
    }

    private void UnlockAndShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void LockAndHideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    #endregion


    #region // Keeping track of pausable objects
    public void RegisterPausable(IPausable pausable)
    {
        if (!pausableObjects.Contains(pausable))
        {
            pausableObjects.Add(pausable);
        }
    }

    public void UnregisterPausable(IPausable pausable)
    {
        pausableObjects.Remove(pausable);
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        foreach (IPausable pausable in pausableObjects)
        {
            if (isPaused)
                pausable.OnPause();
            else
                pausable.OnResume();
        }
    }
    #endregion
}
