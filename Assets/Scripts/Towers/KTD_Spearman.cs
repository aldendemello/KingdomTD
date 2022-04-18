using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class KTD_Spearman : MonoBehaviour
{

    private Vector3 enemypos;

    private bool firstClick;

    // Info

    public int level;

    public int damage;

    public float range;

    public int energy;

    public Transform UIParent;

    private Transform GameUI;

    private bool pauseState;

    private string upgrading;

    private string upgrade_cost_text;

    public TextMeshProUGUI halstein_energy;

    private Button backBtn, upgradeBtn;

    public float attack_speed = 11f;

    private float c_attack_speed = 0.45f;

    private Button sellButton;

    private TextMeshProUGUI sellText;

    private Transform RangeObject;

    private Sprite currentSprite;

    private int old_cost;

    public Animator anim;

    public AudioSource upgradeSound;

    // Start is called before the first frame update
    void Start()
    {

        //STATS
        level = 1;
        damage = 15;
        firstClick = true;
        range = 4;
        old_cost = 1000;
        energy = 1150;
        upgradeSound = GameObject.Find("UI").GetComponent<AudioSource>();
        //OBJECT DECS
        GameUI = GameObject.Find("UI").transform;
        UIParent = GameUI.GetChild(1);
        halstein_energy = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<TextMeshProUGUI>();
        RangeObject = this.gameObject.transform.GetChild(0);
        anim = GetComponent<Animator>();
        currentSprite = this.gameObject.GetComponent<SpriteRenderer>().sprite;
        upgrade_cost_text = (energy.ToString() + " ENERGY");
        upgrading = "Level: " + level.ToString() + "\n\nIncreases Range\nIncreases Damage\nIncreases Speed";
        backBtn = UIParent.transform.GetChild(2).GetComponent<Button>();
        upgradeBtn = UIParent.transform.GetChild(1).GetChild(0).GetComponent<Button>();
        sellButton = UIParent.transform.GetChild(3).GetComponent<Button>();
        sellText = UIParent.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>();


        //Update Objects
        RangeObject.GetComponent<SpriteRenderer>().enabled = false;
        backBtn.onClick.AddListener(hideUpgradeMenu);

    }

    void CancelAnim()
    {
        anim.SetInteger("Attack", 0);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (other.gameObject.GetComponent<KTD_Enemy>().isAerial == true)
            {
                enemypos = other.transform.position;
                enemypos.z = 2;
                anim.SetInteger("Attack", 1);
            }

        }
        else
        {
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (other.gameObject.GetComponent<KTD_Enemy>().isAerial == true)
            {
                Invoke("CancelAnim", 1f);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
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
                                UIParent.transform.GetChild(1).GetChild(0).GetChild(0).localScale = new Vector3(1f, 2f, 0f);
                                UIParent.transform.GetChild(1).GetComponent<Image>().enabled = true;
                            }
                            UIParent.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>().sprite = currentSprite;
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
                damage += 15;
                level = 2;

                energy = 1150;
                attack_speed -= 0.5f;
                range += .5f;
                this.gameObject.transform.GetChild(0).localScale = new Vector3(range, range, 5f);
                upgrading = "Level: " + level.ToString() + "\n\nIncreases Range\nIncreases Damage\nSee Camo Enemies";
            }
            else if (level == 2)
            {
                //4;
                level = 3;
                damage += 15;
                energy = 1350;
                attack_speed -= 0.5f;
                range += .5f;
                this.gameObject.transform.GetChild(0).localScale = new Vector3(range, range, 5f);
                upgrading = "Level: " + level.ToString() + "\n\nIncreases Range\nIncreases Damage\nIncreases Speed";

            }
            else if (level == 3)
            {
                //5;
                level = 4;
                attack_speed -= 0.5f;
                damage += 15;
                energy = 1550;
                range += .5f;
                this.gameObject.transform.GetChild(0).localScale = new Vector3(range, range, 5f);
                upgrading = "Level: " + level.ToString() + "\n\nIncreases Range\nIncreases Damage";
            }
            else if (level == 4)
            {
                level = 5;
                damage += 15;
                energy = 1750;
                attack_speed -= 0.5f;
                range += .5f;
                this.gameObject.transform.GetChild(0).localScale = new Vector3(range, range, 5f);
                upgrading = "Level: " + level.ToString() + "\n\nIncreases Range\nIncreases Damage\nIncreases Speed";


            }
            else if (level == 5)
            {
                c_attack_speed -= 0.1f;
                level = 6;
                damage += 30;
                attack_speed -= 0.5f;
                range += .5f;
                upgrading = "Level: " + level.ToString() + "\n\nNo More Upgrades.";
                this.gameObject.transform.GetChild(0).localScale = new Vector3(range, range, 5f);
                UIParent.transform.GetChild(1).GetChild(0).GetChild(0).gameObject.SetActive(false);
                UIParent.transform.GetChild(1).GetChild(2).gameObject.SetActive(false);
                UIParent.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
                UIParent.transform.GetChild(1).GetComponent<Image>().enabled = false;

            }
            else if (level == 6)
            {

            }
            upgrade_cost_text = energy.ToString() + " ENERGY";
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

