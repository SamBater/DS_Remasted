using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artorias : ActorInput
{
    enum Action
    {
        NormalAttack = 0,
        Charge = 1,
        WolfAtk = 2,
        RunSlash = 3,
        HeavyAttack = 4,
        RunAttack = 5,
        WolfAtkX3 = 6,
        SlashBack = 7,
        SwordRotation360 = 8,
        wolfcharge_slashback_charge = 10
    }
    private Animator animator;
    private bool phrth2 = false;
    public float distance;
    private ActorManager player_sm;
    public float againstTime = 0;
    
    private void Start() 
    {
        animator = transform.GetChild(0).gameObject.GetComponent<Animator>();
        player_sm = GameObject.FindWithTag("Player").GetComponent<ActorManager>();
        //ac.cc.LockOnToggle();
    }
    private void Update()
    {
        distance = Vector3.Distance(transform.position,player_sm.transform.position);
        againstTime += distance < 5.0f ? Time.deltaTime : 0.0f;
        if(am.sm.hp <= am.sm.hp / 2.0f)
        {
            phrth2 = true;
        }

        if (!enableInput) return;
        if(player_sm)
        {
            float r = Random.Range(0, 1.25f);
            if (distance > 5.0f)
            {
                if( r < 0.25) Attack(Action.wolfcharge_slashback_charge);
                else if( r < 0.5) Attack(Action.Charge);
                else if( r < 0.75) Attack(Action.RunSlash);
                else if(r < 1.0f) Attack(Action.WolfAtkX3);
                else if(r < 1.25f) Attack(Action.RunAttack);
            }
            else
            {
                if(r < 0.2f) Attack(Action.SwordRotation360);
                else if(r < 0.5f) Attack(Action.SlashBack);
                else if( r < 0.7f)Attack(Action.WolfAtk);
                else if(r < 1.0f) Attack(Action.HeavyAttack);
                else Attack(Action.NormalAttack);
            }
        }
    }

    private void MoveToPlayerPos()
    {
        movingVec = Vector3.forward;
        Quaternion r = Quaternion.LookRotation(player_sm.transform.position - transform.position, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, r, Time.deltaTime * 2.0f);
    }

    private void Attack(Action actionID)
    {
        ac.Attack((int)actionID);
    }
    
}
