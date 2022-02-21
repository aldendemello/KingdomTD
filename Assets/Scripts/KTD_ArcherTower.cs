using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KTD_ArcherTower : MonoBehaviour
{

    public GameObject player;

    public Sprite towerSprite;

    public const string LAYER = "FG";

    private int placecount;

    private GameObject newTower;

    private bool isnt_Placed;

    private bool tower_place_init;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (tower_place_init && Input.GetMouseButton(0))
        {
            isnt_Placed = false;
            
        }
        if (isnt_Placed && tower_place_init)
        {
            newTower.transform.position = new Vector3(player.transform.position.x + 2, player.transform.position.y + 5, player.transform.position.z);
        } else if (tower_place_init) {
            newTower.transform.position = new Vector3(player.transform.position.x + 2, player.transform.position.y + 5, player.transform.position.z);
            newTower.AddComponent<BoxCollider2D>();
            newTower.GetComponent<BoxCollider2D>().size = new Vector2(1.4f, 0.75f);
            newTower.GetComponent<BoxCollider2D>().offset = new Vector2(0f, -1.15f);

            SpriteRenderer s_r = newTower.GetComponent<SpriteRenderer>();
            s_r.color = new Color(1f,1f,1f,1f);
            tower_place_init = false;
        }
        
    }

    public void PlaceTower()
    {
        tower_place_init = true;
        newTower = new GameObject();
        newTower.name = "ArcherTower";
        newTower.AddComponent<SpriteRenderer>();
        SpriteRenderer spriteR = newTower.GetComponent<SpriteRenderer>();
        spriteR.sprite = towerSprite;
        spriteR.color = new Color(1f,1f,1f,.5f);
        spriteR.sortingLayerName = LAYER;
        newTower.transform.localScale = new Vector3(2.5f, 2.5f, 0);
        newTower.AddComponent<KTD_ArcherTower>();
        isnt_Placed = true;    
    }
}
