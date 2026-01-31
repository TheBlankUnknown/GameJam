using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerShoot : MonoBehaviour
{
    [Header("References")]
    public Transform firePoint;
    public InputActionAsset inputActions;
    public PlayerUI playerUI; // optional: update ammo icon

    [Header("Shooting")]
    public float fireRate = 0.5f;

    [Header("French Fry Cluster")]
    public int fryCount = 5;
    public float spreadAngle = 8f;

    [Header("Default Ammo")]
    public Projectile defaultProjectile; // slot 1 default

    private InputAction attackAction;
    private float nextFireTime;
    private Camera mainCamera;

    // --- Ammo system ---
    private List<Projectile> ammoSlots = new List<Projectile>();
    private int currentSlotIndex = 0;
    private const int maxSlots = 5;

    private void Awake()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
            Debug.LogError("No Main Camera found in the scene!");

        // Add default projectile to slot 1
        if (defaultProjectile != null)
            AddAmmo(defaultProjectile);
    }

    private void OnEnable()
    {
        var map = inputActions.FindActionMap("Player");

        attackAction = map.FindAction("Attack");
        attackAction.Enable();
        attackAction.performed += Shoot;

        // Number keys to switch ammo
        for (int i = 1; i <= 5; i++)
        {
            int index = i - 1; // slot 0-4
            var keyAction = map.FindAction($"Select{i}");
            if (keyAction != null)
            {
                keyAction.Enable();
                keyAction.performed += ctx => SwitchAmmo(index);
            }
        }
    }

    private void OnDisable()
    {
        attackAction.performed -= Shoot;
        attackAction.Disable();

        for (int i = 1; i <= 5; i++)
        {
            var keyAction = inputActions.FindActionMap("Player")?.FindAction($"Select{i}");
            if (keyAction != null)
            {
                keyAction.performed -= ctx => SwitchAmmo(i - 1);
                keyAction.Disable();
            }
        }
    }

    public void AddAmmo(Projectile projectile)
    {
        if (ammoSlots.Contains(projectile)) return;

        if (ammoSlots.Count >= maxSlots)
        {
            // Replace the oldest ammo except slot 1 (default)
            ammoSlots.RemoveAt(1); // keep slot 0
        }

        ammoSlots.Add(projectile);

        // Auto-select newly added ammo
        currentSlotIndex = ammoSlots.Count - 1;
        UpdateUI();
    }

    private void SwitchAmmo(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= ammoSlots.Count) return;

        currentSlotIndex = slotIndex;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (playerUI != null && ammoSlots.Count > 0)
        {
            string name = ammoSlots[currentSlotIndex].name;

            if (name.Contains("FrenchFry"))
                playerUI.SetAmmoType("FrenchFry");
            else if (name.Contains("Special"))
                playerUI.SetAmmoType("Special");
            else
                playerUI.SetAmmoType("Normal");
        }
    }

    private void Shoot(InputAction.CallbackContext context)
    {
        if (BuyMenu.Instance != null && BuyMenu.Instance.IsOpen) return;
        if (Time.time < nextFireTime) return;
        if (ammoSlots.Count == 0) return;

        Projectile projectilePrefab = ammoSlots[currentSlotIndex];

        Rigidbody playerRb = GetComponent<Rigidbody>();
        Vector3 playerVelocity =
            playerRb != null ? playerRb.linearVelocity : Vector3.zero;

        // Aim from screen center
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            targetPoint = hit.point;
        else
            targetPoint = ray.origin + ray.direction * 100f;

        Vector3 baseDirection = (targetPoint - firePoint.position).normalized;

        // FrenchFry cluster
        if (projectilePrefab.name.Contains("FrenchFry"))
        {
            for (int i = 0; i < fryCount; i++)
            {
                float angleOffset = Random.Range(-spreadAngle, spreadAngle);
                Quaternion spreadRot = Quaternion.AngleAxis(angleOffset, Vector3.up);
                Vector3 spreadDir = spreadRot * baseDirection;

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
