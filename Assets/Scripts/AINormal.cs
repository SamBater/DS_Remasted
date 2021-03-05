﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


 public class AINormal : ActorInput
 {
     public enum FSMState
     {
         idle,
         patrol,
         chase,
         dead,
         agianst,
         attack,
         defence
     }

     public Transform player;
     public StateManager player_sm;
     public Transform dest;
    [Range(1,32)]
    public float rotateSpeed;
    public bool turnToTarget;
     NavMeshAgent agent;
     public Transform[] partorlPoints;
     private int currentPratrolPoints;
     public FSMState currentState;
     float disBetweenAI_Player = 0;
     [SerializeField] float againstMaxCD; //最大对抗时间

     [SerializeField] float againstCD;
     [SerializeField] float againstTime; //实际对抗实际

     public float attackDistance = 3.0f;
     public float chaseDistance = 10.0f;

     
     [Range(15,90)]public float viewAngle;
     [Range(5,20)]public float viewDistance;
     private void Start()
     {
         agent = GetComponent<NavMeshAgent>();
         player = GameObject.FindGameObjectWithTag("Player").transform;
         player_sm = GameObject.FindGameObjectWithTag("Player").GetComponent<StateManager>();
         am = GetComponent<ActorManager>();
         ac = am.ac;

         if (partorlPoints.Length == 0)
             currentState = FSMState.idle;
         else
             currentState = FSMState.patrol;
     }

     private void Update()
     {
         // if(!enableInput) return;
         if(am.sm.isDead) return;
         disBetweenAI_Player = Vector3.Distance(player.position, transform.position);
         switch (currentState)
         {
             case FSMState.patrol:
                 Partrol();
                 break;

             case FSMState.chase:
                 Chase();
                 break;

             case FSMState.attack:
                 Attack();
                 break;

             case FSMState.agianst:
                 Holding();
                 break;

             case FSMState.defence:
                 Defence();
                 break;

             default:

                 //Debug.LogError("Did not exsist this state.");
                 break;
         }
     }

     private void Defence()
     {
         if (am.sm.Naili / am.sm.maxEndurance < 0.6)
         {
             ac.Defence();
         }

         if (am.sm.Naili / am.sm.maxEndurance < 0.2)
         {
             ac.DefenceOff();
         }

         if (player_sm.isHeal)
         {
             ac.DefenceOff();
             currentState = FSMState.attack;
             return;
         }

         if (player_sm.isAttack)
         {
             ac.DefenceOff();
             
         }
     }

     void Partrol()
     {
         agent.isStopped = false;
         MoveForward(0.4f);
         //agent.speed = ac.walkSpeed * 0.4f;
         Vector3 destPos = partorlPoints[currentPratrolPoints].position;
         float distance = Vector3.Distance(destPos, transform.position);
         agent.destination = destPos;
         if (distance <= 0.5f)
         {
             currentPratrolPoints++;
             currentPratrolPoints = currentPratrolPoints % partorlPoints.Length;
         }

         if (disBetweenAI_Player < chaseDistance)
         {
             currentState = FSMState.chase;
             return;
         }
         //进入攻击范围
         else if (disBetweenAI_Player <= attackDistance)
         {
             currentState = FSMState.attack;
             MoveForward(0);
             agent.isStopped = true;
             ac.Attack();
             return;
         }

     }

     /// <summary>
     /// 僵持阶段
     /// </summary>
     void Holding()
     {
         againstTime += Time.deltaTime;

         MoveRight(0.5f);

         if (ac.lockOnTarget == null)
         {
             SetMoveDirectionAndInputMag(player,12.0f);
         }

         if (againstCD < 0.01)
         {
             againstCD = Random.Range(1.0f, againstMaxCD);
         }

         if (disBetweenAI_Player > attackDistance)
         {
             currentState = FSMState.chase;
             return;
         }

         //将决策时间随机化
         if (againstTime > againstCD)
         {
             againstTime = againstCD;
             MoveRight(0);
             //pressOnLB = false;
             ac.DefenceOff();
             if (disBetweenAI_Player > attackDistance)
             {
                 currentState = FSMState.chase;
                 Debug.Log("against -> chase");
                 return;
             }
             else
             {
                 currentState = FSMState.attack;
                 return;
             }
         }
         else if (player_sm.isAttack)
         {
             Debug.Log("该防御了!");
             currentState = FSMState.defence;
             againstTime = againstCD = 0;
             MoveRight(0);
             return;
         }
         else if (player_sm.isHeal)
         {
             Debug.Log("重拳出击!");
             againstTime = againstCD = 0;
             MoveRight(0);
             MoveForward(0);
             agent.isStopped = true;
             ac.DefenceOff();
             currentState = FSMState.attack;
             return;
         }
     }

     /// <summary>
     /// 攻击条件：在攻击范围内；耐力大于；在前方
     /// </summary>
     void Attack()
     {
         agent.isStopped = true;
         if(ac.enableTurnDirection)
             SetMoveDirectionAndInputMag(player,12.0f);
         
         if (am.sm.Naili / am.sm.maxEndurance > 0.5)
         {
             ac.Attack();
         }

         else
         {
             currentState = FSMState.defence;
             return;
         }

         if (!am.sm.isAttack)
         {
             if (disBetweenAI_Player > attackDistance)
             {
                 currentState = FSMState.chase;
                 return;
             }
         }
     }

     void Chase()
     {
         if (am.sm.isAttack) return;
         if (disBetweenAI_Player > chaseDistance)
         {
             currentState = FSMState.patrol;
             MoveForward(0f);
             return;
         }

         else if (disBetweenAI_Player <= attackDistance)
         {
             currentState = FSMState.attack;
             agent.isStopped = true;
             MoveForward(0f);
             ac.Attack();
             return;
         }

         MoveForward(1.0f);
         agent.speed = ac.walkSpeed;
         agent.isStopped = false;
         agent.destination = player.position;
         //SetMoveDirectionAndInputMag(player.position,1.0f);
     }

     public void SetMoveDirectionAndInputMag(Transform target, float speed)
     {
         Quaternion r = Quaternion.LookRotation(target.position - am.sm.gameObject.transform.position,Vector3.up);
         r.x = r.z = 0.0f;
         am.sm.gameObject.transform.rotation = Quaternion.Lerp(am.sm.gameObject.transform.rotation,r,Time.deltaTime * speed);
     }
     
     public void SetMoveDirectionAndInputMag(Vector3 target,float speed)
     {
         Quaternion r = Quaternion.LookRotation(target - am.sm.gameObject.transform.position,Vector3.up);
         r.x = r.z = 0.0f;
         transform.rotation = Quaternion.Lerp(transform.rotation,r,Time.deltaTime * speed);
     }

     public bool SpottedPlayer()
     {
         Vector3 dir = player.position - transform.position;
         float angle = Vector3.Angle(transform.forward, dir);
         bool inViewField = angle <= viewAngle / 2;
         RaycastHit hitinfo;
         int playerMask = LayerMask.GetMask("Player");
         int defaultMask = LayerMask.GetMask("Default");
         int both = (1 << playerMask) | (1 << defaultMask); 
         Physics.Raycast(transform.position + Vector3.up, dir, out hitinfo, viewDistance,both);
         Debug.DrawRay(transform.position + Vector3.up,dir,Color.red);
         inViewField = inViewField && hitinfo.collider == null;
         return inViewField;
     }
 }
