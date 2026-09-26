using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PhotosensitivityController : MonoBehaviour
{
    public GameObject warningPanel;

    void Update()
    {
        if (Input.anyKeyDown || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
        {
            DisableWarningPanel();
        }
    }

    void DisableWarningPanel()
    {
        if (warningPanel != null)
        {
            warningPanel.SetActive(false);
        }
    }
}