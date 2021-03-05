using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneFire : MonoBehaviour
{
    public ParticleSystem burstFire;
    public ParticleSystem flame;
    public bool lit = false;
    public EventCasterManager boneFireEvent;
    
    private void Start()
    {
        boneFireEvent = GetComponentInChildren<EventCasterManager>();

        if (lit)
        {
            Fire();
            boneFireEvent.interractionEvent = InterractionEvent.BornFireSit;
        }
    }

    public void FireBurst()
    {
        burstFire.Play();
        lit = true;
    }

    public void Fire()
    {
        flame.Play();
        boneFireEvent.interractionEvent = InterractionEvent.BornFireSit;
    }
}
