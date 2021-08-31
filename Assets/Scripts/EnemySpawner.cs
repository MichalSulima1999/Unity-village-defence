using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] Round[] rounds;
    [SerializeField] Text nextRoundText;
    [SerializeField] GameObject shop;

    private int currentRound = -1;
    private int waveCounter = 0;
    private bool canPlayNextRound = false;

    private int enemiesLeft = 0;

    private PlayerInput playerInput;
    private InputAction shopAction;

    private void Awake() {
        playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();

        shopAction = playerInput.actions["Shop"];
    }

    private void OnEnable() {
        shopAction.performed += _ => OpenShop();
    }

    private void OnDisable() {
        shopAction.performed -= _ => OpenShop();
    }

    void OpenShop() {
        if (canPlayNextRound) {
            if (!Shop.ShopActivated)
                shop.SetActive(true);
            else
                shop.SetActive(false);
        }
    }

    public void NextRound() {
        if(!canPlayNextRound || shop.activeSelf)
            return;

        currentRound++;
        waveCounter = 0;
        enemiesLeft = rounds[currentRound].numberOfWaves * rounds[currentRound].numberOfEnemiesPerWave;

        StartCoroutine(StartWaves());
    }

    private void Update() {
        if (GameManager.Won)
            return;

        if (currentRound >= rounds.Length - 1 && GameManager.enemiesLeft.Length <= 0) {
            GameManager.Won = true;
            return;
        }   

        if (enemiesLeft <= 0 && GameManager.enemiesLeft.Length <= 0 && currentRound < rounds.Length - 1) {
            canPlayNextRound = true; 
        }
        else 
            canPlayNextRound = false;
            

        if (canPlayNextRound) {
            nextRoundText.enabled = true;
        } else {
            nextRoundText.enabled = false;
            shop.SetActive(false);
        }
    }

    IEnumerator SpawnWave() {
        for (int i = 0; i < rounds[currentRound].numberOfEnemiesPerWave; i++) {
            SpawnEnemy();
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator StartWaves() {
        for (int i = 0; i < rounds[currentRound].numberOfWaves; i++) {
            StartCoroutine(SpawnWave());

            waveCounter++;
            Debug.Log("Wave counter: " + waveCounter + " numberOfWaves: " + rounds[currentRound].numberOfWaves);

            yield return new WaitForSeconds(rounds[currentRound].timeBetweenWaves);
        }
    }

    void SpawnEnemy() {
        int randomSpawnerIndex = Random.Range(0, spawnPoints.Length);
        int randomEnemyIndex = Random.Range(0, rounds[currentRound].enemyTypes.Length);
        Instantiate(rounds[currentRound].enemyTypes[randomEnemyIndex], spawnPoints[randomSpawnerIndex].position, spawnPoints[randomSpawnerIndex].rotation);
        enemiesLeft--;
    }
}
