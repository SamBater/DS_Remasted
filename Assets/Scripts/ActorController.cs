using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    [Header("==== Speed Settings")]
    public float walkSpeed = 1.0f;
    public float runSpeed = 2.0f;

    [Range(0,100)]
    public float turnDirectionSpeed = 0.75f;
    public GameObject model;
    public ActorInput playerInput;
    public CameraController cc;

    public Animator animator;
    private StateManager sm;
    private CharacterController characterController;

    public Vector3 planarVec;
    public Vector3 deltaPos;

    [SerializeField]
    private bool dontMove = false;
    public bool modelForwardTrackMovingVec = false;

    public int wineCount = 10;
    public bool enableTurnDirection = true;

    private bool combo = false;

    public Transform neckPos;
    public Transform lockOnTarget;
    
    void Awake()
    {
        playerInput = GetComponent<ActorInput>();
        animator = model.GetComponent<Animator>();
        sm = GetComponent<StateManager>();
        characterController = GetComponent<CharacterController>();
        if(neckPos == null) neckPos = animator.GetBoneTransform(HumanBodyBones.Neck);
        if (!cc) cc = new CameraController();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.layer != LayerMask.NameToLayer("Enemy") && gameObject.layer != LayerMask.NameToLayer("Player"))
            return;

        if(playerInput.running) modelForwardTrackMovingVec = true;
        
        //更改整个朝向,模型transform始终朝向MovingVec，MovingVec取决于transform，所以的话
        if (cc.cameraStatus == CameraStatus.FOLLOW && !sm.isLock)
        {
            HandleMoveOnNormal();
        }

        else if (cc.cameraStatus == CameraStatus.LOCKON)
        {
            HandleMoveWhenLockOn();
        }
    }

    protected void HandleMoveOnNormal()
    {
        if(dontMove) return;
        if (cc!=null  && playerInput.GetInputMag() > 0.1f)
        {
            if(!enableTurnDirection) return;
            Vector3 vec = cc.transform.forward;
            vec.y = 0;
            transform.forward = Vector3.Lerp(transform.forward, vec, turnDirectionSpeed);
            if(playerInput.MovingVec != Vector3.zero)
                model.transform.forward = Vector3.Lerp(model.transform.forward, playerInput.MovingVec, turnDirectionSpeed);
        }

        //运动向量 = 输入轴大小 * 模型方向 * 跑/走 给予的速度
        planarVec = dontMove ? Vector3.zero : playerInput.GetInputMag() * model.transform.forward * (playerInput.running ? runSpeed : walkSpeed);

        float newF = playerInput.GetInputMag() * (playerInput.running ? 2.0f : 1.0f);
        animator.SetFloat("forward", newF > 0.1f ? newF : 0.0f, 0.25f, Time.deltaTime);
        animator.SetFloat("right", 0);
    }

    protected void HandleMoveWhenLockOn()
    {
        Vector3 localMovingVec = transform.InverseTransformVector(playerInput.MovingVec);
        if (modelForwardTrackMovingVec)
        {
            if (planarVec != Vector3.zero)
                model.transform.forward = planarVec.normalized;
        }
        else if(enableTurnDirection)
        {
            model.transform.forward = transform.forward;
        }
        //planarVec = dontMove ? Vector3.zero : playerInput.inputMag * playerInput.GetMoveVec() * (playerInput.running ? runSpeed : walkSpeed);
        planarVec = dontMove ? Vector3.zero : playerInput.MovingVec;
        animator.SetFloat("forward", localMovingVec.z * (playerInput.running ? 2.0f : 1.0f), 0.35f, Time.deltaTime);
        animator.SetFloat("right", localMovingVec.x * (playerInput.running ? 2.0f : 1.0f), 0.35f, Time.deltaTime);
    }

    private void FixedUpdate()
    { 
        PerformMove();
    }
    
    /// <summary>
    /// 根据向量移动角色
    /// </summary>
    protected void PerformMove()
    {
        if(characterController)
        {
            Physics.SyncTransforms();
            characterController.Move(Physics.gravity * Time.deltaTime + deltaPos);
        }
        
        deltaPos = Vector3.zero;
    }

    public void Stab()
    {
        animator.SetInteger("attackMotionType",1);
        animator.SetTrigger("attack");
    }

    public void Stabed()
    {
        animator.SetInteger("hitID",1);
        animator.SetTrigger("hit");
    }
    
    /// <summary>
    /// 播放攻击动画
    /// </summary>
    public void Attack()
    {
        if(sm.Naili < 1.0f) return;
        //处理特殊事件"弹反"攻击
        if (CheckState("Ground"))
        {
            // EventCasterManager ecm = playerInput.am.im.GetFirstContactEventCaster();
            // if (ecm && ecm.active && ecm.interractionEvent == InterractionEvent.FrontStab && planarVec.magnitude < 0.1f && cc.cameraStatus == CameraStatus.FOLLOW)
            // {
            //     transform.position = ecm.am.transform.position +
            //                          ecm.am.transform.TransformVector(ecm.offset);
            //     model.transform.LookAt(ecm.am.transform.position, Vector3.up);
            //     Stab();
            //     ecm.am.ac.Stabed();
            //     return;
            // }
        }
        
        //普通攻击
        if(!sm.isAttack)
            animator.SetTrigger("attack");
            
        //连击
        else if(combo)
        {
            animator.SetTrigger("attack");
        }

    }

    public void Attack(int atkMotionID)
    {
        animator.SetInteger("attackMotionType",atkMotionID);
        Attack();
    }
    
    /// <summary>
    /// 播放翻滚动画
    /// </summary>
    public void Roll()
    {
        if(playerInput.MovingVec != Vector3.zero)
            animator.SetTrigger("roll");
        else
            animator.SetTrigger("jab");
    }
    
    /// <summary>
    /// 播放防御动画
    /// </summary>
    public void Defence()
    {
        animator.SetBool("defence", true);
    }
    
    /// <summary>
    /// 取消防御动画
    /// </summary>
    public void DefenceOff()
    {
        animator.SetBool("defence", false);
    }

    public void OnGround()
    {
        animator.SetBool("onGround", true);
    }

    public void NotOnGround()
    {
        animator.SetBool("onGround", false);
    }

    public bool CheckState(string stateName, string layerName = "Base Layer")
    {
        int layerIndex = animator.GetLayerIndex(layerName);
        bool result = animator.GetCurrentAnimatorStateInfo(layerIndex).IsName(stateName);
        return result;
    }

    public bool CheckStateTag(string tagName, string layerName = "Base Layer")
    {
        int layerIndex = animator.GetLayerIndex(layerName);
        bool result = animator.GetCurrentAnimatorStateInfo(layerIndex).IsTag(tagName);
        return result;
    }

    public void EnableInput()
    {
        playerInput.InputToggle(true);
    }
    
    public void PlayHeal()
    {
        if (wineCount > 0)
            animator.SetInteger("wineCount", wineCount -= 1);
    }

    public void StopMove(bool value)
    {
        dontMove = value;
    }
    
    /// <summary>
    /// 开启combo trigger 允许角色进行连击
    /// </summary>
    /// <param name="v"></param>
    public void EnableCombo(bool v)
    {
        combo = v;
    }
    
    /// <summary>
    /// 点火
    /// </summary>
    public void LitFire()
    {
        animator.Play("bonefire_lit");
    }
    
    public void SitFire()
    {
        animator.Play("bonefire_rest");
    }

    public void StandUpFire()
    {
        animator.SetTrigger("Leave");
        playerInput.am.wm.SetAllWeaponOnUseVisiable(true);
        playerInput.am.ActorUIManager.ShowFirePanel();
    }
}
