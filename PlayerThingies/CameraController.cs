using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float fastSpeed = 15f;
    public float lookSpeed = 2f;
    public float arrowLookSpeed = 15f; // for arrow key look
    public bool enableLook = true;

    private float yaw = 0f;
    private float pitch = 0f;

    void Update()
    {
        float speed = Input.GetKey(KeyCode.LeftShift) ? fastSpeed : moveSpeed;

        // Movement
        Vector3 move = new Vector3(
            Input.GetAxis("Horizontal"),                                // A/D
            (Input.GetKey(KeyCode.E) ? 1 : 0) - (Input.GetKey(KeyCode.Q) ? 1 : 0), // E/Q
            Input.GetAxis("Vertical")                                   // W/S
        );
        transform.Translate(move * speed * Time.deltaTime, Space.Self);

        // Mouse look (right-click)
        if (enableLook && Input.GetMouseButton(1))
        {
            yaw += Input.GetAxis("Mouse X") * lookSpeed;
            pitch -= Input.GetAxis("Mouse Y") * lookSpeed;
        }

        // Arrow key look (optional alternative or supplement)
        if (Input.GetKey(KeyCode.LeftArrow)) yaw -= arrowLookSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.RightArrow)) yaw += arrowLookSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.UpArrow)) pitch -= arrowLookSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.DownArrow)) pitch += arrowLookSpeed * Time.deltaTime;

        pitch = Mathf.Clamp(pitch, -90f, 90f);
        transform.eulerAngles = new Vector3(pitch, yaw, 0f);
    }
}
