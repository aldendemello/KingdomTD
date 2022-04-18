using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using UnityEngine.UI;
using TMPro;


public class KTD_wavecreator : MonoBehaviour
{
    public int wave;

    public TextMeshProUGUI wavenum;

    public Button playbtn;

    public Vector3 mapSpawn;

    public PathCreator path;

    public Vector2 end_coords;

    // Enemy Amount Counts


    // Enemy Enabled Booleans


    private Transform enemy_to_spawn;

    private bool bandit_enabled;

    private bool hotairball_enabled;

    private bool giant_enabled;

    private bool skeletons_enabled;

    private bool archmage_enabled;

    private bool dragon_enabled;

    private bool demon_enabled;

    private bool gladiators_enabled;

    private int max_enemy_count;

    private GameObject[] enemies;

    // Max Enemy Integers

    private int gladvar;

    private int gladCount;

    private int skelevar;

    private int skeleCount;

    private int habvar;

    private int habCount;

    private int banditvar;

    private int banditCount;

    private int archmvar;

    private int archmCount;

    private int giantvar;

    private int giantCount;

    private int dragonvar;

    private int dragonCount;

    private int demonvar;

    private int demonCount;


    private float spawnSpeed;

    private bool attack;

    private bool doneSpawning;

    // Start is called before the first frame update
    void Start()
    {
        skelevar = 1;
        wave = 1;
        playbtn = GetComponent<Button>();
        attack = false;
        spawnSpeed = 2.0f;
        skeletons_enabled = true;

    }


