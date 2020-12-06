using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artorias : ActorInput
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
        //if (!enableInput) return;
        // 改成监听AddHP
        if(am.sm.hp <= am.sm.hp / 2.0f)
        {
            phrth2 = true;
        }
        distance = Vector3.Distance(transform.position,player_sm.transform.position);
        
        // if(againstTime > 3.0f)
        //     WolfxRollBack();
        if(player_sm)
        {
            MoveToPlayerPos();
        }
    }

    private void MoveToPlayerPos()
    {
        movingVec = Vector3.forward;
        Quaternion r = Quaternion.LookRotation(player_sm.transform.position - transform.position, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, r, Time.deltaTime * 2.0f);
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
    
    void WolfAttackx3()
    {
        animator.SetTrigger("wolfx3");
    }

    void WolfxRollBack()
    {
        animator.SetTrigger("wolfBack");
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
