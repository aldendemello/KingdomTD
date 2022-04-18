using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KTD_TowerPlace : MonoBehaviour
{

    public GameObject player;

    public string towerType;

    Component script;
    public Sprite towerSprite;

    public Sprite build;

    public TextMeshProUGUI energy_lvl;

    public RuntimeAnimatorController builder;

    public const string LAYER = "FG";

    private int placecount;

    private Transform newTower;

    private bool tower_placed;

    private bool tower_place_init;

    private bool tower_colliding;

    private int energy_val;

    private Vector3 difference;

    private bool towerDeleted;

    private int old_energy;

    public GameObject NoEnergy;

    private float maxTime;

    private bool out_of_energy;

    private Vector3 tower_start;

    private Vector3 lastpos;

    private int posCounter;

    private int[] locations_x;

    private int[] locations_y;

    private bool firstStart;

    private bool tapOnce;

    // Start is called before the first frame update
    void Start()
    {
        towerDeleted = false;
        maxTime = 2f;
        out_of_energy = false;
        posCounter = 0;
        firstStart = true;
        locations_x = new int[] { 3, -3, -3, 3 };
        locations_y = new int[] { 5, 5, -3, -3 };
    }


    public void PlaceTower()
    {
        int old_energy;

        int.TryParse(energy_lvl.GetComponent<TextMeshProUGUI>().text, out old_energy);
        tower_place_init = true; // Begin tower-placing mode
        if (towerType == "ArcherTower")
        {
            newTower = Archer();
            energy_val = 225;
        }
        else if (towerType == "FarmTower")
        {
            newTower = Farm();
            energy_val = 1125;
        }
        else if (towerType == "CannonTower")
        {
            newTower = Cannon();
            energy_val = 375;
        }
        else if (towerType == "BallistaTower")
        {
            newTower = Ballista();
            energy_val = 525;
        }
        else if (towerType == "MortarTower")
        {
            newTower = Mortar();
            energy_val = 600;
        }
        else if (towerType == "WarmageTower")
        {
            newTower = Warmage();
            energy_val = 1500;
        }
        else if (towerType == "SpearmanTower")
        {
            newTower = Spearman();
            energy_val = 1000;
        }
        else if (towerType == "Spike")
        {
            newTower = Spike();
            energy_val = 15;
        }

        newTower.GetComponent<KTD_TowerPlace>().energy_lvl = energy_lvl;
        newTower.GetComponent<KTD_TowerPlace>().towerType = towerType;
        tower_start = newTower.transform.position;
        SpriteRenderer spriteR = newTower.GetComponent<SpriteRenderer>();
        spriteR.color = new Color(1f, 1f, 1f, .5f);

        tower_placed = false;
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Path" && towerType != "Spike")
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            tower_colliding = true;
            if (Input.GetMouseButtonDown(0) && tower_colliding)
            {
                tower_placed = false;
                Destroy(this.gameObject);
                Destroy(this.gameObject.GetComponent("KTD_TowerPlace"));
                tower_place_init = false;
                towerDeleted = true;

            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
    }

    public void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Tower" || other.gameObject.tag == "Farm")
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            tower_colliding = true;
            if (Input.GetMouseButtonDown(0) && tower_colliding)
            {

                tower_placed = false;
                Destroy(this.gameObject);
                Destroy(this.gameObject.GetComponent("KTD_TowerPlace"));
                tower_place_init = false;
                towerDeleted = true;

            }
        }

    }

    void OnCollisionExit2D(Collision2D other)
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (out_of_energy)
        {
            maxTime -= Time.deltaTime;
            if (maxTime <= 0)
            {
                NoEnergy.SetActive(false);
                maxTime = 2f;
                out_of_energy = false;
            }
        }
        if (tower_place_init == true && !tower_placed)
        {
            if (Input.GetMouseButtonDown(1))
            {
                tower_place_init = false;
                Destroy(newTower.gameObject);
            }
            else if (tower_colliding == false)
            {
                if (Input.GetMouseButtonDown(0) && tower_colliding == false && towerDeleted == false)
                {
                    //    Build Animation Creation
                    int old_energy;
                    int.TryParse(energy_lvl.GetComponent<TextMeshProUGUI>().text, out old_energy);
                    int new_energy = (old_energy - energy_val);
                    if (new_energy < 0)
                    {
                        out_of_energy = true;
                        NoEnergy.SetActive(true);
                        tower_place_init = false;
                        Destroy(newTower.gameObject);

                    }
                    else
                    {
                        // Make final movement of new tower and change color to full opacity
                        if (newTower != null && newTower.GetComponent<SpriteRenderer>().color != Color.red)
                        {
                            newTower.transform.position = new Vector3(player.transform.position.x + locations_x[posCounter], player.transform.position.y + locations_y[posCounter], player.transform.position.z);
                            locations_x = new int[] { 3, -3, -3, 3 };
                            locations_y = new int[] { 5, 5, -3, -3 };
                            GameObject build_anim = new GameObject();
                            build_anim.name = "BuildingAnim";
                            build_anim.AddComponent<SpriteRenderer>();
                            SpriteRenderer spriteW = build_anim.GetComponent<SpriteRenderer>();
                            spriteW.sprite = build;
                            spriteW.sortingLayerName = "Animations";
                            build_anim.transform.localScale = new Vector3(2.5f, 2.5f, 0);
                            build_anim.AddComponent<Animator>();
                            build_anim.GetComponent<Animator>().runtimeAnimatorController = builder;
                            build_anim.transform.position = new Vector3(player.transform.position.x + locations_x[posCounter], player.transform.position.y + locations_y[posCounter], player.transform.position.z);

                            SpriteRenderer s_r = newTower.GetComponent<SpriteRenderer>();
                            s_r.color = new Color(1f, 1f, 1f, 1f);
                            tower_placed = true;
                            Destroy(newTower.GetComponent("KTD_TowerPlace"));
                            if (towerType == "ArcherTower")
                            {
                                newTower.gameObject.AddComponent<KTD_Archer>();
                                newTower.GetChild(0).GetComponent<CircleCollider2D>().enabled = true;
                                newTower.GetComponent<BoxCollider2D>().isTrigger = false;
                            }
                            else if (towerType == "CannonTower")
                            {
                                newTower.gameObject.AddComponent<KTD_CannonTower>();
                                newTower.GetChild(0).GetComponent<CircleCollider2D>().enabled = true;
                                newTower.GetComponent<BoxCollider2D>().isTrigger = false;
                            }
                            else if (towerType == "FarmTower")
                            {
                                newTower.gameObject.AddComponent<KTD_farmtower>();
                                newTower.GetComponent<BoxCollider2D>().isTrigger = false;
                                newTower.tag = "Farm";
                            }
                            else if (towerType == "MortarTower")
                            {
                                newTower.gameObject.AddComponent<KTD_Mortar>();
                                newTower.GetChild(0).GetComponent<CircleCollider2D>().enabled = true;
                                newTower.GetComponent<BoxCollider2D>().isTrigger = false;
                            }
                            else if (towerType == "BallistaTower")
                            {
                                newTower.gameObject.AddComponent<KTD_Ballista>();
                                newTower.GetChild(0).GetComponent<CircleCollider2D>().enabled = true;

                            }
                            else if (towerType == "WarmageTower")
                            {
                                newTower.gameObject.AddComponent<KTD_Warmage>();
                                newTower.GetChild(0).GetComponent<CircleCollider2D>().enabled = true;
                                newTower.GetComponent<BoxCollider2D>().isTrigger = false;
                            }
                            else if (towerType == "SpearmanTower")
                            {
                                newTower.gameObject.AddComponent<KTD_Spearman>();
                                newTower.GetChild(0).GetComponent<CircleCollider2D>().enabled = true;
                                newTower.GetComponent<BoxCollider2D>().isTrigger = false;
                            }
                            else if (towerType == "Spike")
                            {
                                newTower.gameObject.AddComponent<KTD_Trap>();
                                newTower.GetComponent<BoxCollider2D>().isTrigger = true;
                            }
                        }
                        

                    }
                }
                else if (tower_placed == false)
                {
                    /*
                    Using is placing a tower and hasn't clicked yet.
                    */
                    Vector3 newpos = new Vector3(player.transform.position.x + 3, player.transform.position.y + 5, player.transform.position.z);
                    try
                    {
                        locations_x = new int[] { 3, -3, -3, 3 };
                        locations_y = new int[] { 5, 5, -3, -3 };

                        if (Input.GetKeyDown(KeyCode.Q))
                        {

                            firstStart = false;
                            incrPosCounter();
                        }
                        if (firstStart == false)
                        {
                            Vector3 updatednewpos = new Vector3(player.transform.position.x + locations_x[posCounter], player.transform.position.y + locations_y[posCounter], player.transform.position.z);
                            newTower.transform.position = updatednewpos;
                        }
                        else
                        {
                            newTower.transform.position = newpos;
                        }
                    }
                    catch
                    {

                    }
                }
            }
        }
        else if (tower_place_init && tower_placed && !towerDeleted)
        {
            int old_energy;
            int.TryParse(energy_lvl.GetComponent<TextMeshProUGUI>().text, out old_energy);
            int new_energy = (old_energy - energy_val);
            energy_lvl.GetComponent<TextMeshProUGUI>().text = new_energy.ToString();
            tower_place_init = false;
        }
    }

    void incrPosCounter()
    {
        posCounter++;
        if (posCounter > 3)
        {
            posCounter = 0;
        }
    }
    private Transform Warmage()
    {
        Vector3 placement = player.transform.position;
        Transform newTower = Instantiate(GameAssets.i.pfWarmageTower, placement, Quaternion.identity);
        newTower.GetChild(0).GetComponent<CircleCollider2D>().enabled = false;
        return newTower;
    }

    private Transform Mortar()
    {
        Vector3 placement = player.transform.position;
        Transform newTower = Instantiate(GameAssets.i.pfMortarTower, placement, Quaternion.identity);
        newTower.GetChild(0).GetComponent<CircleCollider2D>().enabled = false;
        return newTower;
    }


    private Transform Ballista()
    {
        Vector3 placement = player.transform.position;
        Transform newTower = Instantiate(GameAssets.i.pfBallistaTower, placement, Quaternion.identity);
        newTower.GetChild(0).GetComponent<CircleCollider2D>().enabled = false;
        return newTower;
    }


    private Transform Archer()
    {
        /*
        Instantiates an Archer Tower prefab.
        */
        Vector3 placement = player.transform.position;
        Transform newTower = Instantiate(GameAssets.i.pfArcherTower, placement, Quaternion.identity);
        newTower.GetComponent<BoxCollider2D>().size = new Vector2(1.4f, 0.75f);
        newTower.GetComponent<BoxCollider2D>().offset = new Vector2(0f, -1.15f);
        newTower.GetChild(0).GetComponent<CircleCollider2D>().enabled = false;
        newTower.transform.localScale = new Vector3(2.5f, 2.5f, 0);
        return newTower;

    }

    private Transform Farm()
    {
        /*
        Instantiates a Farm Tower prefab.
        */
        Vector3 placement = player.transform.position;
        Transform newTower = Instantiate(GameAssets.i.pfFarmTower, placement, Quaternion.identity);
        newTower.tag = "Tower";
        return newTower;

    }

    private Transform Cannon()
    {
        /*
        Instantiates an Cannon Tower prefab.
        */
        Vector3 placement = player.transform.position;
        Transform newTower = Instantiate(GameAssets.i.pfCannonTower, placement, Quaternion.identity);
        newTower.transform.localScale = new Vector3(2.5f, 2.5f, 0);
        newTower.GetChild(0).GetComponent<CircleCollider2D>().enabled = false;
        return newTower;
    }

    private Transform Spearman()
    {
        Vector3 placement = player.transform.position;
        Transform newTower = Instantiate(GameAssets.i.pfSpearmanTower, placement, Quaternion.identity);
        newTower.GetChild(0).GetComponent<CircleCollider2D>().enabled = false;
        return newTower;
    }

    private Transform Spike()
    {
        Vector3 placement = player.transform.position;
        Transform newTower = Instantiate(GameAssets.i.pfTrap, placement, Quaternion.identity);
        //newTower.GetChild(0).GetComponent<CircleCollider2D>().enabled = false;
        return newTower;
    }


}
