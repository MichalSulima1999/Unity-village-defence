using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public int damage { get; set; }
    public bool enemyBullet { get; set; }

    private void OnTriggerEnter(Collider other) {
        if (!enemyBullet && other.CompareTag("Enemy")) {
            other.GetComponent<EnemyStats>().takeDamage(damage);
        } else if (enemyBullet && other.CompareTag("Player")) {
            other.GetComponent<PlayerStats>().takeDamage(damage);
        } else if (enemyBullet && other.CompareTag("Base")) {
            other.GetComponent<PlayerBase>().TakeDamage(damage);
        } else if (enemyBullet && other.CompareTag("Turret")) {
            other.GetComponent<Tower>().TakeDamage(damage);
        }
    }
}
