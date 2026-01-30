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

        // ── Camera-relative movement ────────────────
        // Get camera's forward and right, project onto XZ plane (y=0)
        Vector3 camForward = cam.transform.forward;
        Vector3 camRight   = cam.transform.right;

        camForward.y = 0f;
        camRight.y   = 0f;

        camForward.Normalize();
        camRight.Normalize();

        // Now build move direction:
        // moveInput.y → forward/back (W/S) along camera's look direction (now X-ish)
        // moveInput.x → left/right (A/D) along camera's right (now Z-ish)
        Vector3 moveDir = (camForward * moveInput.y) + (camRight * moveInput.x);

        if (moveDir.sqrMagnitude > 0.001f)
        {
            moveDir.Normalize();
            rb.linearVelocity = moveDir * speed;
        }
        else
        {
            // Optional: stop horizontal movement when no input (keeps gravity/falling clean)
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        }

        // ── Snap player to face camera's horizontal forward direction ──
        // (player model usually faces +Z, but LookRotation will orient correctly)
        Vector3 lookDirection = camForward; // already flattened & normalized
        if (lookDirection.sqrMagnitude > 0.001f)
        {
            rb.rotation = Quaternion.LookRotation(lookDirection) * Quaternion.Euler(0, -90f, 0);
        }
    }
}