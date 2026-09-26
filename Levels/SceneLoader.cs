using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    public void OnClickLoadScene(int sceneIndex)
    {
        Time.timeScale = 1f;
        
        SceneManager.LoadScene(sceneIndex);

        // main menu is currently index 0
        if (sceneIndex == 0) 
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            // If loading the game scene, lock the cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void OnClickQuit()
    {
        Application.Quit();
        Debug.Log("Quit.");
    }
}