using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("References")]
    public Transform target;                // Assign Player capsule
    public PlayerMovement player;           // Assign PlayerMovement script

    [Header("Camera Settings")]
    public Vector3 offset = new Vector3(0, 2, -5);
    public float mouseSensitivity = 2f;
    public float rotationSmoothTime = 0.12f;

    private float yaw;
    private float pitch;
    private Vector3 currentRotation;
    private Vector3 rotationVelocity;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (target == null || player == null) return;

        // Read input from PlayerMovement
        Vector2 lookInput = player.lookInput;

        // Update rotation
        yaw += lookInput.x * mouseSensitivity;
        pitch -= lookInput.y * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, -35f, 60f);

        Vector3 targetRotation = new Vector3(pitch, yaw);
        currentRotation = Vector3.SmoothDamp(currentRotation, targetRotation, ref rotationVelocity, rotationSmoothTime);
        transform.eulerAngles = currentRotation;

        // Follow the player
        transform.position = target.position + transform.rotation * offset;
    }
}
