using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KTD_Javelin : MonoBehaviour
{
    private Vector3 targetPos;

    public string proj;

    public int damage;
    float moveSpeed = 20f;

    float selfDestDist = 1f;

    private float maxTime;
    // Start is called before the first frame update
    void Start()
    {
        maxTime = 1f;
    }
    public static void Create(Vector3 spawnPos, Vector3 targetPos, int damage, bool aerial)
    {
        Transform arrowTransform = Instantiate(GameAssets.i.pfJavelin, spawnPos, Quaternion.identity);
        arrowTransform.GetComponent<SpriteRenderer>().sortingLayerName = "Animations";
        KTD_Javelin arrow = arrowTransform.GetComponent<KTD_Javelin>();
        arrow.damage = damage;
        if (aerial)
        {
            arrow.name = "pfAerialJavelin(Clone)";
        }
        arrow.Setup(targetPos);
    }

    private void Setup(Vector3 targetPos)
    {
        this.targetPos = targetPos;
    }

    void FixedUpdate()
    {
        maxTime -= Time.deltaTime;
        if (maxTime <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        Vector3 moveDir = (targetPos - transform.position).normalized;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
        float angle = detAngle(moveDir);
        transform.eulerAngles = new Vector3(0, 0, angle);
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
