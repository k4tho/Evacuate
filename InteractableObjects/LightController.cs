using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LightController : MonoBehaviour
{
    [Header("UI: assign the Light Strobe slider from your Canvas (menu & in-game)")]
    public Slider strobeSlider;

    [Header("Player-controlled flicker strength (0…1)")]
    [Range(0f, 1f)]
    public float flickerStrength = 1f;

    [System.Serializable]
    public class FlickeringLight
    {
        public Light lightSource;
        public bool enableRepetitiveFlicker;
        public bool enableRandomFlicker;
        [Range(0f, 4f)]
        public float maxIntensity = 1f;
    }

    [Header("List all lights you want to flicker here")]
    public List<FlickeringLight> lights = new List<FlickeringLight>();

    void Awake()
    {
        // ✅ Load the last-saved value *before* any flicker starts
        flickerStrength = SettingsManager.StrobeStrength;
        Debug.Log($"[LightController] Loaded flickerStrength = {flickerStrength:F2}");
    }

    void Start()
    {
        // —— UI hookup ——
        if (strobeSlider != null)
        {
            strobeSlider.minValue     = 0f;
            strobeSlider.maxValue     = 1f;
            strobeSlider.wholeNumbers = false;

            // Listen for live changes
            strobeSlider.onValueChanged.AddListener(OnSliderChanged);

            // Move the thumb to the saved value *without* firing the callback
            strobeSlider.SetValueWithoutNotify(flickerStrength);
        }
        else
        {
            Debug.LogWarning("[LightController] No strobeSlider assigned in Inspector!");
        }

        // —— Start all flicker coroutines ——
        foreach (var f in lights)
        {
            if (f.enableRepetitiveFlicker)
            {
                f.maxIntensity = 1f;
                StartCoroutine(RepetitiveFlicker(f));
            }
            if (f.enableRandomFlicker)
            {
                f.maxIntensity = 4f;
                StartCoroutine(RandomFlicker(f));
            }
        }
    }

    void OnSliderChanged(float value)
    {
        // 1) Persist
        flickerStrength = Mathf.Clamp01(value);
        SettingsManager.StrobeStrength = flickerStrength;

        // 2) Immediately update your logs / any other UIs
        Debug.Log($"[LightController] slider → flickerStrength = {flickerStrength:F2}");
        // (no need to manually restart coroutines—they always read flickerStrength each pass)
    }

    IEnumerator RepetitiveFlicker(FlickeringLight f)
    {
        var light = f.lightSource;
        while (light != null)
        {
            float maxI = f.maxIntensity;
            float minI = Mathf.Lerp(maxI, 0f, flickerStrength);
            light.intensity = minI;
            yield return new WaitForSeconds(0.5f);

            light.intensity = maxI;
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator RandomFlicker(FlickeringLight f)
    {
        var light = f.lightSource;
        while (light != null)
        {
            float maxI    = f.maxIntensity;
            float minI    = Mathf.Lerp(maxI, 0f, flickerStrength);
            float randomI = Random.Range(minI, maxI);
            light.intensity = randomI;
            yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
        }
    }
}
