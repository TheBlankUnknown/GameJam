using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Transform))]
public class EnemyHop : MonoBehaviour
{
    [Header("Hop Settings")]
    public float hopHeight = 0.3f;        // How high the enemy hops
    public float hopDuration = 0.2f;      // Time to reach peak
    public float minWait = 2f;            // Minimum time between hops
    public float maxWait = 5f;            // Maximum time between hops

    private bool isHopping = false;

    void Start()
    {
        StartCoroutine(HopRoutine());
    }

    private IEnumerator HopRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(minWait, maxWait);
            yield return new WaitForSeconds(waitTime);

            if (!isHopping)
            {
                isHopping = true;
                yield return HopOnce();
                isHopping = false;
            }
        }
    }

    private IEnumerator HopOnce()
    {
        float t = 0f;
        float halfDuration = hopDuration;

        // Store starting Y position
        float startY = transform.position.y;
        float peakY = startY + hopHeight;

        while (t < halfDuration * 2f)
        {
            t += Time.deltaTime;
            float normalizedTime = t / (halfDuration * 2f);

            // Use sine curve for smooth up/down motion
            float yOffset = Mathf.Sin(normalizedTime * Mathf.PI) * hopHeight;

            Vector3 currentPos = transform.position;
            currentPos.y = startY + yOffset; // Only modify vertical
            transform.position = currentPos;

            yield return null;
        }

        // Ensure it lands exactly where it should be
        Vector3 finalPos = transform.position;
        finalPos.y = startY;
        transform.position = finalPos;
    }
}
