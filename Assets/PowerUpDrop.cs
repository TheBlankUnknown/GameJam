using UnityEngine;

public class PowerUpDrop : MonoBehaviour
{
    [Header("Drop Chance")]
    [Range(0f, 1f)]
    public float dropChance = 0.1f;

    [Header("PowerUp Prefabs")]
    public GameObject biggerBulletsPrefab;
    public GameObject speedPrefab;
    public GameObject nukePrefab;

    [Header("Rarity Weights")]
    public int biggerBulletsWeight = 50;
    public int speedWeight = 40;
    public int nukeWeight = 10; // rarer

    public void TryDrop()
    {
        if (Random.value > dropChance)
            return;

        GameObject prefabToDrop = GetWeightedRandomPowerUp();
        Instantiate(prefabToDrop, transform.position, Quaternion.identity);
    }

    private GameObject GetWeightedRandomPowerUp()
    {
        int totalWeight = biggerBulletsWeight + speedWeight + nukeWeight;
        int roll = Random.Range(0, totalWeight);

        if (roll < biggerBulletsWeight)
            return biggerBulletsPrefab;

        roll -= biggerBulletsWeight;

        if (roll < speedWeight)
            return speedPrefab;

        return nukePrefab;
    }
}
