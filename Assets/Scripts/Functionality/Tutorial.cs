using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Tutorial : MonoBehaviour, IPointerDownHandler
{

    [SerializeField] private GameObject tooltip;
    [SerializeField] private GameObject arrowHelper;
    [SerializeField] private TextMeshProUGUI energy;
    [SerializeField] private TextMeshProUGUI tooltipText;
    private Button playBtn;
    private Button archerBtn;
    private Button cannonBtn;
    private Button farmBtn;
    private bool moved;
    private int xCoord;
    private int yCoord;
    private int rotation;
    private float waitTime;
    private bool wait;
    private int step;
    public AudioSource beep;

    public bool isTutorial;

    // Start is called before the first frame update
    void Start()
    {
        if (isTutorial)
        {

            tooltipText = tooltip.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            playBtn = GameObject.Find("Play").GetComponent<Button>();
            archerBtn = GameObject.Find("TowerSelect").transform.GetChild(1).GetComponent<Button>();
            cannonBtn = GameObject.Find("TowerSelect").transform.GetChild(2).GetComponent<Button>();
            farmBtn = GameObject.Find("TowerSelect").transform.GetChild(3).GetComponent<Button>();
            tooltip.SetActive(false);
            step = 0;
            playBtn.interactable = false;
            cannonBtn.interactable = false;
            archerBtn.interactable = false;
            farmBtn.interactable = false;
            moved = false;
            WaitForUserInput("Welcome to the Kingdom TD Tutorial. Press E to continue");
            waitTime = 3f;
            xCoord = 1191;
            yCoord = 900;
            rotation = 270;
            arrowHelper.SetActive(false);
        }

    }

    void PlayBeep()
    {
        beep.Play();
    }
    // Update is called once per frame
    void Update()
    { // 138 129
        if (isTutorial)
        {
            arrowHelper.transform.position = new Vector2(xCoord, yCoord);
            arrowHelper.transform.rotation = Quaternion.Euler(0, 0, rotation);
            if (wait)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    wait = false;
                    step++;
                    Invoke("PlayBeep", 0.00001f);
                }
            }
            else
            {
                if (step == 1)
                { // Move Around
                    if (moved)
                    {
                        waitTime -= Time.deltaTime;
                        if (waitTime <= 0) { step++; waitTime = 5f; Invoke("PlayBeep", 0.00001f); }
                    }
                    tooltipText.text = "Press the W A S D keys to move Halstein around";
                    if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
                    {
                        moved = true;
                    }
                }
                else if (step == 2)
                { // Halstein Attack
                    tooltipText.text = "Halstein can attack enemies as well. Press space to attack when close to an enemy";
                    if (Input.GetKeyDown(KeyCode.Space)) { step++; Invoke("PlayBeep", 0.00001f); }
                }
                else if (step == 3)
                {
                    arrowHelper.SetActive(true);
                    wait = true;
                    WaitForUserInput("Halstein's energy depletes when he attacks. Press E to continue");
                }
                else if (step == 4)
                {
                    xCoord = 1500;
                    yCoord = 775;
                    rotation = 180;
                    arrowHelper.SetActive(true);
                    tooltipText.text = "Hastein's energy also depletes when he places towers. Try placing an archer tower.";
                    archerBtn.interactable = true;
                    if (GameObject.Find("pfArcherTower(Clone)") != null)
                    {
                        step++;
                        Invoke("PlayBeep", 0.00001f);
                    }
                }
                else if (step == 5)
                {
                    arrowHelper.SetActive(false);
                    tooltipText.text = "Left click to place the tower, or right click to cancel. Towers can't be placed on paths or on top of other towers.";
                    waitTime -= Time.deltaTime;
                    if (waitTime <= 0) { step++; waitTime = 5f; Invoke("PlayBeep", 0.00001f); }
                }
                else if (step == 6)
                {
                    xCoord = 1365;
                    yCoord = 200;
                    rotation = 90;
                    playBtn.interactable = true;
                    wait = true;
                    arrowHelper.SetActive(true);
                    WaitForUserInput("Enemies will spawn when you press the play button. Whenever you feel ready press it to start wave 1. Press E to continue");
                }
                else if (step == 7)
                {
                    arrowHelper.SetActive(false);
                    tooltipText.text = "When enemies die, Halstein gets energy back. He also gets energy back at the end of each round.";
                    waitTime -= Time.deltaTime;
                    if (waitTime <= 0) { step++; waitTime = 3f; Invoke("PlayBeep", 0.00001f); }
                }
                else if (step == 8)
                {
                    wait = true;
                    WaitForUserInput("There are a few enemy types. Normal, Aerial, Camo, and Aerial Camo. Press E to continue");
                }
                else if (step == 9)
                {
                    wait = true;
                    WaitForUserInput("Halstein can kill both normal & camo enemies with his sword. But not Aerial ones. Press E to continue");
                }
                else if (step == 10)
                {
                    wait = true;
                    WaitForUserInput("Some towers can see camo enemies, some can't. However, some towers can be upgraded to see them. Press E to continue");
                    moved = false;
                }
                else if (step == 11)
                {
                    tooltipText.text = "Click on the archer tower you placed earlier.";
                    waitTime -= Time.deltaTime;
                    if (waitTime <= 0)
                    {
                        step++;
                        Invoke("PlayBeep", 0.00001f);
                        int old_energy;
                        int.TryParse(energy.GetComponent<TextMeshProUGUI>().text, out old_energy);
                        old_energy += 1000;
                        energy.text = old_energy.ToString();
                        waitTime = 5f;
                    }
                }
                else if (step == 12)
                {


                    tooltipText.text = "Click the upgrade button to increase it's damage, range, speed, and to let it see camo enemies.";
                    if (GameObject.Find("pfArcherTower(Clone)").GetComponent<KTD_Archer>().level == 2)
                    {
                        step++;
                        int old_energy;
                        int.TryParse(energy.GetComponent<TextMeshProUGUI>().text, out old_energy);
                        old_energy += 200;
                        energy.text = old_energy.ToString();
                        Invoke("PlayBeep", 0.00001f);
                    }
                }
                else if (step == 13)
                {
                    tooltipText.text = "Towers aren't all that can be upgraded. Click on Halstein to increase his damage, speed, and energy regeneration.";
                    if (moved)
                    {
                        moved = false;

                    }
                    if (GameObject.Find("Halstein").GetComponent<KTD_Halstein>().level == 2) { Invoke("PlayBeep", 0.00001f); step++; }
                }
                else if (step == 14)
                {
                    tooltipText.text = "You are able to toggle the speed of the game by pressing F. Going from 1 to 1.5x and vice versa";
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        step++;
                        Invoke("PlayBeep", 0.00001f);
                    }
                }
                else if (step == 15)
                {
                    tooltipText.text = "Lastly, you can press M to toggle viewing the whole map.";
                    if (Input.GetKeyDown(KeyCode.M))
                    {
                        step++;
                        Invoke("PlayBeep", 0.00001f);
                    }
                }
                else if (step == 16)
                {
                    wait = true;
                    WaitForUserInput("Congrats, and welcome to Kingdom TD! You can stay here and play, or return to the main menu with the back button. Enjoy! Press E to continue");
                }
                else if (step == 17)
                {
                    tooltip.SetActive(false);
                    cannonBtn.interactable = true;
                    archerBtn.interactable = true;
                    farmBtn.interactable = true;
                }
            }
        }


    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (this.gameObject.name == "Tutorial")
        {
            SceneManager.LoadScene(2);
        }
        else if (this.gameObject.name == "Back to Menu")
        {
            SceneManager.LoadScene(0);
        }
    }

    void WaitForUserInput(string txt)
    {
        tooltipText.text = txt;
        tooltip.SetActive(true);
        wait = true;
    }
}
