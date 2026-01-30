using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    public Transform firePoint;
    public Projectile projectilePrefab;
    public InputActionAsset inputActions;
    public float fireRate = 0.5f;

    [Header("French Fry Cluster")]
    public int fryCount = 5;
    public float spreadAngle = 8f; // degrees

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
        var map = inputActions.FindActionMap("Player");
        attackAction = map.FindAction("Attack");

        attackAction.Enable();
        attackAction.performed += Shoot;
    }

    private void OnDisable()
    {
        attackAction.performed -= Shoot;
        attackAction.Disable();
    }

    private void Shoot(InputAction.CallbackContext context)
    {
        if (BuyMenu.Instance != null && BuyMenu.Instance.IsOpen) return;
        if (Time.time < nextFireTime) return;

        Rigidbody playerRb = GetComponent<Rigidbody>();
        Vector3 playerVelocity =
            playerRb != null ? playerRb.linearVelocity : Vector3.zero;

        // Aim from screen center
        Ray ray = mainCamera.ViewportPointToRay(
            new Vector3(0.5f, 0.5f, 0f)
        );

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            targetPoint = hit.point;
        else
            targetPoint = ray.origin + ray.direction * 100f;

        Vector3 baseDirection =
            (targetPoint - firePoint.position).normalized;

        // 🔑 CLUSTER LOGIC
        if (projectilePrefab.name.Contains("FrenchFry"))
        {
            for (int i = 0; i < fryCount; i++)
            {
                float angleOffset =
                    Random.Range(-spreadAngle, spreadAngle);

                Quaternion spreadRot =
                    Quaternion.AngleAxis(angleOffset, Vector3.up);

                Vector3 spreadDir =
                    spreadRot * baseDirection;

                Projectile fry = Instantiate(
                    projectilePrefab,
                    firePoint.position,
                    Quaternion.LookRotation(spreadDir)
                );

                fry.Launch(spreadDir, playerVelocity);
            }
        }
        else
        {
            // Normal single shot
            Projectile bullet = Instantiate(
                projectilePrefab,
                firePoint.position,
                Quaternion.LookRotation(baseDirection)
            );

            bullet.Launch(baseDirection, playerVelocity);
        }

        nextFireTime = Time.time + fireRate;
    }
}
