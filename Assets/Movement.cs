using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 6f;

    [Header("Look Input")]
    [HideInInspector] public Vector2 lookInput; // Camera reads this

    [Header("Visuals")]
    public SimpleWalkBob walkBob;

    private Rigidbody rb;
    private Vector2 moveInput;
    private Camera cam;

    // 🔑 added only to support walk bob
    private bool isMoving;

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

    // ✅ KEPT — unchanged
    public void ResetLookInput()
    {
        lookInput = Vector2.zero;
    }

    private void FixedUpdate()
    {
        if (rb == null || cam == null) return;

        // ── Camera-relative movement (UNCHANGED) ──
        Vector3 camForward = cam.transform.forward;
        Vector3 camRight   = cam.transform.right;

        camForward.y = 0f;
        camRight.y   = 0f;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = camForward * moveInput.y + camRight * moveInput.x;

        if (moveDir.sqrMagnitude > 0.001f)
        {
            isMoving = true;
            moveDir.Normalize();
            rb.linearVelocity = moveDir * speed;
        }
        else
        {
            isMoving = false;
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        }

        // ── Rotation (UNCHANGED) ──
        Vector3 lookDirection = camForward;
        if (lookDirection.sqrMagnitude > 0.001f)
        {
            rb.rotation =
                Quaternion.LookRotation(lookDirection)
                * Quaternion.Euler(0, -90f, 0);
        }
    }

    private void Update()
    {
        // ✅ ONLY NEW BEHAVIOR
        if (walkBob != null)
        {
            walkBob.UpdateBob(isMoving);
        }
    }
}
