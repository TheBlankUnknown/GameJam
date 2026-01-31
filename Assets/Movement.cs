using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 6f;

    [Header("Look Input")]
    [HideInInspector] public Vector2 lookInput;

    [Header("Visuals")]
    public SimpleWalkBob walkBob;

    [Header("Jumping")]
    public float jumpForce = 5f;
    public LayerMask groundMask;
    public float groundCheckDistance = 0.1f;

    private bool isGrounded;

    private Rigidbody rb;
    private Vector2 moveInput;
    private Camera cam;

    private bool isMoving;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation; // prevents tipping over
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
        Vector3 camForward = cam.transform.forward;
        Vector3 camRight = cam.transform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = camForward * moveInput.y + camRight * moveInput.x;

        Vector3 velocity = rb.linearVelocity; // preserve vertical velocity

        if (moveDir.sqrMagnitude > 0.001f)
        {
            isMoving = true;
            Vector3 moveHorizontal = moveDir.normalized * speed;
            velocity.x = moveHorizontal.x;
            velocity.z = moveHorizontal.z;
        }
        else
        {
            isMoving = false;
            velocity.x = 0f;
            velocity.z = 0f;
        }

        rb.linearVelocity = velocity;

        // Rotation (unchanged)
        Vector3 lookDirection = camForward;
        if (lookDirection.sqrMagnitude > 0.001f)
        {
            rb.rotation = Quaternion.LookRotation(lookDirection) * Quaternion.Euler(0, -90f, 0);
        }
    }

    private void Update()
    {
        isGrounded = CheckGrounded();

        if (walkBob != null)
        {
            walkBob.UpdateBob(isMoving);
        }
    }

    private bool CheckGrounded()
    {
        BoxCollider box = GetComponent<BoxCollider>();
        if (box == null) return false;

        // Bottom center of the box
        Vector3 rayOrigin = transform.position + box.center - Vector3.up * (box.size.y / 2 - 0.01f);
        float rayLength = groundCheckDistance + 0.01f;

        bool grounded = Physics.Raycast(rayOrigin, Vector3.down, rayLength, groundMask);
        Debug.DrawRay(rayOrigin, Vector3.down * rayLength, grounded ? Color.green : Color.red);

        return grounded;
    }

    void OnJump(InputValue value)
    {
        Debug.Log("Jump pressed, grounded: " + isGrounded);
        if (value.isPressed && isGrounded)
        {
            // Smooth physics jump
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    public float GetSpeed()
    {
        return speed;
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
}
