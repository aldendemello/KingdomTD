using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class Follower : MonoBehaviour
{
    public PathCreator pathCreator;
    public EndOfPathInstruction end; 
    public float speed;
    float dstTravelled;

    void Update()
    {
        try {
            dstTravelled += (speed * Time.deltaTime);
        transform.position = pathCreator.path.GetPointAtDistance(dstTravelled, end);
        } catch {}
        
    }

}
