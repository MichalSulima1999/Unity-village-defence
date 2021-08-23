using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBlueprint : MonoBehaviour
{
    [SerializeField] private TowerBlueprintBottom towerBlueprintBottom;
    [SerializeField] private Vector3 colliderOffset;
    [SerializeField] private Vector3 colliderDimensions;

    public bool canBuild { get; set; } = false;

    private bool blueprintColliding = false;

    private void FixedUpdate() {
        blueprintColliding = Physics.CheckBox(transform.position + colliderOffset, colliderDimensions / 2f);

        if (towerBlueprintBottom.grounded && !blueprintColliding)
            canBuild = true;
        else
            canBuild = false;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + colliderOffset, colliderDimensions);
    }
}
