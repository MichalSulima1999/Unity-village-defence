using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildMode : MonoBehaviour
{
    [SerializeField] GameObject towerBlueprint;
    [SerializeField] GameObject tower;
    [SerializeField] Transform turretSlot;
    [SerializeField] Material blueprintMaterial;

    [SerializeField] float buildRange = 15f;
    [SerializeField] int towerCost = 50;

    [SerializeField] PlayerStats playerStats;

    [SerializeField] private PlayerInput playerInput;
    private InputAction buildAction;
    private InputAction shootAction;
    public bool inBuildMode { get; set; } = false;

    private GameObject turret;
    private TowerBlueprint towerBlueprintScript;
    private Transform cameraTransform;
    

    // Start is called before the first frame update
    void Start()
    {
        buildAction = playerInput.actions["BuildMode"];
        shootAction = playerInput.actions["Shoot"];

        cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (buildAction.triggered) {
            if (inBuildMode) {
                LeaveBuildMode();
            } else {
                EnterBuildMode();
            }
        }

        if (inBuildMode) {
            RaycastHit hit;
            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, buildRange)) {
                turret.transform.position = hit.point;
            }
            if (towerBlueprintScript.canBuild && playerStats.GetMoney() >= towerCost) {
                blueprintMaterial.color = Color.green;
                if (shootAction.triggered) {
                    BuildTower();
                }
                    
            } else {
                blueprintMaterial.color = Color.red;
            }
        }
    }

    void BuildTower() {
        GameObject builtTower = Instantiate(tower, turret.transform.position, Quaternion.identity);
        GameObject towerHead = null;
        foreach (Transform child in builtTower.transform) {
            if (child.CompareTag("Turret")) {
                towerHead = child.gameObject;
                break;
            }
        }

        towerHead.GetComponent<Tower>().damage = playerStats.towerDamage;
        towerHead.GetComponent<Tower>().maxHealth = playerStats.towerHealth;
        towerHead.GetComponent<Tower>().attackRange = playerStats.towerAttackRange;
        towerHead.GetComponent<Tower>().shootingSpeed = playerStats.towerAttackSpeed;
        playerStats.CollectMoney(-towerCost);
    }

    void EnterBuildMode() {
        Debug.Log("Enter Build Mode");
        inBuildMode = true;
        turret = Instantiate(towerBlueprint, turretSlot.position, Quaternion.identity);
        towerBlueprintScript = turret.GetComponent<TowerBlueprint>();
    }

    void LeaveBuildMode() {
        Debug.Log("Leave Build Mode");
        inBuildMode = false;
        if (turret != null)
            Destroy(turret);
    }
}
