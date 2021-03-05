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
        }
        catch (System.Exception)
        {
            
        }

        try
        {
            weaponHandleR = transform.DeepFind("weaponHandleR").gameObject;
            wcR = BindWeaponController(weaponHandleR);
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
            animator.SetInteger("attackMotionType",(int)wd.weaponItem.wpAtkMotionID);
        }
        catch (System.Exception)
        {

        }

        if (am.gameObject.CompareTag("Player"))
        {
            ChangeWeaponEvent.Invoke((int)wcR.weaponDataOnUse.weaponItem.GetID(),true);
            ChangeWeaponEvent.Invoke((int)wcL.weaponDataOnUse.weaponItem.GetID(),false);
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
    
    
    /// <summary>
    /// 打开当前武器的碰撞器
    /// </summary>
    public void WeaponEnable()
    {
        r0l1 = animator.GetBool("R0L1");
        GetWeaponDataOnUse(!r0l1).col.enabled = true;
        SetWeaponVFX(true);
    }

    /// <summary>
    /// 关闭武器触发器
    /// </summary>
    public void WeaponDisable()
    {
        r0l1 = animator.GetBool("R0L1");
        GetWeaponDataOnUse(!r0l1).col.enabled = false;
        SetWeaponVFX(false);
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

    /// <summary>
    /// 切换到下一把武器
    /// </summary>
    /// <param name="rh"></param>
    public void Switch2NextWeapon(bool rh)
    {
        WeaponController wc = rh ? wcR : wcL;
        WeaponData current = wc.weaponDataOnUse;
        WeaponData next = wc.GetNextWeapon();

        wc.SetWeaponVisiable(current.gameObject,false);
        wc.SetWeaponVisiable(next.gameObject,true);

        wc.weaponDataOnUse = next;

        //如果是玩家，则进行UI更新.
        // if(am.gameObject.CompareTag("Player"))
        // ChangeWeaponEvent.Invoke((int)wc.weaponDataOnUse.weapon.GetID(),rh);
        if (wc.weaponDataOnUse.weaponItem == null)
        {
            wc.EquipWeapon((WeaponItem)Item.GetItem(ItemEnum.Fist),next.transform.GetSiblingIndex());
        }
        am.SetAtkAnimationInt(wc.weaponDataOnUse.weaponItem.wpAtkMotionID);
        AnimatorFactory.SetLocalMotion(animator,wc.weaponDataOnUse.weaponItem.localMotionID1H);

        SetAllWeaponOnUseVisiable(true);
    }

    /// <summary>
    /// 装备武器
    /// </summary>
    /// <param name="weaponItem"></param>
    /// <param name="pos"></param>
    /// <param name="rh"></param>
    public void EquipWeapon(WeaponItem weaponItem, int pos, bool rh)
    {
        WeaponController wc = rh ? wcR : wcL;
        wc.EquipWeapon(weaponItem,pos); //更改模型、wd引用
        
        //tips:黑魂以右手作为localMothion动画的判断
        if (wc.weaponDataOnUse == GetWeaponDataOnUse(true))
        {
            AnimatorFactory.SetLocalMotion(animator,wc.weaponDataOnUse.weaponItem.localMotionID1H);
        }
        
        //更新UI
        if(rh)
            am.inventory.InventoryUI.rhwpISM.UpdateItem(pos,weaponItem);
        else
            am.inventory.InventoryUI.lhwpISM.UpdateItem(pos,weaponItem);
    }

    public void SetWeaponVFX(bool v)
    {
        if (weaponVFX)
        {
            weaponVFX.SetActive(v);
        }
    }
    
    /// <summary>
    /// 获取正在使用的武器
    /// </summary>
    /// <param name="right"></param>
    /// <returns></returns>
    public WeaponData GetWeaponDataOnUse(bool right)
    {
        if (right && wcR) 
            return wcR.weaponDataOnUse;
        else if (!right && wcL) 
            return wcL.weaponDataOnUse;
        else 
            return null;
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

    public Item[] GetWeapons(bool rh)
    {
        if (rh) return wcR.GetWeapons();
        return wcL.GetWeapons();
    }

}
