using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public class PlayerInput : ActorInput
{
    [Header("==== Key Settings ====")]

    public KeyCode keyRun;

    public KeyCode keyAction = KeyCode.E;

    public KeyCode keyUseItem = KeyCode.R;

    public KeyCode keyDualWeapon = KeyCode.F;

    public KeyCode keyNextRHWeapon = KeyCode.L;

    public KeyCode keyAttack = KeyCode.Mouse0;
    public KeyCode keyNextItem = KeyCode.O;

    public KeyCode keyLockOn = KeyCode.Q;    
    [Header("==== Output Signals ====")]
    protected float forwardAxis;
    protected float rightAxis;

    public float inputMag; //运动幅度 [0,1]
    
    Vector2 playerInput;

    public bool pressOnLB = false;
    public bool pressLB = false;
    public bool pressLT = false;
    public bool pressRB = false;
    public bool pressOnRB = false;
    public bool pressRT = false;
    public bool pressB = false;

    public bool pressR = false;
    public KeyCode switchWeapon = KeyCode.Y;

    private void Awake()
    {
        
    }

    void Start()
    {
        am = GetComponent<ActorManager>();
        ac = GetComponent<ActorController>();
    }

    void Update()
    {
        rightAxis = Input.GetAxis("Horizontal");
        forwardAxis = Input.GetAxis("Vertical");
        MoveInput();
        InputDetecte();
        ParseInput();
    }

    private void InputDetecte()
    {
        running = Input.GetKey(keyRun);
        pressRB = Input.GetMouseButtonDown(0);
        pressOnLB = Input.GetMouseButton(1);
        pressLB = Input.GetMouseButtonDown(1);
        pressLT = Input.GetKeyDown(KeyCode.LeftControl);
        pressRT = Input.GetKey(KeyCode.CapsLock);
        pressB = Input.GetKeyDown(KeyCode.Space);
        pressOnRB = Input.GetKey(KeyCode.CapsLock);
        pressR = Input.GetKeyDown(keyLockOn);
    }

    /// <summary>
    /// 通过球体映射，平滑化纵轴和横轴的输入
    /// </summary>
    /// <param name="input">x为横轴，z为纵轴</param>
    /// <returns>球体映射后输入向量</returns>
    private Vector3 SquareToCircle(Vector3 input)
    {
        input.x = input.x * Mathf.Sqrt(1-(input.z * input.z) / 2.0f);
        input.z = input.z * Mathf.Sqrt(1-(input.z * input.x) / 2.0f);
        return input;
    }


    //根据输入构造移动向量
    protected void MoveInput()
    {
        if(!EnableInput)
        {
            inputMag = 0.0f;
            MovingVec = Vector3.zero;
            return;
        }
        MovingVec.x = rightAxis;
        MovingVec.z = forwardAxis;
        MovingVec = SquareToCircle(playerInput);
        
        //输入的运动幅度
        inputMag = Mathf.Sqrt(forwardAxis * forwardAxis + rightAxis * rightAxis);
        inputMag = Mathf.Clamp(inputMag,0.0f,1.0f);

        if(inputMag > 0.1f)
        {
            //移动
            MovingVec = forwardAxis * transform.forward + rightAxis * transform.right;
        }

        else
        {
            MovingVec = Vector3.zero;
        }
    }
    
    /// <summary>
    /// 解析输入，构造命令
    /// </summary>
    void ParseInput()
    {
        if(!EnableInput) return;
        if(am.sm.Naili < 2.5f) return;    //至少Ground 0.5s才能继续行动
        WeaponData leftHand = am.wm.GetWeaponDataOnUse(false);
        WeaponData rightHand = am.wm.GetWeaponDataOnUse(true);
        if (am.sm.weaponHold == WeaponHold._1h)
        {
            //如果左手持盾
            if(leftHand)
            {
                if (leftHand.weapon.wpAtkMotionID == WpAtkMotionID.Shield)
                {
                    // float oldWeight = animator.GetLayerWeight(1);
                    // int index = animator.GetLayerIndex("defence");
                    //只能在Localmothion状态持盾，否则设置盾权重为0.
                    if (pressOnLB)
                    {
                        //animator.SetLayerWeight(index, Mathf.Lerp(oldWeight,1.0f,0.15f));
                        ac.Defence();
                    }
                    else
                    {
                        //animator.SetLayerWeight(index, Mathf.Lerp(oldWeight,0.0f,0.15f));
                        ac.DefenceOff();
                    }
                }

                //如果持有常规武器，并且按下LB
                else if (pressLB)
                {
                    int index = ac.animator.GetLayerIndex("defence");
                    ac.animator.SetLayerWeight(index, 0.0f);
                    ac.animator.SetBool("R0L1", true);
                    ac.animator.SetInteger("attackMotionType", (int)leftHand.weapon.wpAtkMotionID);
                    ac.Attack();
                }
            }
        }

        if(pressR)
        {
            ac.cc.LockOnToggle();
        }

        if (pressRB)
        {
            ac.animator.SetBool("R0L1", false);
            ac.animator.SetInteger("attackMotionType", (int)rightHand.weapon.wpAtkMotionID);
            ac.Attack();
        }

        ac.animator.SetBool("holdOnRB",pressOnRB);


        //响应弓箭的长按.
        if(pressOnRB && ac.animator.GetInteger("attackMotionType") == 44)
        {
            if(Input.GetMouseButtonUp(0))
            {
                ac.animator.SetTrigger("attack");
            }
        }

        if (pressRT)
        {
            ac.animator.SetTrigger("Hattack");
        }

        if (pressLT)
        {
            if (ac.CheckStateTag("attack") || ac.CheckState("Ground"))
            {
                //如果左手为盾牌
                if (WeaponData.IsShield(am.wm.GetWeaponDataOnUse(false)))
                {
                    ac.animator.SetTrigger("countBack");
                }
            }
        }

        if (pressB)
        {
            ac.Roll();
        }
                
        if(Input.GetKeyDown(keyAction))
            am.DoAction();

        if (Input.GetKeyDown(keyDualWeapon))
            am.ChangeDualHand(true);
        
        if(Input.GetKeyDown(keyNextRHWeapon))
            am.ChangeWeapon(false);

        if(Input.GetKeyDown(keyNextItem))
            am.NextItem();

        if(Input.GetKeyDown(keyUseItem))
            am.UseItem();
        
        if(Input.GetKeyDown(KeyCode.LeftAlt))
            am.Die();
        
        if(Input.GetKeyDown(KeyCode.I))
            UIManager.instance.ShowOptionPanel();

    }
}
