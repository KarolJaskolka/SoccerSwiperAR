using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuActions : MonoBehaviour
{

    public GameObject pauseMenuUI;

    public void Start()
    {
        pauseMenuUI.SetActive(false);
    }
    

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
    }

    public void RedirectToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
