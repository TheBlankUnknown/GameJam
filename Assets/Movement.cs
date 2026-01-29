using UnityEngine;
using UnityEngine.InputSystem;

public class CapsuleMovement : MonoBehaviour
{
    public float speed = 6f;

    private Rigidbody rb;
    private Vector2 moveInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // THIS is what Unity is actually calling
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y);
        rb.linearVelocity = movement * speed;
    }
}
