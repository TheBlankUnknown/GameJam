using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [Header("HP")]
    public Slider hpBar;
    public int maxHP = 100;

    [Header("Ammo Icon")]
    public Image ammoIcon;
    public Sprite PotatoSprite;
    public Sprite frenchFrySprite;
    public Sprite HotPotatoSprite;

    [Header("Money UI")]
    public TMP_Text moneyText;

    [Header("Damage Effect")]
    public Image damageOverlay;
    public float flashSpeed = 5f;   // How fast it fades
    public float flashAlpha = 0.5f; // Max alpha when damaged
    private bool isDamaged = false;

    private int money = 0;
    private int currentHP;

    void Start()
    {
        currentHP = maxHP;
        UpdateHPBar();
        UpdateMoneyUI();
    }

    void Update()
    {
        if (damageOverlay == null) return;

        if (isDamaged)
        {
            // Set overlay to red with alpha
            damageOverlay.color = new Color(1f, 0f, 0f, flashAlpha);
            isDamaged = false; // reset, will fade automatically
        }
        else
        {
            // Fade back to transparent
            damageOverlay.color = Color.Lerp(damageOverlay.color, Color.clear, flashSpeed * Time.deltaTime);
        }
    }

    // ── HP ─────────────────────────────
    public void SetHP(int hp)
    {
        currentHP = Mathf.Clamp(hp, 0, maxHP);
        UpdateHPBar();
    }

    public void FlashDamage()
    {
        isDamaged = true;
    }

    private void UpdateHPBar()
    {
        if (hpBar != null)
            hpBar.value = currentHP;
    }

    // ── Ammo ───────────────────────────
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
                ammoIcon.sprite = PotatoSprite;
                break;
        }
    }

    // ── Money ──────────────────────────
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

    public int GetMoney()
    {
        return money;
    }

    public string GetMoneyText()
    {
        return $"$ {money}";
    }

    private void UpdateMoneyUI()
    {
        if (moneyText != null)
            moneyText.text = GetMoneyText();
    }
}
