using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum CollectibleType {
    Health,
    Ammo,
    Granade
}

public class Collectible : MonoBehaviour {
    [SerializeField] CollectibleType collectibleType;
    [SerializeField] int amount;
    [SerializeField] SFXManager sFXManager;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            switch (collectibleType) {
                case CollectibleType.Health:
                    if (playerStats.CanCollectHealth()) {
                        playerStats.Heal(amount);
                        sFXManager.PlayCollectible(transform);
                        Destroy(gameObject);
                    }   
                    break;
                case CollectibleType.Ammo:
                    if (playerStats.CanCollectMagazines()) {
                        other.GetComponent<PlayerStats>().CollectMagazines(amount);
                        sFXManager.PlayCollectible(transform);
                        Destroy(gameObject);
                    } 
                    break;
                case CollectibleType.Granade:
                    if (playerStats.CanCollectGranades()) {
                        other.GetComponent<PlayerStats>().CollectGranades(amount);
                        sFXManager.PlayCollectible(transform);
                        Destroy(gameObject);
                    }
                    break;
            }
        }
    }
}
