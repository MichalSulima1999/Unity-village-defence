using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GranadeAttack : MonoBehaviour {
    [SerializeField] PlayerStats playerStats;
    [SerializeField] float range = 20f;
    public float cooldown = 0.25f;
    [SerializeField] float timeOffset = 0.5f;

    [SerializeField] Transform hand;
    [SerializeField] GameObject granadePrefab;

    private Transform cameraTransform;

    public void ThrowGranade() {
        cameraTransform = Camera.main.transform;

        Invoke("Throw", timeOffset);
    }

    void Throw() {
        /*Ray r = new Ray(cameraTransform.position, cameraTransform.forward);
        GameObject granade = Instantiate(granadePrefab, hand.position + cameraTransform.forward * 2, Quaternion.identity);
        GranadeController granadeController = granade.GetComponent<GranadeController>();
        granadeController.damage = playerStats.granadeDamage;
        granadeController.target = r.GetPoint(range);

        playerStats.CollectGranades(-1);*/
    }
}
