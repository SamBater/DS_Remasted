using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AINormal : PlayerInput
{
    public enum FSMState
    {
        idle,patrol,chase,dead,agianst,attack,defence
    }
    public Transform player;
    public StateManager player_sm;
    public Transform dest;
    NavMeshAgent agent;
    public Transform[] partorlPoints;
    private int currentPratrolPoints;
    public FSMState currentState;
    float disBetweenAI_Player = 0;
    public float attackTime ;
    [SerializeField]
    float attackCD = 1.5f;
    float attackMaxCD = 3.0f;
    [SerializeField]
    float againstMaxCD;    //最大对抗时间

    [SerializeField]
    float againstCD;
    [SerializeField]
    float againstTime;  //实际对抗实际

    public float attackDistance = 3.0f;
    public float chaseDistance = 10.0f;
    private float defenceTime;

    private void Start() {
        agent = GetComponent<NavMeshAgent>(); 
        player = GameObject.FindGameObjectWithTag("Player").transform;
        player_sm = GameObject.FindGameObjectWithTag("Player").GetComponent<StateManager>();


        if(partorlPoints.Length == 0)
            currentState = FSMState.idle;
        else
            currentState = FSMState.patrol;
    }
    private void Update() 
    {
       // if(!enableInput) return;
        MoveInput();
        disBetweenAI_Player = Vector3.Distance(player.position,transform.position);
        switch(currentState)
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
            Agianst();
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
        defenceTime += Time.deltaTime;
        if(defenceTime < 2.0f || player_sm.isAttack)
        {
            pressOnLB = true;
        }
        else if(disBetweenAI_Player > attackDistance)
        {
            pressOnLB = false;
            defenceTime = 0.0f;
            currentState = FSMState.chase;
        }
        else if(disBetweenAI_Player < attackDistance)
        {
            pressOnLB = false;
            defenceTime = 0.0f;
            currentState = FSMState.attack;
        }
    }

    void Partrol()
    {
        agent.isStopped = false;
        forwardAxis = 0.4f;
        agent.speed = ac.walkSpeed * 0.4f;
        Vector3 destPos = partorlPoints[currentPratrolPoints].position;
        float distance = Vector3.Distance(destPos,transform.position);
        if(distance <= 1.5f)
        {
            currentPratrolPoints++;
            currentPratrolPoints = currentPratrolPoints % partorlPoints.Length;
        }
        agent.destination = destPos;

        if(disBetweenAI_Player < chaseDistance)
        {
            currentState = FSMState.chase;
            return ;
        }
        //进入攻击范围
        else if(disBetweenAI_Player <= attackDistance)
        {
            currentState = FSMState.attack;
            forwardAxis = 0.0f;
            agent.isStopped = true;
            ac.Attack();
            return;
        }

    }

    void Agianst()
    {        
        againstTime += Time.deltaTime;

        rightAxis = 0.5f;
        
        if(ac.cc.lockOnTarget == null)
        {
            ac.cc.LockOnToggle();
        }

        if(againstCD < 0.01)
        {
            againstCD = Random.Range(1.0f,againstMaxCD);
        }

        if(disBetweenAI_Player > attackDistance)
        {
            currentState = FSMState.chase;
            return;
        }
        //将决策时间随机化
        if(againstTime > againstCD)
        {
            againstTime = againstCD = rightAxis = 0.0f;
            pressOnLB = false;
            ac.DefenceOff();
            if(disBetweenAI_Player > attackDistance)
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
        else if(player_sm.isAttack)
        {
            Debug.Log("该防御了!");
            currentState = FSMState.defence;
            againstTime = againstCD = rightAxis = 0.0f;
            return;
        }
        else if(player_sm.isHeal)
        {
            Debug.Log("重拳出击!");
            againstTime = againstCD = rightAxis = 0.0f;
            pressOnLB = false;
            ac.DefenceOff();
            currentState = FSMState.attack;
            return;
        }
    }

    void Attack()
    {   
        attackTime += Time.deltaTime;
        //攻击时间片已分配完毕

        if(attackCD < 0.01f)
        {
            attackCD = Random.Range(0.5f,attackMaxCD);
        }
        if(disBetweenAI_Player > attackDistance && !am.sm.isAttack)
        {
            attackTime = attackCD = 0.001f;
            currentState = FSMState.chase;
            Debug.Log("attack -> chase");
        }
        else if(disBetweenAI_Player < attackDistance && attackTime < attackCD)
        {
            ac.turnDirectionSpeed = 0.3f;
            
            //ac.Attack();
        }
        else
        {
            //如果正在攻击，则等待动画播放完毕后再进行转换
            if(!am.sm.isAttack)
            {
                attackTime = attackCD = 0.001f;
                currentState = FSMState.agianst;
                return;
            }
        }
    }

    void Chase()
    {
        if(am.sm.isAttack) return;
        if(disBetweenAI_Player > chaseDistance)
        {
            currentState = FSMState.patrol;
            forwardAxis = 0.0f;
            return ;
        }

        else if(disBetweenAI_Player <= attackDistance)
        {
            currentState = FSMState.attack;
            agent.isStopped = true;
            forwardAxis = 0.0f;
            ac.Attack();
            return;
        }
        forwardAxis = 1.2f;
        agent.speed = ac.walkSpeed;
        agent.isStopped = false;
        agent.destination = player.position;
        //SetMoveDirectionAndInputMag(player.position,1.0f);
    }

    public void SetMoveDirectionAndInputMag(Vector3 pos,float mag)
    {
        Quaternion targetRotation = Quaternion.LookRotation(pos - transform.position);
        targetRotation.z= 0;
        //if(ac.enableTurnDirection)
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 8.0f);
        
        MovingVec = Vector3.forward;
        inputMag = mag;
    }
}
