using UnityEngine;

public abstract class EnemyAttack : MonoBehaviour {
    public float timeBetweenAttacks;
    public float attackRange;
    [HideInInspector] public bool alreadyAttacked = false;

    public abstract void Attack(Transform target);

    public void ResetAttack() {
        alreadyAttacked = false;
    }
}
