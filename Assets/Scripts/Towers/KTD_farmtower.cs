using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KTD_farmtower : MonoBehaviour
{
    // Start is called before the first frame update

    private bool pauseState;

    public int increase;

    public int level;

    public Transform UIParent;

    private Transform GameUI;

    private string upgrading;

    private Button backBtn, upgradeBtn;

    private string upgrade_cost_text;


    public TextMeshProUGUI halstein_energy;

    public int energy;

    private bool firstClick;

    private Button sellButton;

    private TextMeshProUGUI sellText;

    private int old_cost;
    public AudioSource upgradeSound;


    void Start()
    {
        upgradeSound = GameObject.Find("UI").GetComponent<AudioSource>();
        increase = 50;
        firstClick = true;
        level = 1;
        energy = 800;
        GameUI = GameObject.Find("UI").transform;
        UIParent = GameUI.GetChild(1);
        upgrade_cost_text = (energy.ToString() + " ENERGY");
        upgrading = "Level: " + level.ToString() + "\nProduces: " + increase.ToString() + "E\n\nIncreases production to 60 Energy per round.\n\n";
        backBtn = UIParent.transform.GetChild(2).GetComponent<Button>();
        upgradeBtn = UIParent.transform.GetChild(1).GetChild(0).GetComponent<Button>();
        backBtn.onClick.AddListener(hideUpgradeMenu);
        upgradeBtn.onClick.AddListener(UpgradeTower);
        this.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        this.gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        old_cost = 1125;
        halstein_energy = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<TextMeshProUGUI>();
        UIParent.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>().sprite = this.gameObject.GetComponent<SpriteRenderer>().sprite;
        sellButton = UIParent.transform.GetChild(3).GetComponent<Button>();
        sellText = UIParent.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            pauseState = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<KTD_Pause>().gameIsPaused;
            if (firstClick == true)
            {
                firstClick = false;
            }
            else
            {
                Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mouse2D = new Vector2(mouse.x, mouse.y);

                // Casts the ray and get the first game object hit
                RaycastHit2D hit = Physics2D.Raycast(mouse2D, Vector2.zero);
                try
                {
                    if (hit.collider.gameObject == this.gameObject && firstClick == false && pauseState == false)
                    {
                        if (level == 6)
                        {
                            UIParent.transform.GetChild(1).GetChild(0).GetChild(0).gameObject.SetActive(false);
                            UIParent.transform.GetChild(1).GetChild(2).gameObject.SetActive(false);
                            UIParent.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
                            UIParent.transform.GetChild(1).GetComponent<Image>().enabled = false;
                        }
                        else
                        {
                            try
                            {
                                upgradeBtn.onClick.RemoveListener(UpgradeTower);
                                sellButton.onClick.RemoveListener(sellTower);
                            }
                            catch { }
                            UIParent.transform.GetChild(1).GetChild(0).GetChild(0).gameObject.SetActive(true);
                            UIParent.transform.GetChild(1).GetChild(2).gameObject.SetActive(true);
                            sellText.text = "+" + ((int)(old_cost * .75)).ToString() + "E";
                            UIParent.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
                            UIParent.transform.GetChild(1).GetChild(0).GetChild(0).localScale = new Vector3(2f, 2f, 0f);
                            UIParent.transform.GetChild(1).GetComponent<Image>().enabled = true;
                        }
                        UIParent.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>().sprite = this.gameObject.GetComponent<SpriteRenderer>().sprite;
                        UIParent.gameObject.SetActive(true);
                        UIParent.transform.GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>().text = upgrade_cost_text;
                        UIParent.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = upgrading;
                        upgradeBtn.onClick.AddListener(UpgradeTower);
                        sellButton.onClick.AddListener(sellTower);

                    }
                    else
                    {
                        try
                        {
                            upgradeBtn.onClick.RemoveListener(UpgradeTower);
                            sellButton.onClick.RemoveListener(sellTower);
                        }
                        catch { }
                    }
                }
                catch { }

            }
        }
    }





    public void UpgradeTower()
    {
        int old_energy;
        int.TryParse(halstein_energy.text, out old_energy);
        if (old_energy >= energy)
        {
            upgradeSound.Play();
            int new_eng = old_energy - energy;
            halstein_energy.text = new_eng.ToString();
            if (level <= 5)
            {
                level++;
                old_cost = energy;
                energy += 150;
                increase += 30;
                upgrading = "Level: " + level.ToString() + "\nProduces: " + increase.ToString() + "E\n\nIncreases production to "
                + (increase + 30).ToString() + " Energy per round.\n\n";
                upgrade_cost_text = energy.ToString() + " ENERGY";
            }
            if (level == 6)
            {
                increase += 30;
                upgrading = "Level: " + level.ToString() + "\nProduces: " + increase.ToString() + "E\n\nNo More Upgrades.";
                UIParent.transform.GetChild(1).GetChild(0).GetChild(0).gameObject.SetActive(false);
                UIParent.transform.GetChild(1).GetChild(2).gameObject.SetActive(false);
                UIParent.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
                UIParent.transform.GetChild(1).GetComponent<Image>().enabled = false;

            }

        }
        else
        {
            GameUI.GetChild(0).GetChild(6).gameObject.SetActive(true);
            Invoke("stopShowingEnergy", 2f);
        }
        hideUpgradeMenu();
    }

    public void hideUpgradeMenu()
    {
        upgradeBtn.onClick.RemoveListener(UpgradeTower);
        sellButton.onClick.RemoveListener(sellTower);
        UIParent.gameObject.SetActive(false);
    }

    public void sellTower()
    {
        int old_energy, new_energy;
        int.TryParse(halstein_energy.text, out old_energy);
        new_energy = old_energy + (int)(old_cost * 0.75);
        halstein_energy.text = new_energy.ToString();
        Destroy(this.gameObject);
        hideUpgradeMenu();
    }

    void stopShowingEnergy()
    {
        GameUI.GetChild(0).GetChild(6).gameObject.SetActive(false);
    }
}
