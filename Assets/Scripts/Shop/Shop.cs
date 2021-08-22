using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] ShopTurret turret;
    [SerializeField] ShopItem knife;
    [SerializeField] ShopItem pistol;
    [SerializeField] ShopItem armor;
    [SerializeField] ShopItem granade;

    [SerializeField] Text moneyText;

    [SerializeField] Text healCostText;
    [SerializeField] Text repairCostText;
    [SerializeField] Button healButton;
    [SerializeField] Button repairButton;

    [SerializeField] int repairCost = 50;

    [SerializeField] PlayerStats playerStats;

    PlayerBase playerBase;

    int healCost;

    private void OnEnable() {
        GameManager.ControlsLocked = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void OnDisable() {
        GameManager.ControlsLocked = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerBase = GameObject.FindGameObjectWithTag("Base").GetComponent<PlayerBase>();

        UpdateStats();
        UpdateText();
    }

    private void Update() {
        CheckButton(turret);
        CheckButton(pistol);
        CheckButton(knife);
        CheckButton(armor);
        CheckButton(granade);

        CheckHealRepairButtons();

        moneyText.text = "$" + playerStats.GetMoney();

        healCost = 100 - playerStats.GetHealthPercentage();
        healCostText.text = healCost + "";

        repairCostText.text = repairCost + "";
    }

    void UpgradeItem(ShopItem item) {
        playerStats.CollectMoney(-item.pricePerLevel[item.level]);
        item.level++;

        UpdateStats();
        UpdateText();
    }

    void CheckButton(ShopItem item) {
        if (item.level >= item.statPerLevel.Length || playerStats.GetMoney() < item.pricePerLevel[item.level]) {
            item.upgradeButton.interactable = false;
        } else {
            item.upgradeButton.interactable = true;
        }
    }

    void CheckHealRepairButtons() {
        if (healCost > playerStats.GetMoney() || !playerStats.CanCollectHealth())
            healButton.interactable = false;
        else
            healButton.interactable = true;

        if (repairCost > playerStats.GetMoney() || !playerBase.CanRepair())
            repairButton.interactable = false;
        else
            repairButton.interactable = true;

    }

    public void TurretUpgrade() {
        UpgradeItem(turret);
    }
    
    public void KnifeUpgrade() {
        UpgradeItem(knife);
    }
    
    public void PistolUpgrade() {
        UpgradeItem(pistol);
    }
    
    public void ArmorUpgrade() {
        UpgradeItem(armor);
    }
    
    public void GranadeUpgrade() {
        UpgradeItem(granade);
    }

    public void Heal() {
        playerStats.Heal(100);
        playerStats.CollectMoney(-healCost);
    }

    public void Repair() {
        playerBase.Repair(10);
        playerStats.CollectMoney(-repairCost);
    }

    public void CloseShop() {
        gameObject.SetActive(false);
    }

    void UpdateStats() {
        playerStats.towerAttackRange = turret.turretRange[turret.level];
        playerStats.towerAttackSpeed = turret.turretAttackSpeed[turret.level];
        playerStats.towerDamage = turret.statPerLevel[turret.level];
        playerStats.towerHealth = turret.turretHealth[turret.level];

        playerStats.UpgradeHealth(armor.statPerLevel[armor.level]);
        playerStats.UpgradeGranades(granade.statPerLevel[granade.level]);
        playerStats.UpgradeKnife(knife.statPerLevel[knife.level]);
        playerStats.UpgradePistol(pistol.statPerLevel[pistol.level]);
    }

    void UpdateText() {
        turret.priceText.text = "$" + turret.pricePerLevel[turret.level];
        pistol.priceText.text = "$" + pistol.pricePerLevel[pistol.level];
        knife.priceText.text = "$" + knife.pricePerLevel[knife.level];
        armor.priceText.text = "$" + armor.pricePerLevel[armor.level];
        granade.priceText.text = "$" + granade.pricePerLevel[granade.level];

        turret.levelText.text = turret.level + "";
        pistol.levelText.text = pistol.level + "";
        knife.levelText.text = knife.level + "";
        armor.levelText.text = armor.level + "";
        granade.levelText.text = granade.level + "";

        moneyText.text = "$" + playerStats.GetMoney();
    }
}
