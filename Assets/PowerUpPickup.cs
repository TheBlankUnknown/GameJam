using UnityEngine;

public class PowerUpPickup : MonoBehaviour
{
    public PowerUpType powerUpType;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PowerUpManager.Instance.Activate(powerUpType);
        Destroy(gameObject);
    }
}
