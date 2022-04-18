using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class KTD_Pause : MonoBehaviour
{
    public bool gameIsPaused = false;
    public static bool quitPause = true;
    public GameObject pauseMenuUI;
    public GameObject gameUI;
    public GameObject lostUI;
    public GameObject optionsUI;
    public GameObject halsteinUpgradeUI;
    // Update is called once per frame
    void Update()
    {

        if (gameIsPaused == false && quitPause == true) {
            resume();
            quitPause = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
            if (gameIsPaused) 
            {  
                resume();
            } else 
            {   
                pause();
            }
            if (optionsUI.activeSelf == true)
            {
                optionsUI.SetActive(false);
                resume();

            }
        }
    }


    public void resume() {
        pauseMenuUI.SetActive(false);
        gameUI.SetActive(true);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    public void pause() {
        gameUI.SetActive(false);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
        halsteinUpgradeUI.SetActive(false);
    }

    public void quit() {
        gameUI.SetActive(true);
        pauseMenuUI.SetActive(false);
        gameIsPaused = false;
        quitPause = true;
        halsteinUpgradeUI.SetActive(false);
        SceneManager.LoadScene(0);
    }
}
