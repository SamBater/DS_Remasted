using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TmpInput : ActorInput
{
    public bool atkToggle;
    // Start is called before the first frame update
    void Start()
    {
        am = GetComponent<ActorManager>();
        ac = am.ac;
    }

    // Update is called once per frame
    void Update()
    {
        if (atkToggle)
        {
            ac.Attack();
        }
    }
}
