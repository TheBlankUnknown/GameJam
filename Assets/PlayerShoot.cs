using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerShoot : MonoBehaviour
{
    [Header("References")]
    public Transform firePoint;
    public InputActionAsset inputActions;
    public PlayerUI playerUI; // updates ammo icon

    [Header("Shooting")]
    public float fireRate = 0.5f;

    [Header("Raycast Settings")]
    public LayerMask shootIgnoreLayer; // assign BuyZone layer in inspector

    [Header("French Fry Cluster")]
    public int fryCount = 5;
    public float spreadAngle = 8f;

    [Header("Default Ammo")]
    public Projectile defaultProjectile; // slot 1 default

    private bool biggerBullets = false;

    private InputAction attackAction;
    private readonly List<InputAction> selectActions = new();

    private float nextFireTime;
    private Camera mainCamera;

    // ── Ammo system ─────────────────────────────
    private readonly List<Projectile> ammoSlots = new();
    private int currentSlotIndex = 0;
    private const int maxSlots = 5;

    //audio for shoot sound
    public AudioManager audioManager;


    private void Awake()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
            Debug.LogError("No Main Camera found in the scene!");

        // Slot 1 always has default ammo
        if (defaultProjectile != null && ammoSlots.Count == 0)
            AddAmmo(defaultProjectile);
    }

    private void OnEnable()
    {
        var map = inputActions.FindActionMap("Player");

        // Attack
        attackAction = map.FindAction("Attack");
        attackAction.Enable();
        attackAction.performed += Shoot;

        // Number keys (1–5)
        for (int i = 1; i <= maxSlots; i++)
        {
            int slotIndex = i - 1;
            var action = map.FindAction($"Select{i}");
            if (action == null) continue;

            action.Enable();
            action.performed += _ => SwitchAmmo(slotIndex);
            selectActions.Add(action);
        }
    }

    private void OnDisable()
    {
        if (attackAction != null)
        {
            attackAction.performed -= Shoot;
            attackAction.Disable();
        }

        foreach (var action in selectActions)
        {
            action.Disable();
        }

        selectActions.Clear();
    }

    // ── Ammo management ─────────────────────────
    public bool AddAmmo(Projectile projectile)
    {
        if (projectile == null) return false;
        if (ammoSlots.Contains(projectile)) return false;
        if (ammoSlots.Count >= maxSlots) return false;

        ammoSlots.Add(projectile);
        currentSlotIndex = ammoSlots.Count - 1; // auto-select newest
        UpdateUI();
        return true;
    }

    private void SwitchAmmo(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= ammoSlots.Count) return;

        currentSlotIndex = slotIndex;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (playerUI == null || ammoSlots.Count == 0) return;

        string name = ammoSlots[currentSlotIndex].name;

        if (name.Contains("FrenchFry"))
            playerUI.SetAmmoType("FrenchFry");
        else if (name.Contains("HotPotato"))
            playerUI.SetAmmoType("HotPotato");
        else
            playerUI.SetAmmoType("Potato");
    }

    // ── Shooting ────────────────────────────────
    private void Shoot(InputAction.CallbackContext context)
    {
        if (BuyMenu.Instance != null && BuyMenu.Instance.IsOpen) return;
        if (Time.time < nextFireTime) return;
        if (ammoSlots.Count == 0) return;
        if (Time.timeScale == 0) return;

        float scaleModifier = 1f;
        // Determine scale modifier
        if (biggerBullets) scaleModifier = 2f;

        Projectile projectilePrefab = ammoSlots[currentSlotIndex];

        Rigidbody rb = GetComponent<Rigidbody>();
        Vector3 playerVelocity = rb != null ? rb.linearVelocity : Vector3.zero;

        // Aim from screen center
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, ~shootIgnoreLayer))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.origin + ray.direction * 100f;
        }

        Vector3 baseDirection = (targetPoint - firePoint.position).normalized;

        // French Fry = cluster
        if (projectilePrefab.name.Contains("FrenchFry"))
        {
            for (int i = 0; i < fryCount; i++)
            {
                float angle = Random.Range(-spreadAngle, spreadAngle);
                Vector3 spreadDir =
                    Quaternion.AngleAxis(angle, Vector3.up) * baseDirection;

                Projectile fry = Instantiate(
                    projectilePrefab,
                    firePoint.position,
                    Quaternion.LookRotation(spreadDir)
                );

                // ✅ APPLY SCALE HERE
                fry.transform.localScale *= scaleModifier;

                fry.Launch(spreadDir, playerVelocity);
            }
        }
        else
        {
            Projectile bullet = Instantiate(
                projectilePrefab,
                firePoint.position,
                Quaternion.LookRotation(baseDirection)
            );

            // ✅ APPLY SCALE HERE
            bullet.transform.localScale *= scaleModifier;

            bullet.Launch(baseDirection, playerVelocity);
        }
        audioManager.PlaySFX(4);
        nextFireTime = Time.time + fireRate;
    }

    /// <summary>Returns the index of a projectile in the ammo slots, or -1 if not found</summary>
    public int GetAmmoSlotIndex(Projectile projectile)
    {
        return ammoSlots.IndexOf(projectile);
    }

    /// <summary>Select a projectile in the ammo slots by index</summary>
    public void SelectAmmo(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= ammoSlots.Count) return;
        currentSlotIndex = slotIndex;
        UpdateUI();
    }

    public void SetBiggerBullets(bool flag)
    {
        biggerBullets = flag;
    }

}
