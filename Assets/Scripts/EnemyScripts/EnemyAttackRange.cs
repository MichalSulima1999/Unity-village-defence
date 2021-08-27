using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum RangedSound{
    Gun,
    Silent
}

public class EnemyAttackRange : EnemyAttack
{
    [SerializeField] private Transform barrelTransform;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private EnemyStats enemyStats;
    [SerializeField] private RangedSound soundType;

    private SFXManager sFXManager;

    private void Start() {
        sFXManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SFXManager>();
    }

    public override void Attack(Transform target) {
        if (!alreadyAttacked) {
            GameObject bullet = Instantiate(bulletPrefab, barrelTransform.position, Quaternion.identity);
            BulletController bulletController = bullet.GetComponent<BulletController>();
            bulletController.target = target.position;
            bulletController.damage = enemyStats.damage;

            switch (soundType) {
                case RangedSound.Gun:
                    sFXManager.PlayShootEnemy(transform);
                    break;
                case RangedSound.Silent:
                    break;
            }

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);

            
        }
    }
}
