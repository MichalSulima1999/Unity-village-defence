using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBase : MonoBehaviour
{
    [SerializeField] private int maxHealth = 1000;
    private int currentHealth;

    [SerializeField] private Text nameText;
    [SerializeField] private string baseName = "BASE";
    [SerializeField] private Image hpImage;
    private Image hpPlayerCanvas;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        nameText.text = baseName;

        hpPlayerCanvas = GameObject.FindGameObjectWithTag("BaseHpPlayerUI").GetComponent<Image>();
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;
        hpImage.fillAmount = (float)currentHealth / (float)maxHealth;
        hpPlayerCanvas.fillAmount = hpImage.fillAmount;

        if(currentHealth <= 0) {
            Destroy(gameObject);
            GameManager.GameOver = true;
        }
    }

    public void Repair(float percentage) {
        currentHealth += (int)(percentage / 100 * maxHealth);

        hpImage.fillAmount = (float)currentHealth / (float)maxHealth;
        hpPlayerCanvas.fillAmount = hpImage.fillAmount;

        if (currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }
    }

    public bool CanRepair() {
        if (currentHealth >= maxHealth)
            return false;

        return true;
    }

    public int HealthPercentage() {
        return currentHealth / maxHealth;
    }
}
