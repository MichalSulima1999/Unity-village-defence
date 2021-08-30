using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyStats : MonoBehaviour
{
    public int damage;
    [SerializeField] private string enemyName;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int moneyLoot = 10;
    [SerializeField] private GameObject[] drops;
    [SerializeField] [Range(0, 100)] private int dropChance = 50;
    [SerializeField] private float timeTillDissapear = 1.5f;
    private int currentHealth;

    [SerializeField] private Slider hpSlider;
    [SerializeField] private Text enemyNameText;

    [SerializeField] private EnemyAI enemyAI;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        hpSlider.maxValue = maxHealth;
        hpSlider.value = currentHealth;
        enemyNameText.text = enemyName;

        animator = GetComponent<Animator>();
    }

    public void takeDamage(int amount) {
        currentHealth -= amount;
        hpSlider.value = currentHealth;

        if(currentHealth <= 0) {
            Die();
        }
    }
    
    void Die() {
        enemyAI.player.GetComponent<PlayerStats>().CollectMoney(moneyLoot);
        animator.SetBool("Die", true);
        gameObject.tag = "Untagged";
        GetComponent<EnemyAI>().enabled = false;
        GetComponent<EnemyAttack>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;

        Invoke("DestroySelf", timeTillDissapear);
    }

    void DestroySelf() {
        DropSomething();
        Destroy(gameObject);
    }

    void DropSomething() {
        int randomDropChance = Random.Range(0, 100);

        if(randomDropChance < dropChance) {
            int randomDropIndex = Random.Range(0, drops.Length);
            Instantiate(drops[randomDropIndex], transform.position, Quaternion.identity);
        }
    }
}
