using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AISniper : ActorInput
{
    enum FSMState 
    {
        idle,draw,shoot,load,dead
    }
    float shootRange = 10.0f;
    float distanceAI_player;
    Transform player;
    [SerializeField]
    float attackCd = 1.5f;
    float attackInterval;
    FSMState state;
    float atkTime;
    [SerializeField]
    private Transform shootPotint;

    private void Start() 
    {
        ac = GetComponent<ActorController>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        state = FSMState.idle;
    }
    private void Update() 
    {
        ac.animator.SetInteger("attackMotionType",45);
        distanceAI_player = Vector3.Distance(transform.position,player.position);
        Aiming();
    }

    public void Aiming()
    {
        ac.Attack();
        Vector3 dir = player.position;
        dir.y = 0;
        transform.LookAt(player.position);
        shootPotint.LookAt(player.position + 1.0f * Vector3.up);
    }
}
