using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class Control : MonoBehaviour, IPointerDownHandler
{

    private bool firstStart = true;

    public Camera cam;

    public Camera overview;

    private bool overview_mode;

    public GameObject loadingScreen;

    public Slider loadingBar;

    private float waitTime;

    private bool isDone;

    private bool autoStart;

    private Transform speed;


    public void Exit()
    {
        Application.Quit();
    }

    void Start()
    {
        overview_mode = false;
        isDone = false;
        waitTime = 2f;
        cam = Camera.main;
        try{
            speed = GameObject.Find("UI").transform.GetChild(0).transform.GetChild(8);
        }
        catch{}
        if (firstStart == true)
        {
            PlayerPrefs.SetInt("Map", 0);
        }
        firstStart = false;
        autoStart = false;
    }


    void Update()
    {
        try
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (Time.timeScale == 1.5f)
                {
                    Time.timeScale = 1f;
                    speed.gameObject.SetActive(false);
                }
                else
                {
                    Time.timeScale = 1.5f;
                    speed.gameObject.SetActive(true);
                }
            }
        }
        catch
        {

        }

        try
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                if (!overview_mode)
                {
                    overview_mode = true;
                    cam.enabled = false;
                    overview.enabled = true;
                }
                else
                {
                    overview_mode = false;
                    cam.enabled = true;
                    overview.enabled = false;
                }
            }
        }
        catch
        {

        }

        try
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                if (autoStart == false)
                {
                    autoStart = true;
                }
                else
                {
                    autoStart = false;
                }
            }
        }
        catch { }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (this.gameObject.name == "ARTUS")
        {
            PlayerPrefs.SetInt("Map", 1);
        }
        else if (this.gameObject.name == "DEMIA")
        {
            PlayerPrefs.SetInt("Map", 3);
        }
        else if (this.gameObject.name == "TIBER")
        {
            PlayerPrefs.SetInt("Map", 4);
        }
    }

    public void LoadScreen()
    {
        int val = PlayerPrefs.GetInt("Map");
        StartCoroutine(LoadSceneAsynchronously(val));
    }

    private IEnumerator LoadSceneAsynchronously(int lvl)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(lvl);
        operation.allowSceneActivation = false;
        loadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            loadingBar.value = progress;
            operation.allowSceneActivation = true;
            if (operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
