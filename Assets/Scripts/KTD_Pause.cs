using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KTD_Pause : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject gameUI;

    public GameObject optionsUI;
    // Update is called once per frame
    void Update()
    {
        Debug.Log(gameIsPaused);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
            Debug.Log("Hello.");
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
    }
}
