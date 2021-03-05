using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artorias : AINormal
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
        SlowWolfAtk = 9,
        wolfcharge_slashback_charge = 10
    }
    private bool phrth2 = false;
    public float distance;
    private void Start() 
    {
        player_sm = GameObject.FindWithTag("Player").GetComponent<StateManager>();
        EnableInput = true;
    }
    private void Update()
    {
        Attack((Action)9);
        if(ac.enableTurnDirection)
        {
            SetMoveDirectionAndInputMag(player_sm.transform,rotateSpeed);
        }
        distance = Vector3.Distance(transform.position,player_sm.transform.position);
        if(am.sm.hp <= am.sm.hp / 2.0f)
        {
            phrth2 = true;
        }

                return;
        if (!EnableInput) return;
        if(player_sm)
        {
            float r = Random.Range(0, 1.25f);

            // 10m之外
            if(distance > 10f)
            {
                running = true;
                MoveForward(distance > 12.0 ? 2.0f : 1.0f );
            }
            
            //5米~10米
            else if (distance > 5.0f)
            {
                if(am.sm.Naili / am.sm.maxEndurance < 0.3f)
                {
                    SetMoveDirectionAndInputMag(player_sm.transform,12.0f);
                    MoveForward(1.0f);
                    return;
                }
                if( r < 0.25) Attack(Action.wolfcharge_slashback_charge);
                else if( r < 0.5) Attack(Action.Charge);
                else if( r < 0.75) Attack(Action.RunSlash);
                else if(r < 1.0f) Attack(Action.WolfAtkX3);
                else if(r < 1.25f) Attack(Action.RunAttack);
            }
            //5米内
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
        MovingVec = Vector3.forward;
        Quaternion r = Quaternion.LookRotation(player_sm.transform.position - transform.position, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, r, Time.deltaTime * 2.0f);
    }

    private void Attack(Action actionID)
    {
        ac.Attack((int)actionID);
    }

    public void SetMoveDirectionAndInputMag(Transform target, float speed)
    {
        Quaternion r = Quaternion.LookRotation(target.position - transform.position,Vector3.up);
        r.x = r.z = 0.0f;
        transform.rotation = Quaternion.Lerp(transform.rotation,r,Time.deltaTime * speed);
    }
    
}
