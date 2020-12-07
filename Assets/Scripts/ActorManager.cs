using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ActorType
{
    Player = 1,
    Enemy = 2,
    Static = 3
}

public class ActorManager : MonoBehaviour
{

    public BattleManager bm;
    public ActorController ac;
    public WeaponManager wm;
    public StateManager sm;
    public DirectorManager dm;
    public InteractionManager im;
    public InventoryManager inventory;
    public SoundManager sfxm;

    public ActorType actorType;
    public LayerMask whatIsEnemy;

    public float OnSkyTime = 0.0f;
    public float OnSkyMaxTime = 5.0f;

    private void Awake() {
        
        ac = GetComponent<ActorController>();
        GameObject model = ac.model;
        GameObject sensor = null;

        try
        {
            sensor = transform.Find("sensor").gameObject;
        }
        catch (System.Exception)
        {

        }
        if(actorType != ActorType.Static)
        {
            bm = Bind<BattleManager>(gameObject);
            wm = Bind<WeaponManager>(model);
            sm = Bind<StateManager>(gameObject);
            dm = Bind<DirectorManager>(gameObject);
            im = Bind<InteractionManager>(sensor);
            sfxm = Bind<SoundManager>(model);
            inventory = GetComponent<InventoryManager>();
        }
    }

    T Bind<T>(GameObject go) where T : IActorManagerInterface
    {
        if(go == null) return null;
        T temp = go.GetComponent<T>();
        if(temp == null)
        {
            temp = go.AddComponent<T>();
        }
        temp.am = this;
        return temp;
    }

    // Update is called once per frame
    void Update()
    {
        if(actorType != ActorType.Static)
        {
            // if(!ac.animator.GetBool("onGround"))
            // {
            //      OnSkyTime += Time.deltaTime;
            // }
            // else
            // { 
            //     OnSkyTime = 0.0f;
            // }
            //
            // if(OnSkyTime > OnSkyMaxTime && !sm.isDead)
            // {
            //     bm.Die();
            // }
            //
            // ac.animator.SetFloat("skyTime", OnSkyTime);

        }
    }

    public void SetCountBackState(bool value)
    {
        sm.isCountBackEnable = value;
    }


    public void Blocked()
    {
        ac.animator.SetTrigger("blocked");
    }

    public void Stunned()
    {
        ac.animator.SetTrigger("stunned");
    }

    public void ToggleLock(bool value)
    {
        ac.animator.SetBool("lock",value);
        if(!value) ac.EnableInput();
    }


    //FUNCTION:切换 单手-双手动画
    public void ChangeDualHand(bool righthand)
    {
        bool twoHand = ac.animator.GetBool("twoHand");        
        if(!twoHand)
        {
            if (righthand)
            {
                wm.SetWeaponOnUseVisiable(false, false);
                WeaponData wd = wm.GetWeaponDataOnUse(true);
                SetAtkAnimationInt(wd.wpAtkMotionID);
                AnimatorFactory.SetLocalMotion(ac.animator,wd.localMotionID2H);
            }
            else
            {
                wm.SetWeaponOnUseVisiable(false, true);
                WeaponData wd = wm.GetWeaponDataOnUse(false);
                SetAtkAnimationInt(wd.wpAtkMotionID);
                AnimatorFactory.SetLocalMotion(ac.animator, wd.localMotionID2H);
                //TODO:并且将左手武器送入又手
            }
           
        }
        //武器卸载
        else
        {
            wm.SetAllWeaponOnUseVisiable(true);
            SetAtkAnimationInt(wm.wcR.wdOnUse.wpAtkMotionID);
            AnimatorFactory.SetLocalMotion(ac.animator, wm.wcR.wdOnUse.localMotionID1H);
        }

        ac.animator.SetBool("twoHand",!twoHand); 
    }

    public void DoAction()
    {
        im.DoAction();
    }

    public void SetAtkAnimationInt(WpAtkMotionID id)
    {
        ac.animator.SetInteger("attackMotionType",(int)id);
    }



    public void OnRootMotionUpdate(Vector3 vec)
    {
        if (sm.isLock) return;
        ac.deltaPos += (1.0f * vec);
    }

    public void ChangeWeapon(bool rh)
    {
        ac.animator.SetTrigger("ChangeWeapon");
        ac.animator.SetBool("twoHand",false);
        wm.ChangeWeapon(rh);
    }

    //找到目前左下角快捷栏显示的物品，然后使用物品
    public void UseItem()
    {
        if(!sm.isGround) return;
        ItemEnum itemId = inventory.GetCurrentItem().iconID;
        ac.animator.SetInteger("itemOnUseID",(int)itemId);
        ac.animator.SetTrigger("useItem");
        inventory.UseItem(itemId);
    }

    public void NextItem()
    {
        inventory.NextItem();
    }


    
}
