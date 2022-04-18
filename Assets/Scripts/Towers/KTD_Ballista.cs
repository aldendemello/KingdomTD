using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KTD_Ballista : MonoBehaviour
{
    private Vector3 ProjPosition;

    private Vector3 enemypos;

    private AudioSource arrow_launch;

    private int arrowCount;

    private int arrowLim;

    private bool firstClick;

    private bool pauseState;


    // Data

    public bool canAerial;

    public int level;

    public int damage;

    public float range;

    public Transform UIParent;

    private Transform GameUI;

    private Button backBtn, upgradeBtn;

    private Transform RangeObject;

    public int energy;

    public TextMeshProUGUI halstein_energy;

    private string upgrading;

    private string upgrade_cost_text;

    private float attack_speed;

    private Button sellButton;

    private TextMeshProUGUI sellText;


    private int old_cost;

    public AudioSource upgradeSound;

    // Start is called before the first frame update
    void Start()
    {
        arrowLim = 5;
        upgradeSound = GameObject.Find("UI").GetComponent<AudioSource>();
        arrowCount = 0;
        arrow_launch = this.gameObject.GetComponent<AudioSource>();
        this.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        firstClick = true;
        energy = 725;
        attack_speed = 1f;
        range = 7.5f;
        level = 1;
        damage = 4;
        GameUI = GameObject.Find("UI").transform;
        UIParent = GameUI.GetChild(1);
        upgrade_cost_text = (energy.ToString() + " ENERGY");
        upgrading = "Level: " + level.ToString() + "\n\nIncreases Range\nIncreases Damage\nIncreases Speed";
        backBtn = UIParent.transform.GetChild(2).GetComponent<Button>();
        upgradeBtn = UIParent.transform.GetChild(1).GetChild(0).GetComponent<Button>();
        backBtn.onClick.AddListener(hideUpgradeMenu);
        this.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        this.gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        halstein_energy = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<TextMeshProUGUI>();
        backBtn.onClick.AddListener(hideUpgradeMenu);
        RangeObject = this.gameObject.transform.GetChild(0);
        RangeObject.GetComponent<SpriteRenderer>().enabled = false;
        sellButton = UIParent.transform.GetChild(3).GetComponent<Button>();
        sellText = UIParent.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>();
        old_cost = 525;

    }

    void CallJavelin()
    {
        KTD_Javelin.Create(ProjPosition, enemypos, damage, canAerial);
        arrow_launch.Play();
        arrowCount++;
        CancelInvoke("CallJavelin");
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (canAerial)
            {
                enemypos = other.transform.position;
                enemypos.z = 2;
                InvokeRepeating("CallJavelin", attack_speed, 1f);
            }
            else
            {
                if (other.gameObject.GetComponent<KTD_Enemy>().isAerial == false)
                {
                    enemypos = other.transform.position;
                    enemypos.z = 2;
                    InvokeRepeating("CallJavelin", attack_speed, 1f);
                }
            }

        }
        else
        {
        }
    }

    // Update is called once per frame
    void Update()
    {
        ProjPosition = transform.Find("ProjPosition").position;
        if (Input.GetMouseButtonDown(0))
        {
            pauseState = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<KTD_Pause>().gameIsPaused;
            if (firstClick == true)
            {
                firstClick = false;
            }
            else
            {
                this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
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
                            UIParent.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
                            UIParent.transform.GetChild(1).GetChild(0).GetChild(0).localScale = new Vector3(2f, 1f, 0f);
                            UIParent.transform.GetChild(1).GetComponent<Image>().enabled = true;
                        }
                        UIParent.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>().sprite = this.gameObject.GetComponent<SpriteRenderer>().sprite;
                        RangeObject.GetComponent<SpriteRenderer>().enabled = true;
                        UIParent.gameObject.SetActive(true);
                        UIParent.transform.GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>().text = upgrade_cost_text;
                        UIParent.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = upgrading;
                        upgradeBtn.onClick.AddListener(UpgradeTower);
                        sellButton.onClick.AddListener(sellTower);
                        sellText.text = "+" + ((int)(old_cost * .75)).ToString() + "E";

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
            old_cost = energy;
            if (level <= 5)
            {
                level++;
                damage += 1;
                attack_speed -= 0.05f;
                range += 0.5f;
                energy += 150;
                this.gameObject.transform.GetChild(0).localScale = new Vector3(range, range, 5f);
                upgrading = "Level: " + level.ToString() + "\n\nIncreases Range\nIncreases Damage\nIncreases Speed";
                upgrade_cost_text = energy.ToString() + " ENERGY";
            }
            if (level == 6)
            {
                damage += 2;
                range += 1f;
                upgrading = "Level: " + level.ToString() + "\n\nNo More Upgrades.";
                this.gameObject.transform.GetChild(0).localScale = new Vector3(range, range, 5f);
                UIParent.transform.GetChild(1).GetChild(0).GetChild(0).gameObject.SetActive(false);
                UIParent.transform.GetChild(1).GetChild(2).gameObject.SetActive(false);
                UIParent.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
                UIParent.transform.GetChild(1).GetComponent<Image>().enabled = false;

            }
            if (level == 2)
            {
                upgrading = "Level: " + level.ToString() + "\n\nIncreases Range\nIncreases Damage\nSee Aerial Enemies";
            }
            if (level == 3)
            {
                canAerial = true;
            }


        }
        else
        {
            GameUI.GetChild(0).GetChild(6).gameObject.SetActive(true);
            Invoke("stopShowingEnergy", 2f);
        }
        hideUpgradeMenu();
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
    public void hideUpgradeMenu()
    {
        upgradeBtn.onClick.RemoveListener(UpgradeTower);
        sellButton.onClick.RemoveListener(sellTower);
        this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        UIParent.gameObject.SetActive(false);
    }

    void stopShowingEnergy()
    {
        GameUI.GetChild(0).GetChild(6).gameObject.SetActive(false);
    }


}
