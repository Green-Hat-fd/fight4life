using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    [SerializeField]
    GameObject MainMenu;

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameIsPaused = !GameIsPaused;
        }

        if (GameIsPaused)
            Resume();
        else
            Pause();
    }

    public void Resume ()
    {
        MainMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    void Pause ()
    {
        MainMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void QuitGame()
    {
        Debug.Log("Uscito correttamente dal gioco");
        Application.Quit();
    }
}
