using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PathCreation;
public class KTD_Enemy : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject enemy;

    public int health;

    public int damage;

    public int max_health;

    public float end_x;

    public float end_y;

    public GameObject kingdom;

    public bool isCamo;

    public int energy_return;

    public bool isAerial;

    private SpriteRenderer sprite;

    private AudioSource deathSound;

    public RuntimeAnimatorController death;

    public PathCreator pathCreator;

    void Start()
    {
        GameObject temp = GameObject.Find("Kingdom");
        pathCreator = temp.GetComponent<KTD_kingdom>().path;
        end_x = temp.GetComponent<KTD_kingdom>().end_x;
        end_y = temp.GetComponent<KTD_kingdom>().end_y;
        max_health = health;
        kingdom = GameObject.FindGameObjectWithTag("Kingdom");
        sprite = GetComponent<SpriteRenderer>();
        deathSound = GetComponent<AudioSource>();
        this.gameObject.GetComponent<Follower>().pathCreator = pathCreator;
    }

    void FixedUpdate()
    {
        if (health <= 0) {
            deathSound.Play();
            GameObject energy = GameObject.FindGameObjectWithTag("PlayerData");
            int incr;
            int.TryParse(energy.GetComponent<TextMeshProUGUI>().text, out incr);
            energy.GetComponent<TextMeshProUGUI>().text = (incr + energy_return).ToString();
            Instantiate(GameAssets.i.pfDeath, this.gameObject.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
            
        } else {
            
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Spike")
        {
            if (!isAerial)
            {
                if (other.gameObject.name == "pfTrap(Clone)")
                {
                    try{
                        health -= other.gameObject.GetComponent<KTD_Trap>().damage;
                    } catch {}
                    
                }
            }
            
        }
        if (other.gameObject.name == "pfSpearman(Clone)")
        {
            if (isAerial)
            {
                health -= other.gameObject.GetComponent<KTD_Spearman>().damage;
            }
            if (health <= 0) {
                other.gameObject.GetComponent<KTD_Spearman>().anim.SetInteger("Attack",1);
                deathSound.Play();
            }
        }
        if (other.gameObject.tag == "Projectile")
        {
            if (isCamo && !isAerial)
            {
                if (other.gameObject.name == "pfJavelin(Clone)")
                {
                    health -= 10;
                } else if (other.gameObject.name == "pfFireball(Clone)")
                {
                    health -= other.gameObject.GetComponent<KTD_Fireball>().damage;
                } else if (other.gameObject.name == "pfCamoArrow(Clone)")
                {
                    health -= other.gameObject.GetComponent<KTD_Arrows>().damage;
                } else if (other.gameObject.name == "pfCamoCannonball(Clone)")
                {
                    health -= other.gameObject.GetComponent<KTD_Cannonball>().damage;
                }

            } else if (isAerial) {
                if (other.gameObject.name == "pfArrow(Clone)" || other.gameObject.name == "pfCamoArrow(Clone)")
                {
                    health -= other.gameObject.GetComponent<KTD_Arrows>().damage;
                } else if (other.gameObject.name == "pfFireball(Clone)")
                {
                    health -= other.gameObject.GetComponent<KTD_Fireball>().damage;
                } else if (other.gameObject.name == "pfAerialJavelin(Clone)")
                {
                    health -= other.gameObject.GetComponent<KTD_Javelin>().damage;
                }
                
            } else {
                if (other.gameObject.name == "pfCannonball(Clone)")
                {
                    health -= 5;
                } else if (other.gameObject.name == "pfJavelin(Clone)")
                {
                    health -= 10;
                }else if (other.gameObject.name == "pfFireball(Clone)")
                {
                    health -= other.gameObject.GetComponent<KTD_Fireball>().damage;
                } else if (other.gameObject.name == "pfArrow(Clone)" || other.gameObject.name == "pfCamoArrow(Clone)")
                {
                    health -= other.gameObject.GetComponent<KTD_Arrows>().damage;
                } else if (other.gameObject.name == "Explosion"){
                    health -= 5;
                } else if (other.gameObject.name == "pfMortarCannonball(Clone)")
                {
                    health -= other.gameObject.GetComponent<KTD_MortarCannonball>().damage;
                } else {
                    health -= 5;
                }
            }
            if (health <= 0) {
                deathSound.Play();
            }
            
            
            
        }
        if (other.gameObject.name == "Halstein" && !isAerial)
        {
            if (other.gameObject.GetComponent<Animator>().GetInteger("Attack") == 1)
            //if (Input.GetMouseButtonDown(0))
            {
                health -= other.gameObject.GetComponent<KTD_Halstein>().attack_dmg;
            }
            if (health <= 0) {
                deathSound.Play();
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        
        
        if (this.gameObject.name != "pfSkeleton(Clone)") {
            try{
                decimal redness = (decimal)health/max_health;
                sprite.color = new Color(255, (float)redness ,(float)redness ,1f);
            } catch {}
            
        }
        if (other.gameObject.name == "Halstein" && !isAerial)
        {
            if (other.gameObject.GetComponent<Animator>().GetInteger("Attack") == 1)
            //if (Input.GetMouseButtonDown(0))
            {
                if (other.gameObject.GetComponent<KTD_Halstein>().level >= 4)
                {
                    // Uhhhh this is not how this is meant to work??? wtf?
                    health -= 1;
                }
                
            }
        }
        
    }

}
