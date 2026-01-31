using System.Collections;
using UnityEngine;


public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager Instance;

    [Header("Player")]
    public PlayerMovement playerMovement;
    public PlayerShoot playerShoot;
    
    [Header("Durations")]
    public float biggerBulletsDuration = 10f;
    public float speedDuration = 8f;

    private Coroutine biggerBulletsRoutine;
    private Coroutine speedRoutine;

    public AudioManager audioManager;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void Activate(PowerUpType type)
    {
        if (type == PowerUpType.BiggerBullets) ActivateBiggerBullets();
        if (type == PowerUpType.Speed) ActivateSpeed();
        if (type == PowerUpType.Nuke) ActivateNuke();
    }

    private void ActivateBiggerBullets()
    {
        audioManager.PlaySFX(11);
        if (biggerBulletsRoutine != null)
            StopCoroutine(biggerBulletsRoutine);

        biggerBulletsRoutine = StartCoroutine(BiggerBulletsTimer());
    }

    IEnumerator BiggerBulletsTimer()
    {
        playerShoot.SetBiggerBullets(true);
        yield return new WaitForSeconds(biggerBulletsDuration);
        playerShoot.SetBiggerBullets(false);
        biggerBulletsRoutine = null;
    }

    private void ActivateSpeed()
    {
        audioManager.PlaySFX(10);
        if (speedRoutine != null)
            StopCoroutine(speedRoutine);

        speedRoutine = StartCoroutine(SpeedTimer());
    }

    IEnumerator SpeedTimer()
    {
        float originalSpeed = playerMovement.GetSpeed();
        playerMovement.SetSpeed(originalSpeed * 1.5f);

        yield return new WaitForSeconds(speedDuration);

        playerMovement.SetSpeed(originalSpeed);
        speedRoutine = null;
    }

    private void ActivateNuke()
    {
        audioManager.PlaySFX(9);
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
            enemy.Die();
    }
}
