using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBlueprintBottom : MonoBehaviour {
    public bool grounded { get; set; } = false;

    private void OnTriggerStay(Collider other) {
        if(other.gameObject.layer == 6) {
            grounded = true;
        }
        //grounded = true;
    }

    private void OnTriggerExit(Collider other) {
        grounded = false;
    }

    private void Update() {
        Debug.Log("Grounded: " + grounded);
    }
}
