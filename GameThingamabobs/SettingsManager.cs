using UnityEngine;

/// <summary>
/// Centralized access to all player‐configurable settings, persisted via PlayerPrefs.
/// Add new settings here as needed.
/// </summary>
public static class SettingsManager
{
    // ─── PlayerPrefs keys ────────────────────────────────────────────────
    private const string ResolutionKey  = "ResolutionIndex";
    private const string FullscreenKey  = "Fullscreen";
    private const string QualityKey     = "QualityIndex";
    private const string BrightnessKey  = "Brightness";
    private const string StrobeKey      = "LightStrobeStrength";

    // ─── Resolution ─────────────────────────────────────────────────────
    /// <summary>
    /// Index into Screen.resolutions[].  
    /// Defaults to 0 (first available resolution).
    /// </summary>
    public static int ResolutionIndex
    {
        get => PlayerPrefs.GetInt(ResolutionKey, 0);
        set
        {
            PlayerPrefs.SetInt(ResolutionKey, value);
            PlayerPrefs.Save();
        }
    }

    // ─── Fullscreen ─────────────────────────────────────────────────────
    /// <summary>
    /// true = fullscreen, false = windowed.  
    /// Defaults to whatever Screen.fullScreen currently is.
    /// </summary>
    public static bool Fullscreen
    {
        get => PlayerPrefs.GetInt(FullscreenKey, Screen.fullScreen ? 1 : 0) == 1;
        set
        {
            PlayerPrefs.SetInt(FullscreenKey, value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    // ─── Quality ────────────────────────────────────────────────────────
    /// <summary>
    /// Index into QualitySettings.names.  
    /// Defaults to QualitySettings.GetQualityLevel().
    /// </summary>
    public static int QualityIndex
    {
        get => PlayerPrefs.GetInt(QualityKey, QualitySettings.GetQualityLevel());
        set
        {
            PlayerPrefs.SetInt(QualityKey, value);
            PlayerPrefs.Save();
        }
    }

    // ─── Brightness ─────────────────────────────────────────────────────
    /// <summary>
    /// UI / post‐process brightness scalar [0…1].  
    /// Defaults to 1.
    /// </summary>
    public static float Brightness
    {
        get => PlayerPrefs.GetFloat(BrightnessKey, 1f);
        set
        {
            PlayerPrefs.SetFloat(BrightnessKey, Mathf.Clamp01(value));
            PlayerPrefs.Save();
        }
    }

    // ─── Strobe Strength ─────────────────────────────────────────────────
    /// <summary>
    /// Flicker intensity [0…1] for lights.  
    /// Defaults to 1.
    /// </summary>
    public static float StrobeStrength
    {
        get => PlayerPrefs.GetFloat(StrobeKey, 1f);
        set
        {
            PlayerPrefs.SetFloat(StrobeKey, Mathf.Clamp01(value));
            PlayerPrefs.Save();
        }
    }
}
