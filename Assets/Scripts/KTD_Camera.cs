using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KTD_Camera : MonoBehaviour
{
    public Transform player;

    public Vector3 offset;

    private Camera cam;

    private float targetZoom;

    private float zoomFactor = 3;

    private float zoomLerp = 10;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        targetZoom = cam.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        float scrollData;
        scrollData = Input.GetAxis("Mouse ScrollWheel");
        float oldTarget = targetZoom;
        targetZoom -= scrollData * zoomFactor;  
        float oldSize = cam.orthographicSize;
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * zoomLerp);
        if (targetZoom > 10 || targetZoom < 5)
        {
            cam.orthographicSize = oldSize;
            targetZoom = oldTarget;
        }
    }

    void FixedUpdate()
    {
        transform.position = new Vector3 (player.position.x + offset.x, player.position.y + offset.y, offset.z);
    }
}
