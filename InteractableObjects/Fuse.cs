using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class Fuse : Collectible
{
    private Light light;
    private MeshRenderer lightRenderer;

    public Transform parent;

    protected override void Awake()
    {
        base.Awake();

        light = GetComponent<Light>();
        lightRenderer = GetComponent<MeshRenderer>();
        
    }

    protected override void Start()
    {
        base.Start();

        itemName = "Fuse";
        gameObject.tag = "Fuse";
        lightRenderer.material.DisableKeyword("_EMISSION");

        // Physics
        posHeld = new Vector3(0.025f, 0.124f, -0.038f);
        rotHeld = new Vector3(-27.068f, 75f, -55.915f);
        scaleHeld = new Vector3(2f, 2f, 2f);
        posDropped = new Vector3(0f, 0f, 0f);
    }
}