using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public int pistolDamage;
    public int knifeDamage;
    public int granadeDamage { get; set; } = 20;
    
    private int currentHealth;
    public int currentBullets { get; set; }
    public int currentMagazines { get; set; }
    public int currentGranades { get; set; }
    public int towerDamage { get; set; }
    public int towerHealth { get; set; }
    public float towerAttackSpeed { get; set; }
    public float towerAttackRange { get; set; }
    [SerializeField] private int money = 100;
    [SerializeField] private int startingMagazines = 1;
    [SerializeField] private int startingGranades = 2;

    [SerializeField] private int maxHealth = 100;
    [SerializeField] public int maxBullets { get; set; } = 12;
    [SerializeField] public int maxMagazines { get; set; } = 6;
    [SerializeField] public int maxGranades { get; set; } = 4;

    [Header("UI")]
    [SerializeField] private Image healthBar;
    [SerializeField] private Text moneyText;
    [SerializeField] private Text magazinesText;
    [SerializeField] private Text bulletText;
    [SerializeField] private Text granadesText;
    [SerializeField] private Text infoText;

    [Header("References")]
    [SerializeField] RagdollDeath ragdollDeath;
    [SerializeField] PlayerController playerController;

    public SFXManager sFXManager { get; set; }

    // Start is called before the first frame update
    void Start() {
        currentHealth = maxHealth;
        currentBullets = maxBullets;
        currentMagazines = startingMagazines;
        currentGranades = startingGranades;

        moneyText.text = "$" + money;
        bulletText.text = currentBullets + "/" + maxBullets;
        magazinesText.text = currentMagazines + "/" + maxMagazines;
        granadesText.text = currentGranades + "/" + maxGranades;

        sFXManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SFXManager>();
    }

    public void TakeDamage(int amount) {
        currentHealth -= amount;

        healthBar.fillAmount = (float)currentHealth / (float)maxHealth;

        if (currentHealth <= 0) {
            ragdollDeath.ToggleRagdoll(true);
            playerController.enabled = false;
            GameManager.Lost = true;
        }
    }

    public void Heal(float percentage) {
        currentHealth += (int)((percentage / 100) * maxHealth);

        healthBar.fillAmount = (float)currentHealth / (float)maxHealth;

        if (currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }
    }

    public void CollectMoney(int amount) {
        money += amount;
        moneyText.text = "$" + money;
    }

    public int GetMoney() {
        return money;
    }

    public void Reload() {
        if (currentBullets >= maxBullets || currentMagazines <= 0)
            return;

        currentBullets = maxBullets;
        currentMagazines--;

        sFXManager.PlayReload(transform);

        playerController.PlayReloadAnim();
        UpdateBulletText();
    }

    public bool HasBullets() {
        if (currentBullets > 0)
            return true;

        sFXManager.PlayEmpty(transform);

        if (infoText.gameObject.activeSelf == false) {
            EnDisableInfoText();
            infoText.text = "RELOAD!";
            Invoke("EnDisableInfoText", 2f);
        }
        
        return false;
    }

    private void EnDisableInfoText() {
        infoText.gameObject.SetActive(!infoText.gameObject.activeSelf);
    }

    public void UpdateBulletText() {
        bulletText.text = currentBullets + "/" + maxBullets;
        magazinesText.text = currentMagazines + "/" + maxMagazines;
    }

    public void CollectMagazines(int amount) {
        currentMagazines += amount;
        UpdateBulletText();
    }

    public bool CanCollectMagazines() {
        if (currentMagazines >= maxMagazines)
            return false;

        return true;
    }
    
    public bool CanCollectHealth() {
        if (currentHealth >= maxHealth)
            return false;

        return true;
    }

    public void UpgradeHealth(int amount) {
        currentHealth += amount - maxHealth;
        maxHealth = amount;
    }

    public bool CanCollectGranades() {
        if (currentGranades >= maxGranades)
            return false;

        return true;
    }

    public void CollectGranades(int amount) {
        currentGranades += amount;
        granadesText.text = currentGranades + "/" + maxGranades;
    }

    public bool HasGranades() {
        if (currentGranades > 0)
            return true;

        if (infoText.gameObject.activeSelf == false) {
            EnDisableInfoText();
            infoText.text = "NO GRANADES!";
            Invoke("EnDisableInfoText", 2f);
        }

        return false;
    }

    public void UpgradeGranades(int amount) {
        granadeDamage = amount;
    }

    public void UpgradeKnife(int amount) {
        knifeDamage = amount;
    }

    public void UpgradePistol(int amount) {
        pistolDamage = amount;
    }

    public int GetHealthPercentage() {
        return (int)((float)currentHealth / (float)maxHealth * 100);
    }
}
