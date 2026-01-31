using UnityEngine;
using UnityEngine.InputSystem;
using TMPro; // If using TextMeshPro

public class BuyMenu : MonoBehaviour
{
    public static BuyMenu Instance;
    public GameObject buyMenuUI;
    public bool IsOpen { get; private set; }

    [Header("Player Shoot Manager")]
    public PlayerShoot playerShoot;          // The main player UI root

    [Header("UI")]
    public GameObject playerUIRoot;          // The main player UI root
    public PlayerUI playerUI;          // The main player UI root
    public TMP_Text buyMenuMoneyText;        // Text element inside Buy Menu to show money
    public GameObject HotPotatoButton;
    public GameObject frenchFriesButton;

    [Header("Ammo")]
    public int hotPotatoPrice = 10;
    public Projectile hotPotatoProjectile;
    public int frenchFriesPrice = 20;
    public Projectile frenchFryProjectile;

    private DefaultInputActions inputActions;

    public AudioManager audioManager;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        inputActions = new DefaultInputActions();
    }

    void Start()
    {
        buyMenuUI.SetActive(false);
    }

    private void UpdateMoneyText()
    {
        // Copy the current money from main UI
        if (buyMenuMoneyText != null && playerUI != null)
        {
            buyMenuMoneyText.text = $"$ {playerUI.GetMoney()}";
        }
    }

    public void OpenMenu()
    {
        IsOpen = true;
        buyMenuUI.SetActive(true);

        // Hide main UI
        if (playerUIRoot != null)
            playerUIRoot.SetActive(false);

        UpdateMoneyText();

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseMenu()
    {
        IsOpen = false;
        buyMenuUI.SetActive(false);

        // Show main UI
        if (playerUIRoot != null)
            playerUIRoot.SetActive(true);

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ToggleMenu()
    {
        if (buyMenuUI.activeSelf)
            CloseMenu();
        else
            OpenMenu();
    }

    public void BuyHotPotato()
    {
        if (playerUI == null || playerShoot == null) return;
        if (playerUI.GetMoney() < hotPotatoPrice) return;

        playerUI.AddMoney(-hotPotatoPrice);
        UpdateMoneyText();

        // Add ammo to PlayerShoot
        playerShoot.AddAmmo(hotPotatoProjectile);

        // Automatically select it
        int slot = playerShoot.GetAmmoSlotIndex(hotPotatoProjectile);
        playerShoot.SelectAmmo(slot);

        // Unlock French Fry purchase
        if (frenchFriesButton != null)
            frenchFriesButton.SetActive(true);
        audioManager.PlaySFX(0);
    }

    public void BuyFrenchFry()
    {
        if (playerUI == null || playerShoot == null) return;
        if (playerUI.GetMoney() < frenchFriesPrice) return;

        playerUI.AddMoney(-frenchFriesPrice);
        UpdateMoneyText();

        playerShoot.AddAmmo(frenchFryProjectile);

        // Immediately select it
        int slot = playerShoot.GetAmmoSlotIndex(frenchFryProjectile);
        playerShoot.SelectAmmo(slot);
        audioManager.PlaySFX(0);
    }

}
