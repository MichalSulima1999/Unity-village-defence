using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackMelee : EnemyAttack {

    [SerializeField] private float knockBackForce = 2f;

    [SerializeField] private SFXManager sFXManager;
    [SerializeField] private GameObject attackArea;
    [SerializeField] private Transform barrelTransform;
    [SerializeField] private Animator animator;
    [SerializeField] private EnemyStats enemyStats;

    public override void Attack(Transform target) {
        if (!alreadyAttacked) {
            GameObject area = Instantiate(attackArea, barrelTransform.position, barrelTransform.rotation);
            MeleeAttackArea meleeAttackArea = area.GetComponent<MeleeAttackArea>();
            meleeAttackArea.damage = enemyStats.damage;
            meleeAttackArea.force = knockBackForce;

            sFXManager.PlaySwosh(transform);

            animator.CrossFade("Gorilla attack", 0.1f);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
}
