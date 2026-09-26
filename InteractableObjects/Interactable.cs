using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public string interactMessage;

    private GameObject panelPrefab; // Reference to the panel prefab (assigned in Inspector)
    private Transform canvasTransform; // Reference to the Canvas transform where the panel will be instantiated

    private GameObject currentPanel;

    protected AudioSource audioSource;

    public UnityEvent onInteraction;

    protected virtual void Start()
    {
        canvasTransform = UiManager.Instance.canvasTransform;
        panelPrefab = UiManager.Instance.panelPrefab;

        interactMessage = "[E] To Interact";
    }

    public virtual void DisplayHoverMessage()
    {
        if (currentPanel != null)
        {
            Destroy(currentPanel);
        }

        // Instantiate the panel prefab
        currentPanel = Instantiate(panelPrefab, canvasTransform);

        // Get the Text component from the instantiated panel
        Text infoText = currentPanel.GetComponentInChildren<Text>();

        // Set the message on the Text component
        infoText.text = interactMessage;

        // Optionally, force the layout to update to resize the panel dynamically
        LayoutRebuilder.ForceRebuildLayoutImmediate(currentPanel.GetComponent<RectTransform>());

        RectTransform panelRectTransform = currentPanel.GetComponent<RectTransform>();
        panelRectTransform.anchoredPosition += new Vector2(0, -125f);

        // Activate the panel if it's inactive
        currentPanel.SetActive(true);
    }

    public virtual void RemoveHoverMessage()
    {
        if (currentPanel != null)
        {
            Destroy(currentPanel);
        }
    }

    public virtual void Interact()
    {
    }
}
