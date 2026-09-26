using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Commentary : MonoBehaviour
{
    private Text text;
    private CanvasGroup canvasGroup;

    private float fadeDuration = .75f;


    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        text = GetComponentInChildren<Text>();
        text.text = "";
    }

    public void DisplayCommentary(string comment)
    {
        text.text = comment;
        canvasGroup.alpha = 1f;
        StopAllCoroutines();
        StartCoroutine(FadeToTransparency());
    }

    IEnumerator FadeToTransparency()
    {
        yield return new WaitForSeconds(2f);  // Wait for 2 seconds before starting to fade out

        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            yield return null;  // Wait until the next frame
        }

        // Ensure panel is fully transparent at the end
        canvasGroup.alpha = 0f;
        text.text = "";
    }
}
