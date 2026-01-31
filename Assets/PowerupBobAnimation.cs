using UnityEngine;

public class PowerUpFloatAndSpin : MonoBehaviour
{
    [Header("Bobbing")]
    public float bobHeight = 0.25f;
    public float bobSpeed = 2f;

    [Header("Spin")]
    public float spinSpeed = 90f; // degrees per second

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        // ⬆⬇ Vertical bob
        float yOffset = Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = startPos + Vector3.up * yOffset;

        // 🔄 Horizontal spin (Y axis)
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime, Space.World);
    }
}
