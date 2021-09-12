using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private GameObject playerBase;
    static public bool GameOver = false;
    static public bool Won = false;
    static public bool Lost = false;
    static public bool ControlsLocked = false;
    static public int Rounds;
    static public GameObject[] enemiesLeft;

    [SerializeField] Canvas winCanvas;
    [SerializeField] GameObject loseCanvas;

    // Start is called before the first frame update
    void Start()
    {
        playerBase = GameObject.FindGameObjectWithTag("Base");

        loseCanvas.SetActive(false);

        InvokeRepeating("CountEnemies", 0f, 0.5f);

        Time.timeScale = 1f;
        Won = false;
        Lost = false;
        ControlsLocked = false;
        GameOver = false;
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

        if (Lost)
            YouLost();
    }

    void CountEnemies() {
        enemiesLeft = GameObject.FindGameObjectsWithTag("Enemy");
    }

    public void YouLost() {
        loseCanvas.SetActive(true);
        ControlsLocked = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0.2f;
    }
}
