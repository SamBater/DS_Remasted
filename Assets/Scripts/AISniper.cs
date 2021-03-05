using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AISniper : AINormal
{
    new enum SniperFSMState 
    {
        idle,draw,shoot,load,dead
    }
    [SerializeField]
    SniperFSMState state;
    [SerializeField]
    private Transform shootPotint;

    public Vector3 shootFixPos;
    private void Start() 
    {
        ac = GetComponent<ActorController>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        state = SniperFSMState.idle;
    }
    private void Update() 
    {
        float distance = Vector3.Distance(transform.position,player.position);
        if (SpottedPlayer())
        {
            Aiming();
            Debug.Log("Time to aimming");
        }
    }

    public void Aiming()
    {
        Vector3 dir = player.position - (transform.position + shootFixPos);
        float angle = Vector3.Angle(dir,transform.forward);

        if (Mathf.Abs(angle) >= 5.0f && ac.enableTurnDirection)
        {
            SetMoveDirectionAndInputMag(player,ac.turnDirectionSpeed);
        }
        else
        {
            ac.Attack(45);
        }
    }
}
