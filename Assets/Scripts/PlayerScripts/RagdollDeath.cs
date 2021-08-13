using UnityEngine;

public class RagdollDeath : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator = null;

    private Rigidbody[] ragdollBodies;
    private Collider[] ragdollColliders;

    private void Start() {
        ragdollBodies = GetComponentsInChildren<Rigidbody>();
        ragdollColliders = GetComponentsInChildren<Collider>();

        ToggleRagdoll(false);
    }

    public void ToggleRagdoll(bool state) {
        animator.enabled = !state;

        foreach(Rigidbody rb in ragdollBodies) {
            rb.isKinematic = !state;
        }

        foreach(Collider collider in ragdollColliders) {
            collider.enabled = state;
        }
    }
}
