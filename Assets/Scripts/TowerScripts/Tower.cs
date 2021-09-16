using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    [SerializeField] private GameObject parent;
    [SerializeField] private Transform barrel;
    [SerializeField] private GameObject bulletGO;
    
    [SerializeField] private float turnSpeed;
    [SerializeField] private string enemyTag = "Enemy";

    [SerializeField] private SFXManager sFXManager;

    [SerializeField] private TowerDestroy towerDestroy;

    public int maxHealth { get; set; }
    public int damage { get; set; }
    public float attackRange { get; set; }
    public float shootingSpeed { get; set; }
    private float sightRange;

    private int currentHealth;
    private Transform target;
    private float fireCountdown;

    [Header("UI")]
    [SerializeField] Image healthBar;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        currentHealth = maxHealth;
        sightRange = attackRange + 10f;

        healthBar.fillAmount = 1;

        towerDestroy.enabled = false;
    }

    void UpdateTarget() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies) {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance) {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= sightRange) {
            target = nearestEnemy.transform;
        } else {
            target = null;
        }
    }
    void LockOnTarget() {
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    private bool CanSeeTarget(Transform target) {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, (target.position - transform.position));
        Debug.DrawRay(transform.position, (target.position - transform.position), Color.green);

        if (Physics.Raycast(ray, out hit)) {
            if (hit.collider.tag == enemyTag) {
                return true;
            }
        }
        return false;
    }


    // Update is called once per frame
    void Update()
    {
        if (target == null)
            return;

        if (!CanSeeTarget(target))
            return;

        LockOnTarget();

        if(fireCountdown <= 0f && Vector3.Distance(transform.position, target.position) <= attackRange) {
            Shoot();
            fireCountdown = shootingSpeed;
        }
        fireCountdown -= Time.deltaTime;
    }

    void Shoot() {
        GameObject bullet = Instantiate(bulletGO, barrel.position, Quaternion.identity);
        BulletController bulletController = bullet.GetComponent<BulletController>();
        bulletController.target = target.position;
        bulletController.damage = damage;

        sFXManager.PlayShootPlayer(transform);
    }

    public void TakeDamage(int amount) {
        currentHealth -= amount;
        healthBar.fillAmount = (float)currentHealth / (float)maxHealth;

        if (currentHealth <= 0)
            DestroyTurret();
    }

    void DestroyTurret() {
        towerDestroy.enabled = true;
        this.enabled = false;

        Destroy(parent, 1f);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
