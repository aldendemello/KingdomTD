using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PathCreation;

public class KTD_kingdom : MonoBehaviour
{
    public int health;

    public TextMeshProUGUI heart;

    public GameObject lostUI;

    public GameObject gameUI;

    public GameObject wave;

    public PathCreator path;

    public float end_x;
    public float end_y;

    public AudioSource upgradeSound;
    // Start is called before the first frame update
    void Start()
    {
        health = 100;

        upgradeSound = GameObject.Find("UI").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        int.TryParse(heart.gameObject.GetComponent<TextMeshProUGUI>().text, out health);
        if (health <= 0)
        {
            gameUI.SetActive(false);

            int length = wave.gameObject.GetComponent<TextMeshProUGUI>().text.Length;
            string wavenum = wave.gameObject.GetComponent<TextMeshProUGUI>().text.Substring(6, length - 6);
            lostUI.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "You Reached Wave: " + wavenum;
            Time.timeScale = 0f;
            lostUI.SetActive(true);
        }
    }

    public void TakeDamage(int amount)
    {
        TextMeshProUGUI hVals = heart.GetComponent<TextMeshProUGUI>();
        hVals.text = (health - amount).ToString();
        health -= amount;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "AerialEnemy" || other.gameObject.tag == "CamoEnemy" || other.gameObject.tag == "AerialEnemy(C)")
        {
            TakeDamage(other.gameObject.GetComponent<KTD_Enemy>().damage);
            Destroy(other.gameObject);
        }
    }
}
