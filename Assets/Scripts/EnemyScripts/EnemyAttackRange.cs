using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackRange : EnemyAttack
{
    [SerializeField] private Transform barrelTransform;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private EnemyStats enemyStats;

    public override void Attack(Transform target) {
        if (!alreadyAttacked) {
            GameObject bullet = Instantiate(bulletPrefab, barrelTransform.position, Quaternion.identity);
            BulletController bulletController = bullet.GetComponent<BulletController>();
            bulletController.target = target.position;
            bulletController.damage = enemyStats.damage;

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
}
