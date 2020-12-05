using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
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

    
    public Vector3  MovingVec; //运动方向 [-1.1]^3
    Vector2 playerInput;

    public bool running;
    public bool enableInput = true;

    public bool pressOnLB = false;
    public bool pressLB = false;
    public bool pressLT = false;
    public bool pressRB = false;
    public bool pressOnRB = false;
    public bool pressRT = false;
    public bool pressB = false;

    public bool pressR = false;
    public KeyCode switchWeapon = KeyCode.Y;

    public ActorController ac;
    public ActorManager am;

    void Start()
    {
        am = GetComponent<ActorManager>();
        ac = GetComponent<ActorController>();
    }

    void Update()
    {   rightAxis = Input.GetAxis("Horizontal");
        forwardAxis = Input.GetAxis("Vertical");
        MoveInput();

        running = Input.GetKey(keyRun);
        pressRB = Input.GetMouseButtonDown(0);
        pressOnLB = Input.GetMouseButton(1);
        pressLB = Input.GetMouseButtonDown(1);
        pressLT = Input.GetKeyDown(KeyCode.LeftControl);
        pressRT = Input.GetKey(KeyCode.CapsLock);
        pressB = Input.GetKeyDown(KeyCode.Space);
        pressOnRB = Input.GetKey(KeyCode.CapsLock);
        pressR = Input.GetKeyDown(keyLockOn);
        
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

        
    }

    //将输入映射成球体
    private Vector3 SquareToCircle(Vector3 input)
    {
        input.x = input.x * Mathf.Sqrt(1-(input.z * input.z) / 2.0f);
        input.z = input.z * Mathf.Sqrt(1-(input.z * input.x) / 2.0f);
        return input;
    }


    //根据输入构造移动向量
    protected void MoveInput()
    {
        if(!enableInput)
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

    public void InputToggle(bool value)
    {
        enableInput = value;
    }

    private void OnGUI() {
        // if(GUI.Button(new Rect(100,100,200,200),"PickUp"))
        // {

        // }
    }    

}
