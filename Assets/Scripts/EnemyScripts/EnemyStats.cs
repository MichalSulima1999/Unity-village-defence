using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : MonoBehaviour
{
    public int damage;
    [SerializeField] private string enemyName;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int moneyLoot = 10;
    [SerializeField] private GameObject[] drops;
    [SerializeField] [Range(0, 100)] private int dropChance = 50;
    private int currentHealth;

    [SerializeField] private Slider hpSlider;
    [SerializeField] private Text enemyNameText;

    [SerializeField] private EnemyAI enemyAI;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        hpSlider.maxValue = maxHealth;
        hpSlider.value = currentHealth;
        enemyNameText.text = enemyName;
    }

    public void takeDamage(int amount) {
        currentHealth -= amount;
        hpSlider.value = currentHealth;

        if(currentHealth <= 0) {
            enemyAI.player.GetComponent<PlayerStats>().CollectMoney(moneyLoot);
            DropSomething();
            Destroy(gameObject, 0.01f);
        }
    }

    void DropSomething() {
        int randomDropChance = Random.Range(0, 100);

        if(randomDropChance < dropChance) {
            int randomDropIndex = Random.Range(0, drops.Length);
            Instantiate(drops[randomDropIndex], transform.position, Quaternion.identity);
        }
    }
}
