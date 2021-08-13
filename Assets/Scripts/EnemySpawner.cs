using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] Round[] rounds;
    [SerializeField] Text nextRoundText;
    [SerializeField] GameObject shop;

    private int currentRound = -1;
    private int waveCounter = 0;
    private bool wavesEnded = true;
    private bool canPlayNextRound = false;
    private bool shopActivated = false;

    public void NextRound() {
        if(!canPlayNextRound || shop.activeSelf)
            return;

        currentRound++;
        waveCounter = 0;
        shopActivated = false;
        wavesEnded = false;

        StartCoroutine(StartWaves());
    }

    private void Update() {
        if (currentRound >= rounds.Length - 1 && wavesEnded && GameManager.enemiesLeft.Length <= 0)
            GameManager.Won = true;

        if (GameManager.Won)
            return;

        if (wavesEnded && GameManager.enemiesLeft.Length <= 0 && currentRound < rounds.Length - 1) {
            canPlayNextRound = true; 
        }
        else 
            canPlayNextRound = false;
            

        if (canPlayNextRound) {
            nextRoundText.enabled = true;

            if(!shopActivated)
                shop.SetActive(true);
            shopActivated = true;
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

            wavesEnded = false;
            waveCounter++;
            if (waveCounter >= rounds[currentRound].numberOfWaves)
                wavesEnded = true;
            Debug.Log("Wave counter: " + waveCounter + " numberOfWaves: " + rounds[currentRound].numberOfWaves);

            yield return new WaitForSeconds(rounds[currentRound].timeBetweenWaves);
        }
    }

    void SpawnEnemy() {
        int randomSpawnerIndex = Random.Range(0, spawnPoints.Length);
        int randomEnemyIndex = Random.Range(0, rounds[currentRound].enemyTypes.Length);
        Instantiate(rounds[currentRound].enemyTypes[randomEnemyIndex], spawnPoints[randomSpawnerIndex].position, spawnPoints[randomSpawnerIndex].rotation);
    }
}
