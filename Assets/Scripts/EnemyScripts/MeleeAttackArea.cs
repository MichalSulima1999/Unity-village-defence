using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackArea : MonoBehaviour
{
    public int damage { get; set; }

    private void Start() {
        Destroy(gameObject, 0.25f);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            other.GetComponent<PlayerStats>().TakeDamage(damage);
        }
    }
}
