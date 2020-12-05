using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("==== Speed Settings")]
    public float jumpForce = 2.5f;
    public float walkSpeed = 1.0f;
    public float runSpeed = 2.0f;

    [Range(0,100)]
    public float turnDirectionSpeed = 0.75f;
    public GameObject model;
    public PlayerInput playerInput;
    public CameraController cc;

    public Animator animator;
    private StateManager sm;
    private CharacterController characterController;

    public Vector3 planarVec;
    public Vector3 thusVec;
    public Vector3 deltaPos;

    [SerializeField]
    private bool dontMove = false;
    public bool modelForwardTrackMovingVec = false;

    public int wineCount = 10;
    public bool enableTurnDirection = true;

    private bool combo = false;

    public Transform neckPos;

    public bool isOnScreen;

    private void OnBecameInvisible() {
        isOnScreen = false;
    }
    private void OnBecameVisible() {
        isOnScreen = true;
    }

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
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
        if (cc.cameraStatus == CameraStatus.FOLLOW || cc.cameraStatus == CameraStatus.AIM)
        {
            HandleMoveOnNormal();
        }

        else if (cc.cameraStatus == CameraStatus.LOCKON)
        {
            HandleMoveWhenLockOn();
        }


        ParseInput();
    }



    private void ParseInput()
    {
        if(!playerInput.enableInput) return;
        WeaponData leftHand = sm.am.wm.GetWeaponDataOnUse(false);
        WeaponData rightHand = sm.am.wm.GetWeaponDataOnUse(true);
        if (sm.weaponHold == WeaponHold._1h)
        {
            //如果左手持盾
            if(leftHand)
            {
                if (leftHand.wpAtkMotionID == WpAtkMotionID.Shield)
                {
                    // float oldWeight = animator.GetLayerWeight(1);
                    // int index = animator.GetLayerIndex("defence");
                    //只能在Localmothion状态持盾，否则设置盾权重为0.
                    if (playerInput.pressOnLB)
                    {
                        //animator.SetLayerWeight(index, Mathf.Lerp(oldWeight,1.0f,0.15f));
                        Defence();
                    }
                    else
                    {
                        //animator.SetLayerWeight(index, Mathf.Lerp(oldWeight,0.0f,0.15f));
                        DefenceOff();
                    }
                }

                //如果持有常规武器，并且按下LB
                else if (playerInput.pressLB)
                {
                    int index = animator.GetLayerIndex("defence");
                    animator.SetLayerWeight(index, 0.0f);
                    animator.SetBool("R0L1", true);
                    animator.SetInteger("attackMotionType", (int)leftHand.wpAtkMotionID);
                    Attack();
                }
            }
        }

        if(playerInput.pressR)
        {
            cc.LockOnToggle();
        }

        if (playerInput.pressRB)
        {
            animator.SetBool("R0L1", false);
            animator.SetInteger("attackMotionType", (int)rightHand.wpAtkMotionID);
            Attack();
        }

        animator.SetBool("holdOnRB",playerInput.pressOnRB);


        //响应弓箭的长按.
        if(playerInput.pressOnRB && animator.GetInteger("attackMotionType") == 44)
        {
            if(Input.GetMouseButtonUp(0))
            {
                animator.SetTrigger("attack");
            }
        }

        if (playerInput.pressRT)
        {
            animator.SetTrigger("Hattack");
        }

        if (playerInput.pressLT)
        {
            if (CheckStateTag("attack") || CheckState("Ground"))
            {
                //如果左手为盾牌
                if (WeaponData.IsShield(sm.am.wm.GetWeaponDataOnUse(false)))
                {
                    animator.SetTrigger("countBack");
                }
            }
        }

        if (playerInput.pressB)
        {
            Roll();
        }

    }

    protected void HandleMoveOnNormal()
    {
        if (!cc.isAI && playerInput.inputMag > 0.1f)
        {
            if(!enableTurnDirection) return;
            Vector3 vec = cc.transform.forward;
            vec.y = 0;
            transform.forward = Vector3.Lerp(transform.forward, vec, turnDirectionSpeed);




            if(playerInput.MovingVec != Vector3.zero)
                model.transform.forward = Vector3.Lerp(model.transform.forward, playerInput.MovingVec, turnDirectionSpeed);
        }

        //运动向量 = 输入轴大小 * 模型方向 * 跑/走 给予的速度
        planarVec = dontMove ? Vector3.zero : playerInput.inputMag * model.transform.forward * (playerInput.running ? runSpeed : walkSpeed);

        float oldF = animator.GetFloat("forward");
        float newF = playerInput.inputMag * (playerInput.running ? 2.0f : 1.0f);
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
        
        Vector3 localMovingVec = transform.InverseTransformVector(playerInput.MovingVec);
        if (modelForwardTrackMovingVec)
        {
            // model.transform.forward = thusVec.normalized;
            if (planarVec != Vector3.zero)
                model.transform.forward = planarVec.normalized;
        }
        else if(enableTurnDirection)
        {
            model.transform.forward = transform.forward;
        }
        planarVec = dontMove ? Vector3.zero : playerInput.inputMag * playerInput.MovingVec * (playerInput.running ? runSpeed : walkSpeed);

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
            Vector3 movement = planarVec + Physics.gravity;
            characterController.Move(movement * Time.deltaTime + deltaPos );
            deltaPos = Vector3.zero;
        }
    }

    public void Attack()
    {
        if (CheckState("Ground") && !animator.GetNextAnimatorStateInfo(0).IsTag("attack"))
        {
            animator.SetTrigger("attack");
        }

        else if(CheckStateTag("attack") && combo)
        {
            animator.SetTrigger("attack");
        }
        
    }

    private void Roll()
    {
        if(playerInput.MovingVec != Vector3.zero)
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
}
