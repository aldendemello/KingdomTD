using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Add camera movement when Halstein moves
// Add level up functionality
// Add attacking
// Add building
// Add movement
// Add Health
// Add Strength
public class KTD_Halstein : MonoBehaviour
{

    public float speed = 3.0f;

    private float totalXRot = 0f;

    private float totalYRot = 0f;

    public GameObject currentTower;


    // Start is called before the first frame update
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        totalXRot += Input.GetAxis("Horizontal") * speed;
        totalYRot += Input.GetAxis("Vertical") * speed;
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    void FixedUpdate()
    {
        
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if(Input.GetKey(KeyCode.A)) 
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector2.up * speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector2.down * speed * Time.deltaTime);
        }
        else
        {
            Debug.Log("Not moving");
        }

    }
}
