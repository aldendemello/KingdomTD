using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KTD_CannonTower : MonoBehaviour
{
    private Vector3 ProjPosition;

    public AudioSource cannon_blast;

    private Vector3 enemypos;

    private int ballCount;

    private int ballLim;

    private bool firstClick;

    private bool pauseState;

    public int level;

    public int damage;

    public float range;

    private Animator anim;

    public Transform UIParent;

    private Transform GameUI;

    private string upgrading;

    private string upgrade_cost_text;

    private float attack_speed = 0.5f;

    private Transform RangeObject;

    public int energy;

    public TextMeshProUGUI halstein_energy;

    private Button backBtn, upgradeBtn;

    private Vector3 upgradepos;

    private Button sellButton;

    private TextMeshProUGUI sellText;

    private int old_cost;

    private bool canseeCamo;

    private Sprite tower;

    public AudioSource upgradeSound;

    // Start is called before the first frame update
    void Start()
    {

        // Tower Data
        level = 1;
        damage = 3;
        range = 3;
        energy = 500;
        ballLim = 1;
        ballCount = 0;
        old_cost = 375;
        firstClick = true;
        canseeCamo = false;
        anim = GetComponent<Animator>();
        tower = GetComponent<SpriteRenderer>().sprite;

        //Objects
        halstein_energy = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<TextMeshProUGUI>();
        GameUI = GameObject.Find("UI").transform;
        UIParent = GameUI.GetChild(1);
        upgradepos = UIParent.position;
        RangeObject = this.gameObject.transform.GetChild(0);
        upgradeSound = GameObject.Find("UI").GetComponent<AudioSource>();
        cannon_blast = this.gameObject.GetComponent<AudioSource>();
        sellButton = UIParent.transform.GetChild(3).GetComponent<Button>();
        sellText = UIParent.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>();
        backBtn = UIParent.transform.GetChild(2).GetComponent<Button>();
        upgradeBtn = UIParent.transform.GetChild(1).GetChild(0).GetComponent<Button>();
        upgrade_cost_text = (energy.ToString() + " ENERGY");
        upgrading = "Level: " + level.ToString() + "\n\nIncreases Range\nIncreases Damage\nIncreases Speed";


        //Object Manipulation
        RangeObject.GetComponent<SpriteRenderer>().enabled = false;
        this.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        backBtn.onClick.AddListener(hideUpgradeMenu);
        UIParent.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>().sprite = this.gameObject.GetComponent<SpriteRenderer>().sprite;



    }

    void CallCannonball()
    {
        KTD_Cannonball.Create(ProjPosition, enemypos, anim, damage, canseeCamo);
        cannon_blast.Play();
        ballCount++;
        if (ballCount >= ballLim)
        {
            ballCount = 0;
            anim.SetInteger("blast", 0);
            CancelInvoke("CallCannonball");
        }
    }


    public static float detAngle(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 90;
        return n;
    }


    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            KTD_Enemy enemy = other.gameObject.GetComponent<KTD_Enemy>();
            if (canseeCamo)
            {
                if (enemy.isAerial == false)
                {
                    enemypos = other.transform.position;
                    enemypos.z = 2;
                    Vector3 moveDir = (enemypos - transform.position).normalized;
                    float angle = detAngle(moveDir);
                    this.gameObject.transform.GetChild(1).transform.eulerAngles = new Vector3(0, 0, angle);
                    try
                    {
                        this.gameObject.transform.GetChild(2).transform.position = upgradepos;
                        this.gameObject.transform.GetChild(2).transform.eulerAngles = new Vector3(0, 0, 360);
                    }
                    catch { }

                    transform.eulerAngles = new Vector3(0, 0, angle);
                    anim.SetInteger("blast", 1);
                    InvokeRepeating("CallCannonball", 1f, 1f);
                }
            }

            if (enemy.isAerial == false && enemy.isCamo == false)
            {
                enemypos = other.transform.position;
                enemypos.z = 2;
                Vector3 moveDir = (enemypos - transform.position).normalized;
                float angle = detAngle(moveDir);
                this.gameObject.transform.GetChild(1).transform.eulerAngles = new Vector3(0, 0, angle);
                try
                {
                    this.gameObject.transform.GetChild(2).transform.position = upgradepos;
                    this.gameObject.transform.GetChild(2).transform.eulerAngles = new Vector3(0, 0, 360);
                }
                catch { }

                transform.eulerAngles = new Vector3(0, 0, angle);
                anim.SetInteger("blast", 1);
                InvokeRepeating("CallCannonball", 1f, 1f);
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
                            UIParent.transform.GetChild(1).GetChild(0).GetChild(0).localScale = new Vector3(1.5f, 1.5f, 0f);
                            UIParent.transform.GetChild(1).GetComponent<Image>().enabled = true;
                        }
                        UIParent.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>().sprite = tower;
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

    public void hideUpgradeMenu()
    {
        upgradeBtn.onClick.RemoveListener(UpgradeTower);
        //Debug.Log("RUPGRADE3");
        sellButton.onClick.RemoveListener(sellTower);
        //Debug.Log("RSELL3");
        RangeObject.GetComponent<SpriteRenderer>().enabled = false;
        UIParent.gameObject.SetActive(false);
    }

    public void UpgradeTower()
    {
        //Debug.Log("UPGRADE");
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
                upgrading = "Level: " + level.ToString() + "\n\nIncreases Range\nIncreases Damage";
                upgrade_cost_text = energy.ToString() + " ENERGY";
            }
            if (level == 6)
            {
                canseeCamo = true;
                damage += 1;
                range += 1f;
                ballLim += 2;
                upgrading = "Level: " + level.ToString() + "\n\nNo More Upgrades.";
                this.gameObject.transform.GetChild(0).localScale = new Vector3(range, range, 5f);
                UIParent.transform.GetChild(1).GetChild(0).GetChild(0).gameObject.SetActive(false);
                UIParent.transform.GetChild(1).GetChild(2).gameObject.SetActive(false);
                UIParent.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
                UIParent.transform.GetChild(1).GetComponent<Image>().enabled = false;

            }
            if (level == 4)
            {
                upgrading = "Level: " + level.ToString() + "\n\nIncreases Range\nIncreases Damage\nSee Camo Enemies";
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

    void stopShowingEnergy()
    {
        GameUI.GetChild(0).GetChild(6).gameObject.SetActive(false);
    }

}