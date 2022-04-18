using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KTD_PlayDeath : MonoBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    void Start()
    {
        AudioSource death = GetComponent<AudioSource>();
        death.Play();
    }
}
