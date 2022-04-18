using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class KTD_Archer : MonoBehaviour
{
    private Vector3 ProjPosition;

    private Vector3 enemypos;


    private int arrowCount;

    private int arrowLim;

    private bool firstClick;

    // Info

    public int level;

    public int damage;

    public Sprite tower1;

    public Sprite tower2;

    public Sprite tower3;

    public Sprite tower4;

    public Sprite tower5;

    public bool canseeCamo;

    public float range;

    private Sprite nextsprite;

    public int energy;

    public Transform UIParent;

    private Transform GameUI;

    private bool pauseState;

    private string upgrading;

    private string upgrade_cost_text;

    public TextMeshProUGUI halstein_energy;

    private Button backBtn, upgradeBtn;

    private float attack_speed = 0.5f;

    private float c_attack_speed = 0.45f;

    private Transform RangeObject;

    private Button sellButton;

    private TextMeshProUGUI sellText;

    private int old_cost;

    public AudioSource upgradeSound;

    // Start is called before the first frame update
    void Start()
    {
        upgradeSound = GameObject.Find("UI").GetComponent<AudioSource>();
        GameUI = GameObject.Find("UI").transform;
        UIParent = GameUI.GetChild(1);
        canseeCamo = false;
        halstein_energy = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<TextMeshProUGUI>();
        arrowLim = 1;
        level = 1;
        damage = 1;
        arrowCount = 0;
        RangeObject = this.gameObject.transform.GetChild(0);
        RangeObject.GetComponent<SpriteRenderer>().enabled = false;
        firstClick = true;
        Sprite[] towersprites = Resources.LoadAll<Sprite>("Sprites/ArcherTower/KTD_NewArcher");
        tower1 = towersprites[0];
        tower2 = towersprites[1];
        tower3 = towersprites[2];
        tower4 = towersprites[3];
        tower5 = towersprites[4];
        range = 3;
        old_cost = 225;
        energy = 650;
        this.gameObject.GetComponent<SpriteRenderer>().sprite = tower1;
        nextsprite = tower2;
        upgrade_cost_text = (energy.ToString() + " ENERGY");
        upgrading = "Level: " + level.ToString() + "\n\nIncreases Range\nIncreases Damage\nIncreases Speed";
        backBtn = UIParent.transform.GetChild(2).GetComponent<Button>();
        upgradeBtn = UIParent.transform.GetChild(1).GetChild(0).GetComponent<Button>();
        sellButton = UIParent.transform.GetChild(3).GetComponent<Button>();
        sellText = UIParent.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>();
        backBtn.onClick.AddListener(hideUpgradeMenu);

    }

    void CallArrow()
    {
        if (canseeCamo)
        {
            KTD_Arrows.Create(ProjPosition, enemypos, damage, true);
        }
        else
        {
            KTD_Arrows.Create(ProjPosition, enemypos, damage, false);
        }

        CancelInvoke("CallArrow");
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (canseeCamo)
            {
                enemypos = other.transform.position;
                enemypos.z = 2;
                for (int i = 0; i <= arrowLim; i++)
                {
                    InvokeRepeating("CallArrow", c_attack_speed, 1f);
                }
            }
            else
            {
                if (other.gameObject.GetComponent<KTD_Enemy>().isCamo == false)
                {
                    enemypos = other.transform.position;
                    enemypos.z = 2;
                    for (int i = 0; i <= arrowLim; i++)
                    {
                        InvokeRepeating("CallArrow", attack_speed, 1f);
                    }
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
            try
            {
                pauseState = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<KTD_Pause>().gameIsPaused;
            }
            catch
            {

            }
            if (firstClick == true)
            {
                firstClick = false;
            }
            else
            {
                try
                {
                    RangeObject.GetComponent<SpriteRenderer>().enabled = false;
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
                                UIParent.transform.GetChild(1).GetChild(0).GetChild(0).localScale = new Vector3(1.5f, 2f, 0f);
                                UIParent.transform.GetChild(1).GetComponent<Image>().enabled = true;
                            }
                            UIParent.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>().sprite = nextsprite;
                            RangeObject.GetComponent<SpriteRenderer>().enabled = true;
                            UIParent.gameObject.SetActive(true);
                            sellText.text = "+" + ((int)(old_cost * .75)).ToString() + "E";
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
                    catch
                    {
                    }
                }
                catch { }
            }

        }
    }

    public void hideUpgradeMenu()
    {
        upgradeBtn.onClick.RemoveListener(UpgradeTower);
        sellButton.onClick.RemoveListener(sellTower);
        RangeObject.GetComponent<SpriteRenderer>().enabled = false;
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

    public void UpgradeTower()
    {
        Sprite currentSprite = this.gameObject.GetComponent<SpriteRenderer>().sprite;
        int old_energy;
        int.TryParse(halstein_energy.text, out old_energy);
        if (old_energy >= energy)
        {
            upgradeSound.Play();
            int new_eng = old_energy - energy;
            halstein_energy.text = new_eng.ToString();
            old_cost = energy;
            if (level == 1)
            {
                this.gameObject.GetComponent<SpriteRenderer>().sprite = nextsprite;
                nextsprite = tower3;
                damage += 2;
                level = 2;

                energy = 650;
                attack_speed -= 0.05f;
                range += .5f;
                this.gameObject.transform.GetChild(0).localScale = new Vector3(range, range, 5f);
                upgrading = "Level: " + level.ToString() + "\n\nIncreases Range\nIncreases Damage\nSee Camo Enemies";
                upgrade_cost_text = "650 ENERGY";
            }
            else if (level == 2)
            {
                this.gameObject.GetComponent<SpriteRenderer>().sprite = nextsprite;
                nextsprite = tower4;
                level = 3;
                canseeCamo = true;
                damage += 2;
                energy = 850;
                upgrade_cost_text = "850 ENERGY";
                this.gameObject.transform.GetChild(0).localScale = new Vector3(4f, 4f, 5f);
                upgrading = "Level: " + level.ToString() + "\n\nIncreases Range\nIncreases Damage\nIncreases Speed";

            }
            else if (level == 3)
            {
                this.gameObject.GetComponent<SpriteRenderer>().sprite = nextsprite;
                nextsprite = tower5;
                level = 4;
                c_attack_speed -= 0.025f;
                damage += 2;
                energy = 1050;
                upgrade_cost_text = "1050 ENERGY";
                this.gameObject.transform.GetChild(0).localScale = new Vector3(4.5f, 4.5f, 5f);
                upgrading = "Level: " + level.ToString() + "\n\nIncreases Range\nIncreases Damage";
            }
            else if (level == 4)
            {
                this.gameObject.GetComponent<SpriteRenderer>().sprite = nextsprite;
                level = 5;
                damage += 2;
                energy = 1300;
                upgrade_cost_text = "1300 ENERGY";
                this.gameObject.transform.GetChild(0).localScale = new Vector3(5f, 5f, 5f);
                upgrading = "Level: " + level.ToString() + "\n\nIncreases Range\nIncreases Damage\nIncreases Speed";


            }
            else if (level == 5)
            {
                this.gameObject.GetComponent<SpriteRenderer>().sprite = nextsprite;
                c_attack_speed -= 0.05f;
                level = 6;
                damage += 4;
                upgrading = "Level: " + level.ToString() + "\n\nNo More Upgrades.";
                this.gameObject.transform.GetChild(0).localScale = new Vector3(6f, 6f, 5f);
                UIParent.transform.GetChild(1).GetChild(0).GetChild(0).gameObject.SetActive(false);
                UIParent.transform.GetChild(1).GetChild(2).gameObject.SetActive(false);
                UIParent.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
                UIParent.transform.GetChild(1).GetComponent<Image>().enabled = false;

            }
            else if (level == 6)
            {

            }
        }
        else
        {
            GameUI.GetChild(0).GetChild(6).gameObject.SetActive(true);
            Invoke("stopShowingEnergy", 2f);

        }

        hideUpgradeMenu();


    }

    void stopShowingEnergy()
    {
        GameUI.GetChild(0).GetChild(6).gameObject.SetActive(false);
    }
}

