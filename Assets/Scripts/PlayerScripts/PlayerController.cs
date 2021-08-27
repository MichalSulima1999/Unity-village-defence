 using UnityEngine;
 using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float rotationSpeed = 0.8f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform barrelTransform;
    [SerializeField] private Transform bulletParent;
    [SerializeField] private float bulletHitMissDistance = 25f;

    [SerializeField] private float animationSmoothTime = 0.1f;
    [SerializeField] private float animationPlayTransition = 0.15f;
    [SerializeField] private Transform aimTarget;
    [SerializeField] private float aimDistance = 1f;
    [SerializeField] private float timeBetweenShooting = 0.25f;

    [SerializeField] private GameObject knifeAttackPrefab;
    [SerializeField] private Transform knifeAttackPosition;
    [SerializeField] private float timeBetweenKnifeAttack = 0.5f;
    private float attackCounter;

    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private GranadeAttack granadeAttack;
    [SerializeField] private PauseMenu pauseMenu;
    private BuildMode buildMode;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private PlayerInput playerInput;
    private Transform cameraTransform;

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction shootAction;
    private InputAction knifeAttackAction;
    private InputAction crouchAction;
    private InputAction nextRoundAction;
    private InputAction reload;
    private InputAction granadeThrow;
    private InputAction pauseAction;

    private Animator animator;
    int jumpAnimation;
    int recoilAnimation;
    int knifeAttackAnimation;
    int reloadAnimation;
    int granadeThrowAnimation;
    int moveXAnimationParameterId;
    int moveZAnimationParameterId;

    Vector2 currentAnimationBlendVector;
    Vector2 animationVelocity;

    [HideInInspector] public bool crouching = false;
    [HideInInspector] public bool aiming = false;

    public UnityEvent recoil;

    private void Awake() {
        buildMode = GetComponent<BuildMode>();

        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Movement"];
        jumpAction = playerInput.actions["Jump"];
        shootAction = playerInput.actions["Shoot"];
        knifeAttackAction = playerInput.actions["KnifeAttack"];
        crouchAction = playerInput.actions["Crouch"];
        nextRoundAction = playerInput.actions["NextRound"];
        reload = playerInput.actions["Reload"];
        granadeThrow = playerInput.actions["ThrowGranade"];
        pauseAction = playerInput.actions["Pause"];

        cameraTransform = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked;

        //Animation
        animator = GetComponent<Animator>();
        jumpAnimation = Animator.StringToHash("Jump");
        moveXAnimationParameterId = Animator.StringToHash("MoveX");
        moveZAnimationParameterId = Animator.StringToHash("MoveZ");
        recoilAnimation = Animator.StringToHash("pistolShootRecoil");
        knifeAttackAnimation = Animator.StringToHash("Attack knife");
        reloadAnimation = Animator.StringToHash("PistolReload");
        granadeThrowAnimation = Animator.StringToHash("GranadeThrow");
    }

    private void OnEnable() {
        shootAction.performed += _ => ShootGun();
        knifeAttackAction.performed += _ => KnifeAttack();
        nextRoundAction.performed += _ => enemySpawner.NextRound();
        reload.performed += _ => playerStats.Reload();
        granadeThrow.performed += _ => ThrowGranade();
        pauseAction.performed += _ => pauseMenu.ChangeState();

        crouchAction.performed += _ => StartCrouch();
        crouchAction.canceled += _ => CancelCrouch();
    }

    private void OnDisable() {
        shootAction.performed -= _ => ShootGun();
        knifeAttackAction.performed -= _ => KnifeAttack();
        nextRoundAction.performed -= _ => enemySpawner.NextRound();
        reload.performed -= _ => playerStats.Reload();
        granadeThrow.performed -= _ => ThrowGranade();
        pauseAction.performed -= _ => pauseMenu.ChangeState();

        crouchAction.performed -= _ => StartCrouch();
        crouchAction.canceled -= _ => CancelCrouch();
    }

    void Update() {
        if (GameManager.GameOver || GameManager.ControlsLocked)
            return;

        aimTarget.position = cameraTransform.position + cameraTransform.forward * aimDistance;

        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0) {
            playerVelocity.y = 0f;
        }

        Vector2 input = moveAction.ReadValue<Vector2>();
        currentAnimationBlendVector = Vector2.SmoothDamp(currentAnimationBlendVector, input, ref animationVelocity, animationSmoothTime);
        Vector3 move = new Vector3(currentAnimationBlendVector.x, 0, currentAnimationBlendVector.y);
        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
        move.y = 0f;
        controller.Move(move * Time.deltaTime * playerSpeed);
        // Blend Strafe Animation
        if (groundedPlayer) {
            animator.SetFloat(moveXAnimationParameterId, currentAnimationBlendVector.x);
            animator.SetFloat(moveZAnimationParameterId, currentAnimationBlendVector.y);
        } else {
            animator.SetFloat(moveXAnimationParameterId, 0f);
            animator.SetFloat(moveZAnimationParameterId, 0f);
        }

        // Changes the height position of the player..
        if (jumpAction.triggered && groundedPlayer) {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            animator.CrossFade(jumpAnimation, animationPlayTransition);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        // Rotate towards camera direction
        Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        attackCounter -= Time.deltaTime;
    }

    private void ShootGun() {
        if (buildMode.inBuildMode || attackCounter > 0 || !playerStats.HasBullets() || GameManager.ControlsLocked)
            return;

        RaycastHit hit;
        GameObject bullet = GameObject.Instantiate(bulletPrefab, barrelTransform.position, barrelTransform.rotation, bulletParent);
        BulletController bulletController = bullet.GetComponent<BulletController>();
        bulletController.damage = playerStats.pistolDamage;

        Vector3 randomVector = new Vector3(0, 0, 0);
        if (aiming) {
            bulletController.damage = (int)(playerStats.pistolDamage * 1.5f);
        }

        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity)) {
            bulletController.target = hit.point + randomVector;
            bulletController.hit = true;
        } else {
            bulletController.target = cameraTransform.position + cameraTransform.forward * bulletHitMissDistance + randomVector;
            bulletController.hit = false;
        }
        animator.CrossFade(recoilAnimation, animationPlayTransition);

        playerStats.currentBullets -= 1;
        playerStats.UpdateBulletText();

        recoil.Invoke();
        playerStats.sFXManager.PlayShootPlayer(transform);
        attackCounter = timeBetweenShooting;
    }

    private void ThrowGranade() {
        if (buildMode.inBuildMode || attackCounter > 0 || !playerStats.HasGranades() || GameManager.ControlsLocked)
            return;

        animator.CrossFade(granadeThrowAnimation, animationPlayTransition);
        granadeAttack.ThrowGranade();

        attackCounter = granadeAttack.cooldown;
    }

    public void PlayReloadAnim() {
        animator.CrossFade(reloadAnimation, animationPlayTransition);
    }

    private void KnifeAttack() {
        if (buildMode.inBuildMode || attackCounter > 0 || GameManager.ControlsLocked)
            return;

        animator.CrossFade(knifeAttackAnimation, animationPlayTransition);
        GameObject knifeAttack = Instantiate(knifeAttackPrefab, knifeAttackPosition.position, knifeAttackPosition.rotation);
        knifeAttack.GetComponent<KnifeAttack>().damage = playerStats.knifeDamage;

        Destroy(knifeAttack, 0.1f);

        attackCounter = timeBetweenKnifeAttack;
    }

    public void StartCrouch() {
        crouching = true;
        playerSpeed /= 2;
        animator.SetBool("Crouching", crouching);
    }

    public void CancelCrouch() {
        crouching = false;
        playerSpeed *= 2;
        animator.SetBool("Crouching", crouching);
    }
}
