using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Flashlight : Collectible
{
    private Light light;

    [SerializeField]
    public float batteryTimeInterval = 30f;      // How long a single battery lasts
    private float maxBatteryCount = 5f;          // Number of batteries
    private float currBatteryCount;              // How many batteries remain
    private float lightIntensityMax = 15f;       // Maximum brightness for the light
    private float rechargeDelay = 2f;            // How long it takes for a single battery to recharge
    private bool flashlightActive;

    public Slider batterySlider;     // Slider to show battery count
    public Image lightImage;         // UI icon to show when flashlight is on
    public GameObject flashlightObject; // Game object to enable and disable flashlight
    public AudioClip onAudio;
    public AudioClip offAudio;

    private Coroutine currCoroutine; // Keeps track of coroutine if using or recharging battery

    protected override void Awake()
    {
        base.Awake();
        light = GetComponent<Light>();
        SetFlashlightActive(false); // Disable flashlight at start
    }

    protected override void Start()
    {
        base.Start();
        itemName = "Flashlight";
        gameObject.tag = "Flashlight";

        light.intensity = 0f;
        flashlightActive = false;
        currCoroutine = null;

        currBatteryCount = maxBatteryCount;

        if (lightImage != null)
            lightImage.enabled = false; // Hide icon at start
    }

    public void UseFlashlight()
    {
        if (currCoroutine != null)
        {
            StopCoroutine(currCoroutine);
            currCoroutine = null;
        }

        if (!flashlightActive && currBatteryCount > 0)
        {
            audioSource.PlayOneShot(onAudio);
            currCoroutine = StartCoroutine(UseBattery());
        }
        else
        {
            audioSource.PlayOneShot(offAudio);
            currCoroutine = StartCoroutine(RechargeBattery());
        }
    }

    IEnumerator UseBattery()
    {
        light.enabled = true;
        light.intensity = lightIntensityMax;
        flashlightActive = true;

        if (lightImage != null)
            lightImage.enabled = true; // Show icon when flashlight is on

        while (currBatteryCount > 0)
        {
            yield return new WaitForSeconds(batteryTimeInterval);
            currBatteryCount--;
            batterySlider.value = (currBatteryCount / maxBatteryCount) * 100;
        }

        yield return StartCoroutine(FlickerEffect());

        flashlightActive = false;

        if (lightImage != null)
            lightImage.enabled = false; // Hide icon when flashlight dies

        currCoroutine = StartCoroutine(RechargeBattery());
    }

    IEnumerator RechargeBattery()
    {
        light.intensity = 0;
        flashlightActive = false;

        if (lightImage != null)
            lightImage.enabled = false; // Hide icon while charging

        while (currBatteryCount < maxBatteryCount)
        {
            yield return new WaitForSeconds(rechargeDelay);
            currBatteryCount++;
            batterySlider.value = (currBatteryCount / maxBatteryCount) * 100;
        }
    }

    IEnumerator FlickerEffect()
    {
        float flickerDuration = 1f;
        float flickerTimer = 0f;

        while (flickerTimer < flickerDuration)
        {
            bool state = !light.enabled;
            light.enabled = state;

            if (lightImage != null)
                lightImage.enabled = state;

            float delay = UnityEngine.Random.Range(0.05f, 0.2f);
            yield return new WaitForSeconds(delay);
            flickerTimer += delay;
        }

        light.enabled = false;

        if (lightImage != null)
            lightImage.enabled = false;
    }

    void UpdateLightIntensity()
    {
        light.intensity = lightIntensityMax * (currBatteryCount / maxBatteryCount);
    }

    public void SetFlashlightActive(bool active)
    {
        flashlightObject.SetActive(active);
        light.enabled = true;

        if (lightImage != null)
            lightImage.enabled = false; // Hide icon when picked up but off
    }
}
