using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


// Add level up functionality
// Add attacking
// Add building
// Add Health
// Add Strength
public class KTD_Halstein : MonoBehaviour
{

    public float speed = 3.0f;

    private Animator anim;

    // Movement Integers
    private const int run_down = -1;

    public Sprite armor2;

    public Sprite armor3;
    public Sprite armor4;
    public Sprite armor5;

    private const int run_up = 1;

    private const int run_left = -1;

    private const int run_right = 1;


    // Base Stats
    public int attack_dmg = 5;

    private float attack_speed = 5f;


    // Timing-Related Variables
    private float attack_timer = 0;

    public RuntimeAnimatorController baseArmor;

    public RuntimeAnimatorController upgrade1;

    public RuntimeAnimatorController upgrade2;

    public RuntimeAnimatorController upgrade3;

    public RuntimeAnimatorController upgrade4;

    private int max = 1;

    private float maxTime;

    public GameObject NoEnergy;

    private bool out_of_energy;

    private int attacking;
    public int level;

    public TextMeshProUGUI energy_lvl;

    private bool pauseState;

    private bool attack;

    private int upgrade_energy;

    private AudioSource attack_sound;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        attack_sound = GetComponent<AudioSource>();
        attacking = 0;
        attack = false;
        level = 1;
        upgrade_energy = 500;
        Transform upgradeData = this.transform.GetChild(1).GetChild(0).GetChild(1);
        upgradeData.GetChild(0).GetChild(0).GetComponent<Image>().sprite = armor2;
        maxTime = 2f;
        out_of_energy = false;
    }   

    void Attacking()
    {
        if (attacking <= max)
        {
            anim.SetInteger("Attack", 0);
            CancelInvoke("Attacking");
            int old_energy;
            int.TryParse(energy_lvl.GetComponent<TextMeshProUGUI>().text, out old_energy);
            int new_energy = (old_energy - 10);
            energy_lvl.GetComponent<TextMeshProUGUI>().text = new_energy.ToString();
            attacking = 0;
            attack = false;

        }
        attacking++;
    }

    // Update is called once per frame
    void Update()
    {
        if (attack_timer >= 0) {
            attack_timer -= Time.deltaTime;
        }
        

        if (out_of_energy)
        {
            maxTime -= Time.deltaTime;

            if (maxTime <= 0f)
            {
                NoEnergy.SetActive(false);
                maxTime = 2f;
                out_of_energy = false;
            }
        }
        
        // Prevents Sprite from Rotating when interacting with collision
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        
        // Sprite Updating
        //// Animation
        float v = Input.GetAxis("Vertical"); 
        float h = Input.GetAxis("Horizontal");

        //// User is not moving horizontally
        if (v<0 && h == 0)
        {
            anim.SetInteger("RS_V",run_down);
            anim.SetInteger("RS_H", 0);
        } else if (v>0 && h == 0)
        {
            anim.SetInteger("RS_V",run_up);
            anim.SetInteger("RS_H", 0);
        
        //// User is not moving vertically
        } else if (h<0 && v == 0)
        {
            anim.SetInteger("RS_H",run_left);
            anim.SetInteger("RS_V", 0);
        } else if (h>0 && v == 0)
        {
            anim.SetInteger("RS_H",run_right);
            anim.SetInteger("RS_V", 0);

        //// General in case circumstance not caught by previous statements
        } else if (v<0)
        {
            anim.SetInteger("RS_V",run_down);
            anim.SetInteger("RS_H", 0);
        } else if (v>0)
        {
            anim.SetInteger("RS_V",run_up);
            anim.SetInteger("RS_H", 0);
        } else if (h<0)
        {
            anim.SetInteger("RS_H", run_left);
            anim.SetInteger("RS_V", 0);
        } else if (h>0)
        {
            anim.SetInteger("RS_H",run_right);
            anim.SetInteger("RS_V", 0);
        } else
        {
            // User is not moving
            anim.SetInteger("RS_V", 0);
            anim.SetInteger("RS_H", 0);
        }

        int old_energy;
        int.TryParse(energy_lvl.GetComponent<TextMeshProUGUI>().text, out old_energy);

        // Attacking
        if (attack_timer <= 0 && old_energy >= 10)
        // User attack cooldown has passed
        {
            if (Input.GetKeyDown("space")) {
                attack = true;
                anim.SetInteger("Attack", 1);
                attack_sound.Play();
                attack_timer = attack_speed;
                InvokeRepeating("Attacking", 1f, 2.0f);
            }
        } else if (attack_timer <= 0 && old_energy < 10) {
            if (Input.GetKeyDown("space")) {
                out_of_energy = true;
                NoEnergy.SetActive(true);
            }
        }

        if (Input.GetMouseButton(0))
        {
            try {
                pauseState = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<KTD_Pause>().gameIsPaused; 
            } catch {

            }   
            this.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
            try{

            
                Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mouse2D = new Vector2(mouse.x, mouse.y);

                // Casts the ray and get the first game object hit
                RaycastHit2D hit = Physics2D.Raycast(mouse2D, Vector2.zero);
                try {
                    if (hit.collider.gameObject.name == "Halstein" && pauseState == false)
                    {
                        this.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
                        this.transform.GetChild(1).gameObject.SetActive(true);
                    } else {
                        this.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
                    }
                } catch{
                    
                }
            } catch {}
        }
    }

    void FixedUpdate()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        // User movement
        if(Input.GetKey(KeyCode.A)) // Left
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D)) // Right
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.W)) // Up
        {
            transform.Translate(Vector2.up * speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.S)) // Down
        {
            transform.Translate(Vector2.down * speed * Time.deltaTime);
        }
        
    }

    public void UpgradeHalstein()
    {
        int old_energy;
        int.TryParse(energy_lvl.GetComponent<TextMeshProUGUI>().text, out old_energy);
        if (old_energy >= upgrade_energy)
        {
            Transform upgradeData = this.transform.GetChild(1).GetChild(0).GetChild(1);
            int new_energy = (old_energy - upgrade_energy);
            energy_lvl.GetComponent<TextMeshProUGUI>().text = new_energy.ToString();
            
            if (this.gameObject.GetComponent<Animator>().runtimeAnimatorController == baseArmor)
            {
                this.gameObject.GetComponent<Animator>().runtimeAnimatorController = upgrade1;
               
                upgradeData.GetChild(0).GetChild(0).GetComponent<Image>().sprite = armor3;
                upgradeData.GetChild(2).GetComponent<TextMeshProUGUI>().text = "650 ENERGY";
                upgrade_energy = 650;
                this.transform.GetChild(1).gameObject.SetActive(false);

                level++;
                attack_dmg += 1;
                attack_speed -= (attack_speed * .1f);
                

                
            }
            else if (this.gameObject.GetComponent<Animator>().runtimeAnimatorController == upgrade1)
            {
                this.gameObject.GetComponent<Animator>().runtimeAnimatorController = upgrade2;
                upgradeData.GetChild(0).GetChild(0).GetComponent<Image>().sprite = armor4;
                upgradeData.GetChild(2).GetComponent<TextMeshProUGUI>().text = "800 ENERGY";
                upgrade_energy = 800;
                this.transform.GetChild(1).gameObject.SetActive(false);
                
                level++;
                attack_dmg += 1;
                attack_speed -= (attack_speed * .1f);
            }
            else if (this.gameObject.GetComponent<Animator>().runtimeAnimatorController == upgrade2)
            {
                this.gameObject.GetComponent<Animator>().runtimeAnimatorController = upgrade3;
                upgradeData.GetChild(0).GetChild(0).GetComponent<Image>().sprite = armor5;
                upgradeData.GetChild(2).GetComponent<TextMeshProUGUI>().text = "1000 ENERGY";
                upgrade_energy = 1000;
                this.transform.GetChild(1).gameObject.SetActive(false);
                level++;
                attack_dmg += 1;
                attack_speed -= (attack_speed * .1f);
            }

            else if (this.gameObject.GetComponent<Animator>().runtimeAnimatorController == upgrade3)
            {
                this.gameObject.GetComponent<Animator>().runtimeAnimatorController = upgrade4;
                upgradeData.GetChild(0).gameObject.SetActive(false);
                upgradeData.GetChild(2).gameObject.SetActive(false);
                upgradeData.GetChild(1).GetComponent<TextMeshProUGUI>().text = "No More Upgrades Available";
                upgradeData.GetComponent<Image>().color = new Color(0f,0f,0f,0f);
                
                this.transform.GetChild(1).gameObject.SetActive(false);
                level++;
                attack_dmg += 3;
                attack_speed -= (attack_speed * .25f);
            }
            
        } else {
            NoEnergy.SetActive(true);
            Invoke("stopShowing", 2f);
        }
    }

    void stopShowing(){
        NoEnergy.SetActive(false);
    }
}