    void FixedUpdate()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length >= 1)
        {
            attack = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (attack)
        {
            if (!gladiators_enabled && !skeletons_enabled && !bandit_enabled && !giant_enabled && !archmage_enabled && !demon_enabled && !dragon_enabled && !hotairball_enabled)
            {
                doneSpawning = true;
            }
            if (enemies.Length == 0 && doneSpawning)
            {
                playbtn.interactable = true;
                attack = false;
                GameObject energy = GameObject.FindGameObjectWithTag("PlayerData");
                int plus_energy;
                if (wave >= 35)
                {
                    plus_energy = 35 * (wave / 3);
                }
                else
                {
                    plus_energy = 55 * (wave / 2);
                }
                foreach (GameObject spike in GameObject.FindGameObjectsWithTag("Spike"))
                {
                    spike.GetComponent<KTD_Trap>().rounds += 1;
                }
                foreach (GameObject farm in GameObject.FindGameObjectsWithTag("Farm"))
                {
                    plus_energy += farm.GetComponent<KTD_farmtower>().increase;
                }
                int incr;
                int.TryParse(energy.GetComponent<TextMeshProUGUI>().text, out incr);
                energy.GetComponent<TextMeshProUGUI>().text = (incr + plus_energy).ToString();
            }
        }



        // Enables certain enemies depending on wave number

    }


    void UpdateCounter()
    {
        wavenum.GetComponent<TMPro.TextMeshProUGUI>().text = "WAVE: " + wave;
    }


    /*
    Below are all the functions that spawn each respective enemy.
    */

    private void SpawnGladiator()
    {
        Transform newEnemy = Instantiate(GameAssets.i.pfGladiator, mapSpawn, Quaternion.identity);
        newEnemy.GetComponent<Follower>().pathCreator = path;
        newEnemy.GetComponent<Animator>().enabled = true;
        newEnemy.GetComponent<KTD_Enemy>().end_x = end_coords.x;
        newEnemy.GetComponent<KTD_Enemy>().end_y = end_coords.y;

        //newEnemy.gameObject.name = "Enemy";
        gladCount++;
        if (gladCount >= gladvar)
        {
            gladCount = 0;
            CancelInvoke("SpawnGladiator");
            gladiators_enabled = false;
        }
    }

    private void SpawnSkele()
    {
        Transform newEnemy = Instantiate(GameAssets.i.pfSkeleton, mapSpawn, Quaternion.identity);
        newEnemy.GetComponent<Follower>().pathCreator = path;
        newEnemy.GetComponent<Animator>().enabled = true;
        newEnemy.GetComponent<KTD_Enemy>().end_x = end_coords.x;
        newEnemy.GetComponent<KTD_Enemy>().end_y = end_coords.y;
        //newEnemy.gameObject.name = "Enemy";
        skeleCount++;
        if (skeleCount >= skelevar)
        {
            skeleCount = 0;
            CancelInvoke("SpawnSkele");
            skeletons_enabled = false;
        }
    }

    private void SpawnHAB()
    {
        Transform newEnemy = Instantiate(GameAssets.i.pfHotAirBalloon, mapSpawn, Quaternion.identity);
        newEnemy.GetComponent<Follower>().pathCreator = path;
        newEnemy.GetComponent<Animator>().enabled = true;
        newEnemy.GetComponent<KTD_Enemy>().end_x = end_coords.x;
        newEnemy.GetComponent<KTD_Enemy>().end_y = end_coords.y;
        //newEnemy.gameObject.name = "Enemy";
        habCount++;
        if (habCount >= habvar)
        {
            habCount = 0;
            CancelInvoke("SpawnHAB");
            hotairball_enabled = false;
        }
    }

    private void SpawnBandit()
    {
        Transform newEnemy = Instantiate(GameAssets.i.pfBandit, mapSpawn, Quaternion.identity);
        newEnemy.GetComponent<Follower>().pathCreator = path;
        newEnemy.GetComponent<Animator>().enabled = true;
        newEnemy.GetComponent<KTD_Enemy>().end_x = end_coords.x;
        newEnemy.GetComponent<KTD_Enemy>().end_y = end_coords.y;
        //newEnemy.gameObject.name = "Enemy";
        banditCount++;
        if (banditCount >= banditvar)
        {
            banditCount = 0;
            CancelInvoke("SpawnBandit");
            bandit_enabled = false;
        }
    }

    private void SpawnArchmage()
    {
        Transform newEnemy = Instantiate(GameAssets.i.pfArchmage, mapSpawn, Quaternion.identity);
        newEnemy.GetComponent<Follower>().pathCreator = path;
        newEnemy.GetComponent<Animator>().enabled = true;
        newEnemy.GetComponent<KTD_Enemy>().end_x = end_coords.x;
        newEnemy.GetComponent<KTD_Enemy>().end_y = end_coords.y;
        //newEnemy.gameObject.name = "Enemy";
        archmCount++;
        if (archmCount >= archmvar)
        {
            archmCount = 0;
            CancelInvoke("SpawnArchmage");
            archmage_enabled = false;
        }
    }

    private void SpawnGiant()
    {
        Transform newEnemy = Instantiate(GameAssets.i.pfGiant, mapSpawn, Quaternion.identity);
        newEnemy.GetComponent<Follower>().pathCreator = path;
        newEnemy.GetComponent<Animator>().enabled = true;
        newEnemy.GetComponent<KTD_Enemy>().end_x = end_coords.x;
        newEnemy.GetComponent<KTD_Enemy>().end_y = end_coords.y;
        //newEnemy.gameObject.name = "Enemy";
        giantCount++;
        if (giantCount >= giantvar)
        {
            giantCount = 0;
            CancelInvoke("SpawnGiant");
            giant_enabled = false;
        }
    }

    private void SpawnDragon()
    {
        Transform newEnemy = Instantiate(GameAssets.i.pfDragon, mapSpawn, Quaternion.identity);
        newEnemy.GetComponent<Follower>().pathCreator = path;
        newEnemy.GetComponent<Animator>().enabled = true;
        newEnemy.GetComponent<KTD_Enemy>().end_x = end_coords.x;
        newEnemy.GetComponent<KTD_Enemy>().end_y = end_coords.y;
        //newEnemy.gameObject.name = "Enemy";
        dragonCount++;
        if (dragonCount >= dragonvar)
        {
            dragonCount = 0;
            CancelInvoke("SpawnDragon");
            dragon_enabled = false;
        }
    }

    private void SpawnDemon()
    {
        Transform newEnemy = Instantiate(GameAssets.i.pfDemon, mapSpawn, Quaternion.identity);
        newEnemy.GetComponent<Follower>().pathCreator = path;
        newEnemy.GetComponent<Animator>().enabled = true;
        newEnemy.GetComponent<KTD_Enemy>().end_x = end_coords.x;
        newEnemy.GetComponent<KTD_Enemy>().end_y = end_coords.y;
        //newEnemy.gameObject.name = "Enemy";
        demonCount++;
        if (demonCount >= demonvar)
        {
            demonCount = 0;
            CancelInvoke("SpawnDemon");
            demon_enabled = false;
        }
    }


    public void startWave()
    {
        skeletons_enabled = true;
        doneSpawning = false;
        if (wave >= 3)
        {
            gladiators_enabled = true;
        }
        if (wave >= 10)
        {
            hotairball_enabled = true;
        }
        if (wave >= 20)
        {
            bandit_enabled = true;
        }
        if (wave >= 25)
        {
            archmage_enabled = true;
        }
        if (wave >= 32)
        {
            giant_enabled = true;
        }
        if (wave >= 50)
        {
            dragon_enabled = true;
        }
        if (wave >= 95)
        {
            demon_enabled = true;
        }

        if (wave == 1)
        {
            skelevar = 10; // Set number of gladiators
            spawnSpeed = 2.0f;


        }
        else if (wave > 1 && wave <= 10)
        {
            skelevar = 2 * wave + 10;
            spawnSpeed = 1.85f;

        }
        else if (wave > 10 && wave <= 20)
        {
            skelevar = wave + ((wave / 2) * 3);

        }
        else
        {
            skelevar = wave + ((wave / 2) * 3);
            spawnSpeed = spawnSpeed - 0.02f;

        }
        ////Debug.Log("Spawning " + skelevar + " Skeletons");
        InvokeRepeating("SpawnSkele", 1f, 0.00001f + spawnSpeed);

        if (gladiators_enabled)
        {

            gladvar = (wave + (wave / 2));
            ////Debug.Log("Spawning " + gladvar + " Gladiators");
            InvokeRepeating("SpawnGladiator", 1f, 0.00001f + spawnSpeed + 1f);

        }
        if (hotairball_enabled)
        {
            habvar = (wave + (wave / 3));
            ////Debug.Log("Spawning " + habvar + " Hot Air Balloons");
            InvokeRepeating("SpawnHAB", 1f, 0.00001f + spawnSpeed + 1f);
        }

        if (bandit_enabled)
        {
            banditvar = ((wave / 5) + 5);
            ////Debug.Log("Spawning " + banditvar + " Bandits");
            InvokeRepeating("SpawnBandit", 1f, 0.00001f + spawnSpeed + 5f);
        }

        if (archmage_enabled)
        {
            archmvar = (wave % 50);
            ////Debug.Log("Spawning " + archmvar + " Archmages");
            InvokeRepeating("SpawnArchmage", 1f, 0.00001f + spawnSpeed);
        }

        if (giant_enabled)
        {
            giantvar = (wave / 2);
            ////Debug.Log("Spawning " + giantvar + " Giants");
            InvokeRepeating("SpawnGiant", 1f, 0.00001f + spawnSpeed + 0.5f);
        }

        if (dragon_enabled)
        {
            demonvar = ((wave * 2) / 3);
            if (wave < 75)
            {
                dragonvar = (wave % 3);
            }
            else
            {
                dragonvar = (wave % 17);
            }
            ////Debug.Log("Spawning " + dragonvar + " Dragons");
            InvokeRepeating("SpawnDragon", 1f, 0.00001f + spawnSpeed + 3.5f);

        }

        if (demon_enabled)
        {
            demonvar = (wave - 92);
            if (wave > 110)
            {
                if (wave % 2 == 0)
                {
                    demonvar = ((wave / 10) + 3);
                }
                else
                {
                    demonvar = ((wave / 10) - 5);
                }
            }
            ////Debug.Log("Spawning " + demonvar + " Demons");
            InvokeRepeating("SpawnDemon", 1f, 0.00001f + spawnSpeed + 5.5f);
        }
        UpdateCounter();
        wave++; // Increment wave.


    }
}
