using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artorias : PlayerInput
{
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
        MoveInput();
        //改成监听AddHP
        // if(am.sm.hp <= am.sm.hp / 2.0f)
        // {
        //     phrth2 = true;
        // }
        // distance = Vector3.Distance(transform.position,player_sm.transform.position);

        // if(againstTime > 3.0f)
        //     Roll();
        // if(player_sm)
        // {            
        //     if(distance > 5.0f)
        //     {
        //         forwardAxis = 0.0f;
        //         Charge();
        //     }
        //     if(distance < 3.0f)
        //     {
        //         forwardAxis = 0.0f;
        //         againstTime += Time.deltaTime;
        //         NormalAttack();
        //     }

        //     else if(distance < 5.0f)
        //     {
        //         forwardAxis = 0.0f;
        //         Charge();
        //     }

        //     else 
        //     {
        //         if(am.sm.isAttack) return;
        //         ac.animator.SetFloat("forward",1.0f);
        //         Quaternion r = Quaternion.LookRotation(player_sm.transform.position - transform.position,Vector3.up);
        //         transform.rotation = Quaternion.Lerp(transform.rotation,r,Time.deltaTime * 2.0f);
        //         ac.model.transform.rotation = transform.rotation;
        //     }

        // }
    }

    void Charge()
    {
        int r = Random.Range(0,10);
        if(animator.GetCurrentAnimatorStateInfo(0).IsTag("attack")) return;
        if(r < 3)
        {
            animator.Play("c4100_strike");           
        }
        else if(r < 7)
        {
            animator.Play("c4100_wolfAttack");
        }
        else
        {
            animator.Play("c4100_slowWolfAttack");
        }
    }

    void NormalAttack()
    {
        ac.Attack();
    }

    void Roll()
    {
        animator.CrossFade("c4100_attackRollBack",0.1f);
        againstTime = 0.0f;
    }

    void LeakOut()
    {
        am.sm.rhATK.physical *= 2.0f;
        animator.CrossFade("c4100_gasLeakOut",0.1f);
    }

    private void OnDisable() 
    {

    }
}
