using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackArea : MonoBehaviour
{
    public int damage { get; set; }
    public float force { get; set; }

    private void Start() {
        Destroy(gameObject, 0.25f);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            other.GetComponent<PlayerStats>().TakeDamage(damage);
            other.GetComponent<ImpactReceiver>().AddImpact(transform.forward, force);
            other.GetComponent<ImpactReceiver>().AddImpact(transform.up, force/2);
        } else if (other.CompareTag("Base")) {
            other.gameObject.GetComponent<PlayerBase>().TakeDamage(damage);
        } else if (other.CompareTag("Turret")) {
            other.gameObject.GetComponent<Tower>().TakeDamage(damage);
        }

        
    }
}
