using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void StartLevel(int levelNumber) {
        SceneManager.LoadScene(levelNumber);
    }

    public void ExitGame() {
        Application.Quit();
    }
}
