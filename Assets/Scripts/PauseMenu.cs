using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject PauseMenuUI;

    public void ChangeState() {
        if (GameIsPaused)
            Resume();
        else
            Pause();
    }

    public void Resume() {
        PauseMenuUI.SetActive(false);
        GameIsPaused = false;
        GameManager.ControlsLocked = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
    }

    void Pause() {
        PauseMenuUI.SetActive(true);
        GameIsPaused = true;
        GameManager.ControlsLocked = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
    }

    public void Quit() {
        Application.Quit();
    }
}
