using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
public class Control : MonoBehaviour
{

    public void Exit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void StartMap()
    {
        SceneManager.LoadScene(1);
        // Loads Map 'ARTUS'
        Debug.Log("Load ARTUS");
        
    }
    public void OnSelect(BaseEventData eventData)
     {
        Debug.Log(this.gameObject.name + " was selected");
     }
}
