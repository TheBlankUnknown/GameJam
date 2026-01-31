using UnityEngine;
using UnityEngine.UI;
using TMPro; // Required if you use TextMeshPro

public class PlayerUI : MonoBehaviour
{
    [Header("HP")]
    public Slider hpBar;
    public int maxHP = 100;

    [Header("Ammo Icon")]
    public Image ammoIcon;          // UI Image to show current ammo
    public Sprite PotatoSprite;     // Sprite for normal projectile
    public Sprite frenchFrySprite;  // Sprite for FrenchFry
    public Sprite HotPotatoSprite;  // Sprite for third ammo type

    [Header("Money UI")]
    public TMP_Text moneyText;      // Text to display money (use Text if not TMP)
    private int money = 0;

    private int currentHP;

    void Start()
    {
        currentHP = maxHP;
        UpdateHPBar();
        UpdateMoneyUI(); // Initialize money display
    }

    // --- HP Functions ---
    public void SetHP(int hp)
    {
        currentHP = Mathf.Clamp(hp, 0, maxHP);
        UpdateHPBar();
    }

    private void UpdateHPBar()
    {
        if (hpBar != null)
            hpBar.value = currentHP;
    }

    // --- Ammo Functions ---
    public void SetAmmoType(string ammoName)
    {
        if (ammoIcon == null) return;

        switch (ammoName)
        {
            case "Potato":
                ammoIcon.sprite = PotatoSprite;
                break;
            case "FrenchFry":
                ammoIcon.sprite = frenchFrySprite;
                break;
            case "HotPotato":
                ammoIcon.sprite = HotPotatoSprite;
                break;
            default:
                ammoIcon.sprite = PotatoSprite; // fallback
                break;
        }
    }

    // --- Money Functions ---
    public void AddMoney(int amount)
    {
        money += amount;
        UpdateMoneyUI();
    }

    public void SetMoney(int amount)
    {
        money = amount;
        UpdateMoneyUI();
    }

    private void UpdateMoneyUI()
    {
        if (moneyText != null)
            moneyText.text = $"$ {money}";
    }
}
