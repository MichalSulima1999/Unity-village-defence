using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [Header("SFX")]
    [SerializeField] private GameObject shootPlayerSFX;
    [SerializeField] private GameObject shootEnemySFX;
    [SerializeField] private GameObject shootHitSFX;
    [SerializeField] private GameObject reloadSFX;
    [SerializeField] private GameObject swoshSFX;
    [SerializeField] private GameObject swoshHitSFX;
    [SerializeField] private GameObject emptySFX;
    [SerializeField] private GameObject granadeExplosionSFX;
    [SerializeField] private GameObject collectibleSFX;
    [SerializeField] private GameObject buildSFX;

    public void PlayShootPlayer(Transform location) {
        GameObject sfx = Instantiate(shootPlayerSFX, location.position, Quaternion.identity);
        Destroy(sfx, 2f);
    }

    public void PlayShootEnemy(Transform location) {
        GameObject sfx = Instantiate(shootEnemySFX, location.position, Quaternion.identity);
        Destroy(sfx, 2f);
    }

    public void PlayShootHit(Transform location) {
        GameObject sfx = Instantiate(shootHitSFX, location.position, Quaternion.identity);
        Destroy(sfx, 2f);
    }

    public void PlayReload(Transform location) {
        GameObject sfx = Instantiate(reloadSFX, location.position, Quaternion.identity);
        Destroy(sfx, 2f);
    }

    public void PlaySwosh(Transform location) {
        GameObject sfx = Instantiate(swoshSFX, location.position, Quaternion.identity);
        Destroy(sfx, 2f);
    }

    public void PlaySwoshHit(Transform location) {
        GameObject sfx = Instantiate(swoshHitSFX, location.position, Quaternion.identity);
        Destroy(sfx, 2f);
    }

    public void PlayEmpty(Transform location) {
        GameObject sfx = Instantiate(emptySFX, location.position, Quaternion.identity);
        Destroy(sfx, 2f);
    }

    public void PlayGranadeExplosion(Transform location) {
        GameObject sfx = Instantiate(granadeExplosionSFX, location.position, Quaternion.identity);
        Destroy(sfx, 2f);
    }
    
    public void PlayCollectible(Transform location) {
        GameObject sfx = Instantiate(collectibleSFX, location.position, Quaternion.identity);
        Destroy(sfx, 2f);
    }

    public void PlayBuild(Transform location) {
        GameObject sfx = Instantiate(buildSFX, location.position, Quaternion.identity);
        Destroy(sfx, 2f);
    }
}
