using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KTD_MortarCannonball : MonoBehaviour
{
    private Vector3 targetPos;

    private Vector3 start;
    
    public Sprite explosion;

    public int damage;

    public RuntimeAnimatorController explode_build;
    float moveSpeed = 20f;
    
    float selfDestDist = 1f;

    private float maxTime;


    // Start is called before the first frame update
    void Start()
    {
        maxTime = 0.85f;
    }


    public static void Create(Vector3 spawnPos, Vector3 targetPos, int damage) {
        Transform arrowTransform = Instantiate(GameAssets.i.pfMortarCannonball, spawnPos, Quaternion.identity);
        arrowTransform.GetComponent<SpriteRenderer>().sortingLayerName = "Animations";
        KTD_MortarCannonball arrow = arrowTransform.GetComponent<KTD_MortarCannonball>();
        arrow.damage = damage;
        arrow.Setup(targetPos);
    }

    private void Setup(Vector3 targetPos)
    {
        this.targetPos = targetPos;
    }

    void FixedUpdate() {
        maxTime -= Time.deltaTime;
        if (maxTime <= 0f) {
            Vector3 blast_pos = this.gameObject.transform.position;
            Destroy(this.gameObject);
            GameObject blast = new GameObject();
            blast.name = "Explosion";
            blast.tag = "Projectile";
            blast.AddComponent<CircleCollider2D>();
            blast.GetComponent<CircleCollider2D>().isTrigger = true;
            blast.GetComponent<CircleCollider2D>().radius = 5;
            blast.AddComponent<SpriteRenderer>();
            blast.AddComponent<SpriteRenderer>();
            SpriteRenderer sprite = blast.GetComponent<SpriteRenderer>();
            sprite.sprite = explosion;
            sprite.sortingLayerName = "MG";
            blast.AddComponent<Animator>();
            blast.GetComponent<Animator>().runtimeAnimatorController = explode_build;
            blast.transform.position = blast_pos;
        }
    }


    private void Update() 
    {

        Vector3 moveDir = (targetPos - transform.position).normalized;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
        float angle = detAngle(moveDir);
        transform.eulerAngles = new Vector3(0,0, angle);
        if (Vector3.Distance(transform.position, targetPos) < selfDestDist)
        {
            Destroy(gameObject);
            GameObject blast = new GameObject();
            blast.name = "Explosion";
            blast.tag = "Projectile";
            blast.transform.localScale = new Vector3(5,5,0);
            blast.AddComponent<CircleCollider2D>();
            blast.GetComponent<CircleCollider2D>().isTrigger = true;
            blast.GetComponent<CircleCollider2D>().offset = new Vector2(0f,-0.3f);
            blast.GetComponent<CircleCollider2D>().radius = 1;
            blast.AddComponent<SpriteRenderer>();
            SpriteRenderer sprite = blast.GetComponent<SpriteRenderer>();
            sprite.sprite = explosion;
            sprite.sortingLayerName = "MG";
            blast.AddComponent<Animator>();
            blast.GetComponent<Animator>().runtimeAnimatorController = explode_build;
            blast.transform.position = gameObject.transform.position;
        }
        
    }

    public static float detAngle(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }

    void OnCollisionStay2D(Collision2D other) {
        if (other.gameObject.tag == "Tower") {
            other.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        }
    }

    void OnCollisionExit2D(Collision2D other) {
        if (other.gameObject.tag == "Tower") {
            other.gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        }
    }
}
