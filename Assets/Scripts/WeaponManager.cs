using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
            animator.SetInteger("attackMotionType",(int)wd.wpAtkMotionID);
        }
        catch (System.Exception)
        {

        }
    }

    private void OnTriggerEnter(Collider other) {
        //print(other.name);
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
        if(weaponColL == null) {
            Debug.LogError(am.gameObject + "weaponCol doesn't exists.");
            return;
        }
        if(weaponColR == null)
        {
            Debug.LogError(am.gameObject + "weaponColR doesn't exists");
            return;
        }
        r0l1 = animator.GetBool("R0L1");
        weaponColR.enabled = !r0l1;
        weaponColL.enabled = r0l1;
        SetWeaponVFX(true);
    }

    //FUNCTION:在攻击后摇阶段调用：关闭武器的触发器、
    public void WeaponDisable()
    {
        if(weaponColL && weaponColR)
        weaponColL.enabled = weaponColR.enabled = false;
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
        wcL.wdOnUse.gameObject.SetActive(value);
        wcR.wdOnUse.gameObject.SetActive(value);
    }

    public void SetWeaponOnUseVisiable(bool value,bool rh)
    {
        if(rh) wcR.wdOnUse.gameObject.SetActive(true);
        else wcL.wdOnUse.gameObject.SetActive(false);
    }

    public void UpdateWeaponCollider(Collider col,bool rightCol=true)
    {
        if(rightCol) weaponColR = col;
        else weaponColL = col;
    }

    public void ChangeWeapon(bool rh)
    {
        WeaponController wc = rh ? wcR : wcL;
        WeaponData wd_cur = wc.wdOnUse;
        WeaponData wd_next = wc.GetNextWeapon();

        wc.SetWeaponVisiable(wd_cur.gameObject,false);
        wc.SetWeaponVisiable(wd_next.gameObject,true);

        UpdateWeaponCollider(wd_next.gameObject.GetComponent<Collider>(),rh);

        wc.wdOnUse = wd_next;

        //如果是玩家，则进行UI更新.
        if(am.gameObject.tag == "Player")
            UIManager.instance.UpdateWeaponIcon(wc.wdOnUse.icon,rh);

        am.SetAtkAnimationInt(wc.wdOnUse.wpAtkMotionID);
        AnimatorFactory.SetLocalMotion(animator,wc.wdOnUse.localMotionID1H);

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
        if (right) return wcR.wdOnUse;
        else return wcL.wdOnUse;
    }

    public void AttackStart()
    {
        WeaponEnable();
        am.ac.playerInput.InputToggle(false);
        am.ac.enableTurnDirection = false;
        SetWeaponVFX(true);
    }

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
