using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCasterManager : IActorManagerInterface
{
    public InterractionEvent interractionEvent;
    public bool active;
    public Vector3 offset = new Vector3(0,0,1);
    
    // Start is called before the first frame update
    void Start()
    {
        if(am == null)
        {
            am = GetComponentInParent<ActorManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 FixedPosition()
    {
        return transform.position + offset;
    }
}
