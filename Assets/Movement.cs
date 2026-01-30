using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 6f;

    [Header("Look Input")]
    [HideInInspector] public Vector2 lookInput; // Camera reads this

    private Rigidbody rb;
    private Vector2 moveInput;
    private Camera cam;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;

        if (cam == null)
            Debug.LogError("No camera found with tag MainCamera!");
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    public void ResetLookInput()
    {
        lookInput = Vector2.zero;
    }

    private void FixedUpdate()
    {
        if (rb == null || cam == null) return;

        // Camera-relative movement
        Vector3 forward = cam.transform.forward;
        Vector3 right = cam.transform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDir = forward * moveInput.y + right * moveInput.x;
        moveDir.Normalize();

        // Move player
        rb.linearVelocity = moveDir * speed;

        // Snap rotation: player always faces camera forward
        Vector3 cameraForward = cam.transform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();

        rb.rotation = Quaternion.LookRotation(cameraForward);
    }
}
