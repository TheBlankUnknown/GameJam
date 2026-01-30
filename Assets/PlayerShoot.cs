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
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
            Debug.LogError("No Main Camera found in the scene!");
    }

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

        // Calculate direction from camera to screen center
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        Vector3 targetPoint;

        // Raycast to see if we hit something
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.origin + ray.direction * 100f; // far away if nothing hit
        }

        Vector3 shootDirection = (targetPoint - firePoint.position).normalized;

        Projectile bullet = Instantiate(projectilePrefab, firePoint.position, Quaternion.LookRotation(shootDirection));
        bullet.Launch(shootDirection, playerVelocity);

        nextFireTime = Time.time + fireRate;
        Debug.Log("Bullet shot towards screen center");
    }
}
