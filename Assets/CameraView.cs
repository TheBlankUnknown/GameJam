using UnityEngine;
using UnityEngine.EventSystems;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("References")]
    public Transform target;       // Assign Player capsule
    public PlayerMovement player;  // Assign PlayerMovement script

    [Header("Camera Settings")]
    public Vector3 offset = new Vector3(5f, 2f, 0f);  // ← now offset along +X, looking toward -X
    public float mouseSensitivity = 2f;
    public float rotationSmoothTime = 0.12f;

    private float yaw;    // horizontal rotation (around world Y)
    private float pitch;  // vertical rotation (around local right / world Z-ish)
    private Vector3 currentRotation;
    private Vector3 rotationVelocity;

    [HideInInspector]
    public bool blockLookInput;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Optional: initialize looking straight along -X
        yaw = 180f;   // so camera looks toward -X by default (adjust as needed)
        pitch = 10f;  // slight downward angle — feel free to change
    }

    void LateUpdate()
    {
        if (target == null || player == null)
            return;

        // 🔑 BLOCK look input when UI just closed or pointer over UI
        Vector2 lookInput = (blockLookInput || EventSystem.current.IsPointerOverGameObject())
            ? Vector2.zero
            : player.lookInput;

        // Update rotation angles
        yaw   += lookInput.x * mouseSensitivity;
        pitch -= lookInput.y * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, -35f, 60f);

        Vector3 targetRotation = new Vector3(pitch, yaw, 0f);

        currentRotation = Vector3.SmoothDamp(
            currentRotation,
            targetRotation,
            ref rotationVelocity,
            rotationSmoothTime
        );

        // Apply rotation — note: order matters (pitch then yaw is common)
        transform.eulerAngles = currentRotation;

        // Position camera relative to target using the camera's current orientation
        // This now rotates the offset vector so +X becomes the "forward" direction
        transform.position = target.position + transform.rotation * offset;
    }
}