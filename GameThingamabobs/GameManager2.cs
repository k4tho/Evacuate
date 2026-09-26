using UHFPS.Runtime;
using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class GameManager2 : MonoBehaviour
{
    public enum PanelType { None, NotePanel, PausePanel, MainPanel, GamePanel, DeadPanel}
    public CanvasGroup PausePanel;
    public CanvasGroup GamePanel;
    public CanvasGroup DeadPanel;
    public ControlsInfoPanel ControlsInfoPanel;
    
    public GameObject ToggleableUI;
    public GameObject Crosshair;

    public PauseManager pauseManager;
    public NoteUiManager noteUiManager;
    private PanelType currPanel;
    private PanelType prevPanel;

    public PlayerController playerController;

    public BackgroundFader BackgroundFade;

    private bool isPointerShown;

    private int pointerCullLayers;
    private Layer pointerInteractLayer;
    private Action<RaycastHit, IInteractStart> pointerInteractAction;
    public Image PointerImage;

    PlayerInput playerInput;
    PlayerInput.UiActions input;

    private bool isCursorVisiblePriorToPause;
    public bool isInputBlocked;
    public bool isMovementPausedPrior;

    void Start()
    {
        if (BackgroundFade != null)
            BackgroundFade.gameObject.SetActive(true);
        
        DisableAllGamePanels();

        playerInput = new PlayerInput();
        playerInput.Ui.Enable();
        input = playerInput.Ui;

        currPanel = PanelType.MainPanel;
        prevPanel = PanelType.None;
        
        var intro = FindFirstObjectByType<IntroCutScene>();
        if (intro != null)
            intro.OnIntroFinished += OnIntroComplete;

        isCursorVisiblePriorToPause = false;
        isInputBlocked = false;
        isMovementPausedPrior = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            ToggleUI();
        }
        else if (input.Escape.WasPressedThisFrame())
        {
            pauseManager.EscapePressed();
            //ShowPanel(PanelType.PausePanel);
        }
        else if (input.OpenNotes.WasPressedThisFrame() && (currPanel==PanelType.MainPanel))
        {
            ShowPanel(PanelType.NotePanel);
        }
    }

    public void ShowPanel(PanelType panel)
    {
        switch (panel)
        {
            case PanelType.None:
                DisableAllGamePanels(); 
                break;
            case PanelType.NotePanel:
                if (currPanel == PanelType.MainPanel)
                {
                    isMovementPausedPrior = playerController.IsMovementPaused();
                    isCursorVisiblePriorToPause = Cursor.visible;
                }

                playerController.UpdateMovementPaused(true);

                prevPanel = currPanel;
                currPanel = panel;

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                noteUiManager.gameObject.SetActive(true);
                GamePanel.alpha = 0;
                DeadPanel.alpha = 0;
                PausePanel.gameObject.SetActive(false);

                playerController.UpdateMovementPaused(true);
                noteUiManager.OpenNoteSelection();
                break;
            case PanelType.PausePanel:
                if (currPanel == PanelType.MainPanel)
                {
                    isMovementPausedPrior = playerController.IsMovementPaused();
                    isCursorVisiblePriorToPause = Cursor.visible;
                }

                prevPanel = currPanel;
                currPanel = panel;

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                PausePanel.gameObject.SetActive(true);
                noteUiManager.gameObject.SetActive(false);
                DeadPanel.alpha = 0;
                break;

            case PanelType.MainPanel:
                // If going back to main panel after pausing, check the previous state
                if ((currPanel == PanelType.PausePanel) && (prevPanel != PanelType.MainPanel))
                {
                    ShowPanel(prevPanel);
                    break;
                }
                if (isCursorVisiblePriorToPause)
                {

                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }

                isInputBlocked = true;
                StartCoroutine(UnblockPuzzleInput());

                prevPanel = currPanel;
                currPanel = panel;

                GamePanel.alpha = 1;
                PausePanel.gameObject.SetActive(false);
                noteUiManager.gameObject.SetActive(false);
                DeadPanel.alpha = 0;
                Debug.Log(isMovementPausedPrior);
                playerController.UpdateMovementPaused(isMovementPausedPrior);
                break;

            case PanelType.DeadPanel:
                prevPanel = currPanel;
                currPanel = panel;

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                GamePanel.alpha = 0;
                PausePanel.gameObject.SetActive(false);
                DeadPanel.alpha = 1;
                break;

        }
    }
    
    private void ToggleUI()
    {
        if (ToggleableUI == null) return;
        bool isOn = ToggleableUI.activeSelf;
        ToggleableUI.SetActive(!isOn);

        // Optionally, lock/unlock cursor when you open this panel:
        if (!isOn)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            // Pause your game if you want:
            playerController.UpdateMovementPaused(true);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            playerController.UpdateMovementPaused(false);
        }
    }

    public void DisableAllGamePanels()
    {
        DeadPanel.alpha = 0;
        PausePanel.gameObject.SetActive(false);
        //HUDPanel.alpha = 0;
        //TabPanel.alpha = 0;
        //InventoryPanel.alpha = 0;
        //IsInventoryShown = false;
    }

    public void ShowControlsInfo(bool show, params ControlsContext[] contexts)
    {
        if (show)
        {
            ControlsInfoPanel.ShowInfo(contexts);
        }
        else
        {
            ControlsInfoPanel.HideInfo();
        }
    }

    public IEnumerator StartBackgroundFade(bool fadeOut, float waitTime = 0, float fadeSpeed = 3)
            => BackgroundFade.StartBackgroundFade(fadeOut, waitTime, fadeSpeed);

    public void ShowPointer(int cullLayers, Layer interactLayer, Action<RaycastHit, IInteractStart> interactAction)
    {
        isPointerShown = true;
        pointerCullLayers = cullLayers;
        pointerInteractLayer = interactLayer;
        pointerInteractAction = interactAction;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        //PointerImage.gameObject.SetActive(true);
    }

    public void HidePointer()
    {
        isPointerShown = false;
        pointerInteractAction = null;
        pointerCullLayers = -1;
        pointerInteractLayer = -1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //PointerImage.gameObject.SetActive(false);
        //PointerImage.rectTransform.anchoredPosition = Vector2.zero;
    }
    
    private void OnIntroComplete()
    {
        if (ToggleableUI != null)
            ToggleableUI.SetActive(true);
        
        if (Crosshair != null)
            Crosshair.SetActive(true);
    }

    private void PauseGame()
    {
        playerController.UpdateMovementPaused(true);
    }

    IEnumerator UnblockPuzzleInput()
    {
        yield return new WaitForSeconds(.1f);
        isInputBlocked = false;
    }

    private void PlayGame()
    {

    }
}
