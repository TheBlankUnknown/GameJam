using UnityEngine;

public class SimpleWalkBob : MonoBehaviour
{
    [Header("Step Settings")]
    public float stepSpeed = 8f;
    public float verticalBobAmount = 0.06f;
    public float stepTiltAngle = 4f; // side-to-side lean
    public float tiltSmooth = 8f;    // how smooth the transition is

    private Vector3 startLocalPos;
    private Quaternion startLocalRot;
    private float stepTime;

    private float currentTiltX;

    void Start()
    {
        startLocalPos = transform.localPosition;
        startLocalRot = transform.localRotation;
    }

    public void UpdateBob(bool isMoving)
    {
        if (!isMoving)
        {
            // Smoothly return to rest pose
            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                startLocalPos,
                Time.deltaTime * 10f
            );

            currentTiltX = Mathf.Lerp(
                currentTiltX,
                0f,
                Time.deltaTime * tiltSmooth
            );

            transform.localRotation =
                startLocalRot * Quaternion.Euler(currentTiltX, 0f, 0f);

            return;
        }

        stepTime += Time.deltaTime * stepSpeed;

        // Vertical bob (purely visual)
        float bobY = Mathf.Abs(Mathf.Sin(stepTime)) * verticalBobAmount;

        // Target tilt based on step phase (left/right)
        float targetTiltX =
            Mathf.Sign(Mathf.Sin(stepTime)) * stepTiltAngle;

        // 🔑 Smooth tilt transition
        currentTiltX = Mathf.Lerp(
            currentTiltX,
            targetTiltX,
            Time.deltaTime * tiltSmooth
        );

        transform.localPosition =
            startLocalPos + new Vector3(0f, bobY, 0f);

        transform.localRotation =
            startLocalRot * Quaternion.Euler(currentTiltX, 0f, 0f);
    }
}
