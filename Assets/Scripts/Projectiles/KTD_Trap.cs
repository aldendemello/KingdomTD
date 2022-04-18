using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KTD_Trap : MonoBehaviour
{
    public int damage;
    public int health;
    public int rounds;

    // Start is called before the first frame update
    void Start()
    {
        health = 2;
        damage = 10;
        rounds = 0;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (other.gameObject.GetComponent<KTD_Enemy>().isAerial == false)
            {
                health -= 1;
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (rounds >= 3 || health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
