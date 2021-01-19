using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    // Start is called before the first frame update

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
        if (!cc.isAI && playerInput.GetInputMag() > 0.1f)
        {
            if(!enableTurnDirection) return;
            Vector3 vec = cc.transform.forward;
            vec.y = 0;
            transform.forward = Vector3.Lerp(transform.forward, vec, turnDirectionSpeed);
            if(playerInput.GetMoveVec() != Vector3.zero)
                model.transform.forward = Vector3.Lerp(model.transform.forward, playerInput.GetMoveVec(), turnDirectionSpeed);
        }

        //运动向量 = 输入轴大小 * 模型方向 * 跑/走 给予的速度
        planarVec = dontMove ? Vector3.zero : playerInput.GetInputMag() * model.transform.forward * (playerInput.running ? runSpeed : walkSpeed);

        float newF = playerInput.GetInputMag() * (playerInput.running ? 2.0f : 1.0f);
        animator.SetFloat("forward", newF > 0.1f ? newF : 0.0f, 0.25f, Time.deltaTime);
        animator.SetFloat("right", 0);
    }

    protected void HandleMoveWhenLockOn()
    {
        //because the movingVec is worldSpace,and the forward to the target. so
        //when you just push forward without right,the movingVec.x would not be zero.
        //resulution1: 转换成局部坐标（这样局部forward永远是Vector3.forward(0,0,1),right同理）
        //resulution2: 用水平轴和竖直轴的输入构造向量.上面同理
        //TODO:BUGS:在root mothion下 仍然会圆周运动.
        
        Vector3 localMovingVec = transform.InverseTransformVector(playerInput.GetMoveVec());
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
        planarVec = dontMove ? Vector3.zero : playerInput.GetMoveVec();
        animator.SetFloat("forward", localMovingVec.z * (playerInput.running ? 2.0f : 1.0f), 0.35f, Time.deltaTime);
        animator.SetFloat("right", localMovingVec.x * (playerInput.running ? 2.0f : 1.0f), 0.35f, Time.deltaTime);
    }

    private void FixedUpdate()
    { 
        PhysicalMove();
    }

    private void LateUpdate() {

    }

    protected void PhysicalMove()
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

    public void Attack()
    {

        
        if (CheckState("Ground"))
        {
            EventCasterManager ecm = playerInput.am.im.GetFirstContactEventCaster();
            if (ecm && ecm.active && ecm.interractionEvent == InterractionEvent.FrontStab && planarVec.magnitude < 0.1f && cc.cameraStatus == CameraStatus.FOLLOW)
            {
                transform.position = ecm.am.transform.position +
                                     ecm.am.transform.TransformVector(ecm.offset);
                model.transform.LookAt(ecm.am.transform.position, Vector3.up);
                Stab();
                ecm.am.ac.Stabed();
            }
            else
                animator.SetTrigger("attack");
        }

        else if(CheckStateTag("attack") && combo)
        {
            animator.SetTrigger("attack");
        }
        
        else
            animator.SetTrigger("attack");
        
    }

    public void Attack(int atkMotionID)
    {
        animator.SetInteger("attackMotionType",atkMotionID);
        Attack();
    }

    public void Roll()
    {
        if(playerInput.GetMoveVec() != Vector3.zero)
            animator.SetTrigger("roll");
        else
            animator.SetTrigger("jab");
    }

    public void Defence()
    {
        animator.SetBool("defence", true);
    }

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

    //function:v 为 true时 允许进行连续攻击.
    public void EnableCombo(bool v)
    {
        combo = v;
    }

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
        UIManager.instance.ShowFirePanel();
    }
}
