using UnityEngine;

public class SettingsMenuManager : MonoBehaviour
{
    public GameObject settingsMenu;
    public GameObject pauseMenu;
    
    public GameObject videoPanel;
    public GameObject audioPanel;
    public GameObject controlsPanel;
    
    
    
    
    
    public void OnClickOpenSettings()
    {
        settingsMenu.SetActive(true);
        
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }
    }
    
    public void OnClickCloseSettings()
    {
        settingsMenu.SetActive(false);
        
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(true);
        }
    }
    
    
    public void OnClickVideo()
    {
        videoPanel.SetActive(true);
        audioPanel.SetActive(false);
        controlsPanel.SetActive(false);
    }

    public void OnClickAudio()
    {
        videoPanel.SetActive(false);
        audioPanel.SetActive(true);
        controlsPanel.SetActive(false);
    }

    public void OnClickControls()
    {
        videoPanel.SetActive(false);
        audioPanel.SetActive(false);
        controlsPanel.SetActive(true);
    }
}