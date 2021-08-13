using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private GameObject playerBase;
    static public bool GameOver = false;
    static public bool Won = false;
    static public bool ControlsLocked = true;
    static public int Rounds;
    static public GameObject[] enemiesLeft;

    [SerializeField] Canvas winCanvas;

    // Start is called before the first frame update
    void Start()
    {
        playerBase = GameObject.FindGameObjectWithTag("Base");

        InvokeRepeating("CountEnemies", 0f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if(playerBase == null) {
            GameOver = true;
            Debug.Log("GameOver");
        }

        if (Won) {
            winCanvas.enabled = true;
        }
    }

    void CountEnemies() {
        enemiesLeft = GameObject.FindGameObjectsWithTag("Enemy");
    }
}
