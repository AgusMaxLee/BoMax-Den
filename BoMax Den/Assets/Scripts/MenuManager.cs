using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    PlayerControls controls;
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;
    private void Awake()
    {
        controls = new PlayerControls();
    }

    void Update()
    {
        Scene scene = SceneManager.GetActiveScene();


        if (Input.GetKeyDown(KeyCode.Escape) && scene.name != "Menu")
        {
            if (gameIsPaused)
            {

                Resume();

            }
            else
            {

                Pause();

            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        controls.Player.Enable();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        controls.Player.Disable();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameIsPaused = true;
    }

    public void LoadMenu()
    {
        Debug.Log("Loading Menu Scene...");
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }

    public void EnterGame()
    {
        Debug.Log("Loading Menu Scene...");
        SceneManager.LoadScene("MainTest");
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }
}
