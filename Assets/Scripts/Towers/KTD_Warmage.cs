using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KTD_Warmage : MonoBehaviour
{
    private Vector3 ProjPosition;

    public AudioSource cannon_blast;

    private int damage;

    private int level;

    private float range;

    private Vector3 enemypos;

    private int ballCount;

    private int ballLim;

    private bool firstClick;

    private bool pauseState;

    private Animator anim;

    public Transform UIParent;

    private Transform GameUI;

    private Transform RangeObject;

    public int energy;

    public TextMeshProUGUI halstein_energy;

    private string upgrading;

    private string upgrade_cost_text;

    private Button backBtn, upgradeBtn;

    private float attack_speed;

    private Button sellButton;

    private TextMeshProUGUI sellText;

    private int old_cost;

    public AudioSource upgradeSound;
    void Start()
    {
        upgradeSound = GameObject.Find("UI").GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        ballLim = 1;
        ballCount = 0;
        damage = 7;
        range = 15f;
        level = 1;
        energy = 1000;
        old_cost = 1500;
        GameUI = GameObject.Find("UI").transform;
        UIParent = GameUI.GetChild(1);
        RangeObject = this.gameObject.transform.GetChild(0);
        RangeObject.GetComponent<SpriteRenderer>().enabled = false;
        upgrade_cost_text = (energy.ToString() + " ENERGY");
        upgrading = "Level: " + level.ToString() + "\n\nIncreases Range\nIncreases Damage\nIncreases Speed";
        backBtn = UIParent.transform.GetChild(2).GetComponent<Button>();
        upgradeBtn = UIParent.transform.GetChild(1).GetChild(0).GetComponent<Button>();
        backBtn.onClick.AddListener(hideUpgradeMenu);
        upgradeBtn.onClick.AddListener(UpgradeTower);
        halstein_energy = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<TextMeshProUGUI>();
        cannon_blast = this.gameObject.GetComponent<AudioSource>();
        this.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        UIParent.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>().sprite = this.gameObject.GetComponent<SpriteRenderer>().sprite;
        firstClick = true;
        sellButton = UIParent.transform.GetChild(3).GetComponent<Button>();
        sellText = UIParent.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    void CallCannonball()
    {

        KTD_Fireball.Create(ProjPosition, enemypos, damage);
        //cannon_blast.Play();
        ballCount++;
        if (ballCount >= ballLim)
        {
            ballCount = 0;
            anim.SetInteger("Shooting", 0);
            CancelInvoke("CallCannonball");

        }
    }


    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            enemypos = other.transform.position;
            enemypos.z = 2;
            anim.SetInteger("Shooting", 1);
            InvokeRepeating("CallCannonball", 1f, 1f);

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
                            UIParent.transform.GetChild(1).GetChild(0).GetChild(0).localScale = new Vector3(.9f, 2f, 0f);
                            UIParent.transform.GetChild(1).GetComponent<Image>().enabled = true;
                        }
                        UIParent.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>().sprite = this.gameObject.GetComponent<SpriteRenderer>().sprite;
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
                damage += 1;
                attack_speed -= 0.05f;
                range += 0.5f;
                old_cost = energy;
                energy += 150;
                this.gameObject.transform.GetChild(0).localScale = new Vector3(range, range, 5f);
                upgrading = "Level: " + level.ToString() + "\n\nIncreases Range\nIncreases Damage\nIncreases Speed";
                upgrade_cost_text = energy.ToString() + " ENERGY";
            }
            if (level == 6)
            {
                damage += 5;
                range += 3f;
                upgrading = "Level: " + level.ToString() + "\n\nNo More Upgrades.";
                this.gameObject.transform.GetChild(0).localScale = new Vector3(range, range, 5f);
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

    void stopShowingEnergy()
    {
        GameUI.GetChild(0).GetChild(6).gameObject.SetActive(false);
    }


}