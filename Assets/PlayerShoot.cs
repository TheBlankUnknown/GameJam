using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    public Transform firePoint;
    public Projectile projectilePrefab;
    public InputActionAsset inputActions;
    public float fireRate = 0.5f;

    private InputAction attackAction;
    private float nextFireTime;

    private void OnEnable()
    {
        if (inputActions == null)
        {
            Debug.LogError("InputActions asset not assigned!");
            return;
        }

        var map = inputActions.FindActionMap("Player");
        if (map == null)
        {
            Debug.LogError("Player action map not found!");
            return;
        }

        attackAction = map.FindAction("Attack");
        if (attackAction == null)
        {
            Debug.LogError("Attack action not found!");
            return;
        }

        attackAction.Enable();
        attackAction.performed += Shoot;
    }

    private void OnDisable()
    {
        if (attackAction != null)
        {
            attackAction.performed -= Shoot;
            attackAction.Disable();
        }
    }
    private void Shoot(InputAction.CallbackContext context)
    {
        if (Time.time < nextFireTime) return;

        Rigidbody playerRb = GetComponent<Rigidbody>();
        Vector3 playerVelocity = playerRb != null ? playerRb.linearVelocity : Vector3.zero;

        Projectile bullet = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        bullet.Launch(firePoint.forward, playerVelocity);

        nextFireTime = Time.time + fireRate;
        Debug.Log("Bullet shot");
    }
    
}