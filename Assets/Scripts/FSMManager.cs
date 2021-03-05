using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class FSMManager : MonoBehaviour
{
    ActorController ac;
    StateManager sm;
    SoundManager soundManager;
    [SerializeField]
    public Transform shootPoint;
    //TODO:修改玩家引用
    private Transform target;
    private AINormal aI;
    private void Start() {
        ac = GetComponentInParent<ActorController>();
        sm = GetComponentInParent<StateManager>();
        soundManager = GetComponent<SoundManager>();
        target = GameObject.FindWithTag("Player").transform;
        aI = GetComponent<AINormal>();
    }

    public void EnableInput()
    {
        ac.EnableInput();
    }

    public void DisableInput()
    {
        ac.playerInput.InputToggle(false);
    }

    public void EnableTurnDirection()
    {
        ac.enableTurnDirection = true;
    }

    public void DisbleTurnDirection()
    {
        ac.enableTurnDirection = false;
    }

    public void FacePlayer()
    {
        aI.turnToTarget = true;
    }

    public void OnAttackEnter()
    {
        ac.enableTurnDirection = true;
        ac.playerInput.InputToggle(false);
        ac.enableTurnDirection = true;
        ac.EnableCombo(false);
        sm.AddEndurance(-10);
    }

    public void OnAttackExit()
    {
        ac.model.SendMessage("WeaponDisable");
        ac.enableTurnDirection = true;
        ac.playerInput.InputToggle(true);
        ac.EnableCombo(false);
    }

    public void OnAttackUpdate()
    {
        //animator.speed = animator.GetFloat("atkSpeed");
    }

    public void OnHitEnter()
    {
        //function : 关闭武器
        gameObject.SendMessage("WeaponDisable");
        ac.playerInput.InputToggle(false);
        ac.enableTurnDirection = false;
    }

    public void OnDieEnter()
    {
        sm.am.Die();
    }

    public void OnJumpEnter()
    {
        ac.playerInput.InputToggle(false);
        
        ac.StopMove(false);


    }

    public void OnFallUpdate()
    {

    }

    public void OnGroundEnter()
    {
        if(ac.animator.GetFloat("forward") > 0.3f)
            ac.animator.SetFloat("forward", 0.3f);

        ac.StopMove(false);

        ac.modelForwardTrackMovingVec = false;

        ac.playerInput.InputToggle(true);
        ac.enableTurnDirection = true;

        ac.model.SendMessage("WeaponDisable");

        ac.EnableCombo(false);

        sm.isImmortal = false;
    }

    public void OnGroundUpdate()
    {
        sm.AddEndurance(ac.playerInput.running ? -5.0f * Time.deltaTime : 12.0f * Time.deltaTime);
    }

    public void OnGroundExit()
    {
        ac.playerInput.InputToggle(false);
        ac.StopMove(true);
        //enableTurnDirection = false;
    }


    //以下可以更改

    public void OnRollEnter()
    {
        ac.playerInput.InputToggle(false);    //roll->roll必备
        sm.isImmortal = true;

        //修正模型方向，防止连滚时方位错乱
        Vector3 vec = ac.cc.transform.forward;
        vec.y = 0;
        transform.forward =  vec;
        ac.model.transform.forward = ac.playerInput.MovingVec;
        
        Vector3 dir = ac.playerInput.MovingVec.normalized;
        dir.y = 0;
        if (dir == Vector3.zero) dir = Vector3.forward;
        ac.model.transform.forward = dir;

        ac.planarVec = Vector3.zero;

        //thusVec = model.transform.forward * rollSpeed;

        ac.modelForwardTrackMovingVec = true;
        
        sm.AddEndurance(-15f);

    }
    
    public void OnRollExit()
    {
        sm.isImmortal = false;
        ac.modelForwardTrackMovingVec = false;
    }
    public void OnRollUpdate()
    {

    }

    public void EnterDrawBow()
    {
        ac.playerInput.InputToggle(true);
        ac.enableTurnDirection = true;
        ac.planarVec = Vector3.zero;

        ac.animator.ResetTrigger("attack");
    }

    public void ExitDrawBow()
    {
        ac.cc.cameraStatus = CameraStatus.FREE;
    }

    public void OnShootEnter()
    {
        ac.playerInput.InputToggle(true);
        ac.enableTurnDirection = true;
    }

    public void OnDefenceUpdate()
    {
        ac.playerInput.InputToggle(true);
    }

    public void OnReloadUpdate()
    {
        
    }

    public void OnDrawUpdate()
    {
        
    }

    public void Shoot()
    {
        GameObject arrow = ObjectPool.instance.GetObject("Arrow_stick");
        arrow.transform.position = shootPoint.position + shootPoint.forward;
        arrow.transform.forward = shootPoint.forward;
        AINormal ai = ac.playerInput as AINormal;
        float r = UnityEngine.Random.Range(0, 0.2f);
        Vector3 randVec = new Vector3(r, 1.0f + r, r);
        if(ai)
            arrow.transform.LookAt(ai.player.position + randVec);
        WeaponData wd = arrow.GetComponent<WeaponData>();
        //TODO：WD的攻击力是发射的瞬间决定的.所以此处应该根据角色当前攻击力去设置.
        wd.battleManager = sm.am.bm;
        arrow.SetActive(true);
        soundManager.PlayGunShoot();
        ac.enableTurnDirection = false;
    }

    public void JustTestEnvent()
    {
        print("just for test.");
    }

    public void OnDefenceEnter()
    {
        ac.playerInput.InputToggle(false);
    }

    public void OnBlockedEnter()
    {
        sm.AddEndurance(-10.0f);
    }

    public void OnDrinkEnter()
    {
        sm.AddHp(200.0f);
    }

    public void SetTurnAble(bool val)
    {
        ac.enableTurnDirection = val;
    }

    

    



}
