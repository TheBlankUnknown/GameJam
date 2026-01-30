using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    public Transform firePoint;
    public Projectile projectilePrefab;
    public InputActionAsset inputActions;
    public float fireRate = 0.5f;

    [Header("Raycast")]
    [SerializeField] private float range = 100f;
    [SerializeField] private LayerMask shootMask; // exclude BuyZone here

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
        // 🚫 Block shooting when buy menu is open
        if (BuyMenu.Instance != null && BuyMenu.Instance.IsOpen)
            return;

        if (Time.time < nextFireTime)
            return;

        Rigidbody playerRb = GetComponent<Rigidbody>();
        Vector3 playerVelocity = playerRb != null ? playerRb.linearVelocity : Vector3.zero;

        // 🔥 ALWAYS raycast from camera center
        Ray ray = new Ray(mainCamera.transform.position,
                          mainCamera.transform.forward);

        Vector3 aimPoint;

        // Ignore trigger colliders (buy zone fix)
        if (Physics.Raycast(
            ray,
            out RaycastHit hit,
            range,
            shootMask,
            QueryTriggerInteraction.Ignore
        ))
        {
            aimPoint = hit.point;
        }
        else
        {
            aimPoint = ray.origin + ray.direction * range;
        }

        // 🔑 Direction is from firePoint → camera aim point
        Vector3 shootDirection = (aimPoint - firePoint.position).normalized;

        Projectile bullet = Instantiate(
            projectilePrefab,
            firePoint.position,
            Quaternion.LookRotation(shootDirection)
        );

        bullet.Launch(shootDirection, playerVelocity);

        nextFireTime = Time.time + fireRate;

        // Debug line to visualize aim
        Debug.DrawRay(mainCamera.transform.position,
                      mainCamera.transform.forward * range,
                      Color.red,
                      1f);
    }
}
