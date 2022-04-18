using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KTD_Cannonball : MonoBehaviour
{
    private Vector3 targetPos;


    public Sprite explosion;

    public RuntimeAnimatorController explode_build;
    float moveSpeed = 20f;
    
    float selfDestDist = 1f;
    // Start is called before the first frame update
    private float maxTime;

    public int damage;


    void Start()
    {
        maxTime = 0.4f;    
    }
    public static void Create(Vector3 spawnPos, Vector3 targetPos, Animator anim, int damage, bool camo) {
        anim.SetInteger("blast",1);
        Transform arrowTransform = Instantiate(GameAssets.i.pfCannonball, spawnPos, Quaternion.identity);
        arrowTransform.GetComponent<SpriteRenderer>().sortingLayerName = "Animations";

        KTD_Cannonball arrow = arrowTransform.GetComponent<KTD_Cannonball>();
        arrow.damage = damage;
        if (camo)
        {
            arrow.name = "pfCamoCannonball(Clone)";
        }
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
            blast.AddComponent<SpriteRenderer>();
            SpriteRenderer sprite = blast.GetComponent<SpriteRenderer>();
            sprite.sprite = explosion;
            sprite.sortingLayerName = "Animations";
            blast.AddComponent<Animator>();
            blast.GetComponent<Animator>().runtimeAnimatorController = explode_build;
            blast.transform.position = blast_pos;
        }
    }

    private void Update() {
        Vector3 moveDir = (targetPos - transform.position).normalized;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
        float angle = detAngle(moveDir);
        
        transform.eulerAngles = new Vector3(0,0, angle);
        if (Vector3.Distance(transform.position, targetPos) < selfDestDist)
        {
            Destroy(gameObject);
            GameObject blast = new GameObject();
            blast.name = "Explosion";
            blast.AddComponent<SpriteRenderer>();
            SpriteRenderer sprite = blast.GetComponent<SpriteRenderer>();
            sprite.sprite = explosion;
            sprite.sortingLayerName = "Animations";
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
}
