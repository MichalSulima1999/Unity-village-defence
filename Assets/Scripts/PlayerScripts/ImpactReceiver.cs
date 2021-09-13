using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class ImpactReceiver : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 impact = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    public void AddImpact(Vector3 dir, float force) {
        dir.Normalize();

        if (dir.y < 0)
            dir.y = -dir.y;

        impact += dir.normalized * force;
    }

    private void Update() {
        if (impact.magnitude > 0.2)
            controller.Move(impact * Time.deltaTime);

        impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.deltaTime);
    }
}
