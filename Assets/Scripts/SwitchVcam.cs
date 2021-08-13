using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class SwitchVcam : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private int priorityBoostAmount = 10;

    [SerializeField] private Canvas thirdPersonCanvas;
    [SerializeField] private Canvas aimCanvas;

    private PlayerController playerController;

    private CinemachineVirtualCamera virtualCamera;
    private InputAction aimAction;

    private void Awake() {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        aimAction = playerInput.actions["Aim"];

        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnEnable() {
        aimAction.performed += _ => StartAim();
        aimAction.canceled += _ => CancelAim();

        aimAction.performed += _ => playerController.StartCrouch();
        aimAction.canceled += _ => playerController.CancelCrouch();
    }

    private void OnDisable() {
        aimAction.performed -= _ => StartAim();
        aimAction.canceled -= _ => CancelAim();

        aimAction.performed -= _ => playerController.StartCrouch();
        aimAction.canceled -= _ => playerController.CancelCrouch();
    }

    private void StartAim() {
        virtualCamera.Priority += priorityBoostAmount;
        aimCanvas.enabled = true;
        thirdPersonCanvas.enabled = false;
        playerController.aiming = true;
    }

    private void CancelAim() {
        virtualCamera.Priority -= priorityBoostAmount;
        aimCanvas.enabled = false;
        thirdPersonCanvas.enabled = true;
        playerController.aiming = false;
    }
}
