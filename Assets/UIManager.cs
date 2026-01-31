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

    private int money = 0;
    private int currentHP;

    void Start()
    {
        currentHP = maxHP;
        UpdateHPBar();
        UpdateMoneyUI();
    }

    // ── HP ─────────────────────────────
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
