using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform playerCenter;
    [SerializeField] private LayerMask whatIsGround, whatIsBullet;

    // States
    [SerializeField] private float sightRange, hearRange;
    private bool bulletInHearRange, playerInAttackRange;

    [SerializeField] private float lookAtDamp = 0.3f;
    [SerializeField] private Transform sightCenter;

    private AudioSource stepSound;
    private float stepSoundCounter = 0f;
    [SerializeField] private float stepSoundTime = 0.4f;

    private Animator animator;
    private Transform lockedTarget;
    private Vector3 lastKnownPosition;
    private Transform playerBase;
    private GameObject rayCaster;
    private bool targetLost = false;

    private PlayerController playerController;
    public GameObject player { get; set; }
    [SerializeField] private EnemyAttack enemyAttack;

    // Start is called before the first frame update
    void Awake()
    {
        playerCenter = GameObject.FindGameObjectWithTag("PlayerCenter").transform;
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        agent = GetComponent<NavMeshAgent>();
        playerBase = GameObject.FindGameObjectWithTag("Base").transform;
        lockedTarget = playerBase;

        InvokeRepeating(nameof(UpdateTarget), 0f, 0.5f);

        animator = GetComponent<Animator>();

        stepSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("Walking", false);

        if (GameManager.GameOver || lockedTarget == null)
            return;

        if (!targetLost && Vector3.Distance(transform.position, lockedTarget.position) <= enemyAttack.attackRange) {
            EnemyAttack();
        } else {
            if (targetLost) {
                ChaseTarget(lastKnownPosition);
                if (Vector3.Distance(transform.position, lastKnownPosition) < 1f) {
                    targetLost = false;
                    lockedTarget = playerBase;
                }
            } else
                ChaseTarget(lockedTarget.position);
        }

        // hear bullet
        bulletInHearRange = Physics.CheckSphere(transform.position, hearRange, whatIsBullet);
        if (bulletInHearRange && lockedTarget == playerBase) {
            lastKnownPosition = playerCenter.position;
            targetLost = true;
        }

        stepSoundCounter -= Time.deltaTime;
    }

    void UpdateTarget() {
        if (Vector3.Distance(sightCenter.position, playerCenter.position) <= sightRange || (!playerController.crouching && Vector3.Distance(transform.position, playerCenter.position) <= hearRange)) {
            LockTarget(playerCenter);
        }

        if(lockedTarget == playerCenter)  // player is more important
            return;

        GameObject[] targets = GameObject.FindGameObjectsWithTag("Turret");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestTarget = null;

        foreach (GameObject target in targets) {
            float distanceToEnemy = Vector3.Distance(sightCenter.position, target.transform.position);
            if (distanceToEnemy < shortestDistance) {
                shortestDistance = distanceToEnemy;
                nearestTarget = target;
            }
        }

        if (nearestTarget != null && shortestDistance <= sightRange) {
            LockTarget(nearestTarget.transform);
        } else {
            LockTarget(playerBase);
        }
    }

    private void LockTarget(Transform target) {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, (target.position - transform.position));
        Debug.DrawRay(transform.position, (target.position - transform.position), Color.green);

        if(Physics.Raycast(ray, out hit)) {
            if (hit.collider.tag == "Player" || hit.collider.tag == target.tag) {
                lockedTarget = target;
                lastKnownPosition = target.position;
                targetLost = false;
            } else {
                // target lost
                if(lockedTarget == playerCenter)
                    targetLost = true;
            }
        }      
    }

    private void TargetLost() {
        lockedTarget = playerBase;
    }

    private void ChaseTarget(Vector3 position) {
        agent.SetDestination(position);
        animator.SetBool("Walking", true);

        if(stepSound != null && stepSoundCounter < 0) {
            stepSound.Play();
            stepSoundCounter = stepSoundTime;
        }
    }
    
    private void EnemyAttack() {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, (lockedTarget.position - transform.position));
        Debug.DrawRay(transform.position, (lockedTarget.position - transform.position), Color.red);

        if (Physics.Raycast(ray, out hit)) {
            if (hit.collider.tag == "Player" || hit.collider.tag == lockedTarget.tag) {
                agent.SetDestination(transform.position);

                LookAtTarget(lockedTarget);

                enemyAttack.Attack(lockedTarget);
            } else {
                ChaseTarget(lockedTarget.position);
            }
        }
    }

    private void LookAtTarget(Transform target) {
        var lookPos = target.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * lookAtDamp);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(sightCenter.position, sightRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, hearRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyAttack.attackRange);

        if (lockedTarget != null)
            Gizmos.DrawRay(transform.position, (lockedTarget.position - transform.position));
    }
}
