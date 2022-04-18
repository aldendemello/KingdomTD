using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KTD_Fireball : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 targetPos;


    public Sprite explosion;

    public int damage;

    public RuntimeAnimatorController explode_build;
    float moveSpeed = 20f;
    
    float selfDestDist = 1f;
    // Start is called before the first frame update
    private float maxTime;

    void Start()
    {
        maxTime = 0.4f;    
        damage = 4;
    }
    public static void Create(Vector3 spawnPos, Vector3 targetPos, int damage) {
        Transform arrowTransform = Instantiate(GameAssets.i.pfFireball, spawnPos, Quaternion.identity);
        arrowTransform.GetComponent<SpriteRenderer>().sortingLayerName = "Animations";
        
        KTD_Fireball arrow = arrowTransform.GetComponent<KTD_Fireball>();
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
            Destroy(this.gameObject);
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
        }
    }

    public static float detAngle(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 90;
        return n;
    }
}

