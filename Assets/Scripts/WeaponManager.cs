using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class UpdateEvent : UnityEvent<int,bool>{}

public class WeaponManager : IActorManagerInterface
{
    public Collider weaponColL;
    public Collider weaponColR;
    GameObject weaponHandleL;
    GameObject weaponHandleR;
    public WeaponController wcL;
    public WeaponController wcR;
    private Animator animator;
    bool r0l1;
    public GameObject weaponVFX;
    public UpdateEvent ChangeWeaponEvent = new UpdateEvent();
    void Awake()
    {
        animator = GetComponent<Animator>();
        try
        {
            weaponHandleL = transform.DeepFind("weaponHandleL").gameObject;
            wcL = BindWeaponController(weaponHandleL);
            weaponColL = weaponHandleL.GetComponentInChildren<Collider>();  
        }
        catch (System.Exception)
        {
            
        }

        try
        {
            weaponHandleR = transform.DeepFind("weaponHandleR").gameObject;
            wcR = BindWeaponController(weaponHandleR);
            weaponColR = weaponHandleR.GetComponentInChildren<Collider>();
        }
        catch (System.Exception e)
        {

        }

    }

    private void Start() {
        //更新左手、右手的Icon图标
        try
        {
            WeaponData wd = GetWeaponDataOnUse(true);
            animator.SetInteger("attackMotionType",(int)wd.weapon.wpAtkMotionID);
        }
        catch (System.Exception)
        {

        }

        if (am.gameObject.CompareTag("Player"))
        {
            ChangeWeaponEvent.Invoke((int)wcR.weaponDataOnUse.weapon.GetID(),true);
            ChangeWeaponEvent.Invoke((int)wcL.weaponDataOnUse.weapon.GetID(),false);
        }
    }

    public WeaponController BindWeaponController(GameObject go)
    {
        WeaponController tempWc;
        tempWc = go.GetComponent<WeaponController>();
        if(tempWc == null)
        {
            tempWc = go.AddComponent<WeaponController>();
        }
        tempWc.wm = this;
        return tempWc;
    }
    
    //FUNCTION:在攻击判定阶段调用：打开武器的碰撞器、
    public void WeaponEnable()
    {
        r0l1 = animator.GetBool("R0L1");
        if(weaponColL != null) {
            weaponColL.enabled = r0l1;
        }
        if(weaponColR != null)
        {
            weaponColR.enabled = !r0l1;
        }
        SetWeaponVFX(true);
    }

    /// <summary>
    /// 关闭武器触发器
    /// </summary>
    public void WeaponDisable()
    {
        if(weaponColL ) 
            weaponColL.enabled = false;     

        if (weaponColR)
            weaponColR.enabled = false;
    }

    public void CountBackEnable()
    {
        am.SetCountBackState(true);
    }

    public void CountBackDisable()
    {
        am.SetCountBackState(false);
    }

    public void SetAllWeaponOnUseVisiable(bool value)
    {
        wcL.weaponDataOnUse.gameObject.SetActive(value);
        wcR.weaponDataOnUse.gameObject.SetActive(value);
    }

    public void SetWeaponOnUseVisiable(bool value,bool rh)
    {
        if(rh) wcR.weaponDataOnUse.gameObject.SetActive(true);
        else wcL.weaponDataOnUse.gameObject.SetActive(false);
    }

    public void UpdateWeaponCollider(Collider col,bool rightCol=true)
    {
        if(rightCol) weaponColR = col;
        else weaponColL = col;
    }

    public void ChangeWeapon(bool rh)
    {
        WeaponController wc = rh ? wcR : wcL;
        WeaponData current = wc.weaponDataOnUse;
        WeaponData next = wc.GetNextWeapon();

        wc.SetWeaponVisiable(current.gameObject,false);
        wc.SetWeaponVisiable(next.gameObject,true);

        UpdateWeaponCollider(next.gameObject.GetComponent<Collider>(),rh);

        wc.weaponDataOnUse = next;

        //如果是玩家，则进行UI更新.
        // if(am.gameObject.CompareTag("Player"))
        // ChangeWeaponEvent.Invoke((int)wc.weaponDataOnUse.weapon.GetID(),rh);

        am.SetAtkAnimationInt(wc.weaponDataOnUse.weapon.wpAtkMotionID);
        AnimatorFactory.SetLocalMotion(animator,wc.weaponDataOnUse.weapon.localMotionID1H);

        SetAllWeaponOnUseVisiable(true);
    }

    public void SetWeaponVFX(bool v)
    {
        if (weaponVFX)
        {
            weaponVFX.SetActive(v);
        }
    }

    public WeaponData GetWeaponDataOnUse(bool right)
    {
        if (!wcR && !wcL) return null;
        if (right) return wcR.weaponDataOnUse;
        else return wcL.weaponDataOnUse;
    }
    
    /// <summary>
    /// 动画事件，启动攻击判定，同时关闭玩家输入
    /// </summary>
    public void AttackStart()
    {
        WeaponEnable();
        am.ac.playerInput.InputToggle(false);
        am.ac.enableTurnDirection = false;
        SetWeaponVFX(true);
    }
    
    /// <summary>
    /// 动画事件，攻击结束，同时打开玩家输入，允许连击
    /// </summary>
    public void AttackEnd()
    {
        WeaponDisable();
        am.ac.playerInput.InputToggle(true);
        am.ac.EnableCombo(true);
        SetWeaponVFX(false);
    } 

    public void TurnAbleAttackStart()
    {
        WeaponEnable();
        am.ac.enableTurnDirection = true;
        SetWeaponVFX(true);
    }   

    public void TurnAbleAttackEnd()
    {
        WeaponDisable();
        am.ac.playerInput.InputToggle(true);
        am.ac.EnableCombo(true);
        am.ac.enableTurnDirection = false;
        SetWeaponVFX(false);
    }

    IEnumerator ExtendAttackBoxForSeconds(float sec)
    {
        weaponColR.bounds.Expand(2.0f);
        yield return new WaitForSecondsRealtime(sec);
        weaponColR.bounds.Expand(-2.0f);
    }

}
