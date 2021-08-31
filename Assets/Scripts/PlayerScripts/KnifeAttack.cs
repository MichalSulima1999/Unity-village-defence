using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeAttack : MonoBehaviour
{
    public int damage { get; set; }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy")) {
            other.GetComponent<EnemyStats>().takeDamage(damage);
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<SFXManager>().PlaySwoshHit(transform);
        }
    }
}
