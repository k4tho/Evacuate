using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [Header("Background Setup")]
    public RectTransform background; // Assign the RectTransform of your background

    [Header("Effect Settings")]
    public float movementAmount = 25f; // Controls how much the background moves
    public float zoomFactor = 1.01f; // Slightly zoom in (e.g., 1.01 = 1% larger)
    public float smoothSpeed = 0.1f; // Controls the smoothness of the movement

    private Vector2 originalSize;

    private ScreenSizeManager screenSizeManager;
    private ParallaxEffectManager parallaxEffectManager;

    void Start()
    {
        // Store original size to debug zoom behavior
        originalSize = background.sizeDelta;

        screenSizeManager = new ScreenSizeManager(background, zoomFactor);
        parallaxEffectManager = new ParallaxEffectManager(background, movementAmount, smoothSpeed);

        screenSizeManager.AdjustBackgroundSize();
    }

    void Update()
    {
        parallaxEffectManager.ApplyParallaxEffect();
    }
}

public class ScreenSizeManager
{
    private RectTransform background;
    private float zoomFactor;

    public ScreenSizeManager(RectTransform background, float zoomFactor)
    {
        this.background = background;
        this.zoomFactor = zoomFactor;
    }

    public void AdjustBackgroundSize()
    {
        // Get screen size
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Get the original aspect ratio of the background image
        float imageAspect = background.rect.width / background.rect.height;

        // Determine whether to scale by width or height to cover the screen
        if (screenWidth / screenHeight > imageAspect)
        {
            // Screen is wider; scale by width
            float newHeight = screenWidth / imageAspect;
            background.sizeDelta = new Vector2(screenWidth * zoomFactor, newHeight * zoomFactor);
        }
        else
        {
            // Screen is taller; scale by height
            float newWidth = screenHeight * imageAspect;
            background.sizeDelta = new Vector2(newWidth * zoomFactor, screenHeight * zoomFactor);
        }

        // Debug the calculated size
        Debug.Log($"Adjusted Background Size: {background.sizeDelta}");
        Debug.Log($"Zoom Factor: {zoomFactor}");
    }
}

public class ParallaxEffectManager
{
    private RectTransform background;
    private float movementAmount;
    private float smoothSpeed;
    private Vector2 targetPosition;
    private Vector2 currentVelocity;

    public ParallaxEffectManager(RectTransform background, float movementAmount, float smoothSpeed)
    {
        this.background = background;
        this.movementAmount = movementAmount;
        this.smoothSpeed = smoothSpeed;
        targetPosition = background.anchoredPosition;
        currentVelocity = Vector2.zero;
    }

    public void ApplyParallaxEffect()
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Vector2 offset = (mousePosition - screenCenter) / screenCenter;

        // Calculate the target position based on mouse offset
        targetPosition = offset * movementAmount;

        // Smoothly move the background towards the target position
        background.anchoredPosition = Vector2.SmoothDamp(background.anchoredPosition, targetPosition, ref currentVelocity, smoothSpeed);
    }
}