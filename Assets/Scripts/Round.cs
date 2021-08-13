using UnityEngine;

[System.Serializable]
public class Round
{
    public int numberOfEnemiesPerWave;
    public int numberOfWaves;
    public float timeBetweenWaves;
    public GameObject[] enemyTypes;
}
