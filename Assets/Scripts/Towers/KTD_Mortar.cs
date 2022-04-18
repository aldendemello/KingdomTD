using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KTD_Mortar : MonoBehaviour
{
    private Vector3 ProjPosition;

    //public AudioSource cannon_blast;

    private Vector3 enemypos;

    private int ballCount;

    private int damage;

    private int ballLim;

    private bool firstClick;

    private bool pauseState;

    private Button backBtn, upgradeBtn;

    public Transform UIParent;

    private Transform GameUI;

    private string upgrading;

    private string upgrade_cost_text;

    public TextMeshProUGUI halstein_energy;

    private float attack_speed = 3f;

    private Transform RangeObject;

    private int level;

    private int energy;

    private float range;

    private Button sellButton;

    private TextMeshProUGUI sellText;

    private int old_cost;

    private Animator anim;

    private AudioSource cannon_blast;

    public AudioSource upgradeSound;

    // Start is called before the first frame update
    void Start()
    {
        upgradeSound = GameObject.Find("UI").GetComponent<AudioSource>();
        ballLim = 1;
        ballCount = 0;
        damage = 4;
        level = 1;
        anim = GetComponent<Animator>();
        cannon_blast = GetComponent<AudioSource>();
        GameUI = GameObject.Find("UI").transform;
        UIParent = GameUI.GetChild(1);
        halstein_energy = GameObject.FindGameObjectWithTag("PlayerData").GetComponent<TextMeshProUGUI>();
        RangeObject = this.gameObject.transform.GetChild(0);
        RangeObject.GetComponent<SpriteRenderer>().enabled = false;
        firstClick = true;
        range = 10;
        energy = 675;
        old_cost = 600;
        attack_speed = 3f;
        upgrade_cost_text = (energy.ToString() + " ENERGY");
        upgrading = "Level: " + level.ToString() + "\n\nIncreases Range\nIncreases Damage\nIncreases Speed";
        backBtn = UIParent.transform.GetChild(2).GetComponent<Button>();
        upgradeBtn = UIParent.transform.GetChild(1).GetChild(0).GetComponent<Button>();
        backBtn.onClick.AddListener(hideUpgradeMenu);
        //cannon_blast = this.gameObject.GetComponent<AudioSource>();
        sellButton = UIParent.transform.GetChild(3).GetComponent<Button>();
        sellText = UIParent.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>();
        this.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
    }

    void CallCannonball()
    {
        KTD_MortarCannonball.Create(ProjPosition, enemypos, damage);
        cannon_blast.Play();
        anim.SetInteger("blast", 0);
        CancelInvoke("CallCannonball");
    }


    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (other.gameObject.GetComponent<KTD_Enemy>().isCamo == false && other.gameObject.GetComponent<KTD_Enemy>().isAerial == false)
            {
                enemypos = other.transform.position;
                enemypos.z = 2;
                Vector3 moveDir = (enemypos - transform.position).normalized;
                float angle = 0f;
                transform.eulerAngles = new Vector3(0, 0, angle);
                anim.SetInteger("blast", 1);
                InvokeRepeating("CallCannonball", attack_speed, 0.5f);
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
                        UIParent.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>().sprite = this.gameObject.GetComponent<SpriteRenderer>().sprite;
                        RangeObject.GetComponent<SpriteRenderer>().enabled = true;
                        sellText.text = "+" + ((int)(old_cost * .75)).ToString() + "E";
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
                damage += 1;
                attack_speed -= 0.085f;
                range += 0.5f;
                old_cost = energy;
                energy += 250;
                this.gameObject.transform.GetChild(0).localScale = new Vector3(range, range, 5f);
                upgrading = "Level: " + level.ToString() + "\n\nIncreases Range\nIncreases Damage\nIncreases Speed";
                upgrade_cost_text = energy.ToString() + " ENERGY";
            }
            if (level == 6)
            {
                damage += 4;
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
