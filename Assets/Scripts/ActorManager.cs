﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ActorType
{
    Player = 1,
    Enemy = 2,
    Static = 3
}

public class ActorManager : MonoBehaviour,ISaveable
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
    public GameObject model;
    private ISaveable saveableImplementation;
    public UIManager ActorUIManager = new UIManager();
    private void Awake()
    {
        try
        {
            ac = GetComponent<ActorController>();
            model = ac.model;
        }
        catch (System.Exception)
        {
            model = gameObject;
        }
        if(actorType != ActorType.Static)
        {
            bm = Bind<BattleManager>(gameObject);
            wm = Bind<WeaponManager>(model);
            sm = Bind<StateManager>(gameObject);
            dm = Bind<DirectorManager>(gameObject);
            im = Bind<InteractionManager>(gameObject);
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
        ac.playerInput.InputToggle(!value);
        ac.StopMove(value);
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
                SetAtkAnimationInt(wd.weapon.wpAtkMotionID);
                AnimatorFactory.SetLocalMotion(ac.animator,wd.weapon.localMotionID2H);
            }
            else
            {
                wm.SetWeaponOnUseVisiable(false, true);
                WeaponData wd = wm.GetWeaponDataOnUse(false);
                SetAtkAnimationInt(wd.weapon.wpAtkMotionID);
                AnimatorFactory.SetLocalMotion(ac.animator, wd.weapon.localMotionID2H);
                //TODO:并且将左手武器送入又手
            }
           
        }
        //武器卸载
        else
        {
            wm.SetAllWeaponOnUseVisiable(true);
            SetAtkAnimationInt(wm.wcR.weaponDataOnUse.weapon.wpAtkMotionID);
            AnimatorFactory.SetLocalMotion(ac.animator, wm.wcR.weaponDataOnUse.weapon.localMotionID1H);
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
        ItemEnum itemId = inventory.GetCurrentItem().GetID();
        ac.animator.SetInteger("itemOnUseID",(int)itemId);
        ac.animator.SetTrigger("useItem");
        inventory.UseItem(itemId);
    }

    public void NextItem()
    {
        inventory.NextItem();
    }
    
    public void Die()
    {
        wm.WeaponDisable();
        if (CompareTag("Player"))
        {
            //屏幕置灰,播放"You died"
            ac.cc.fade = true;
            
            
            //在五秒前的位置生成 "魂" 带有EventCast

            //重载场景,插入过渡LoadScene
            //不重载的物体：玩家信息；Boss、精英怪；宝箱、拉杆、门的状态；
            GameManager.LoadScene();
        }

        else if (CompareTag("Enemy"))
        {
            
        }
    }


    public void PopulateSaveData(SaveData saveData)
    {
        if (CompareTag("Player"))
        {
            saveData.player_pos = transform.position;
        }
        else if(CompareTag("BornFire"))
        {
            BoneFire boneFire = GetComponent<BoneFire>();
            SaveData.MyStruct myStruct;
            myStruct.u_id = GetInstanceID();
            myStruct.activity = boneFire.lit;
            saveData.reborn.Add(myStruct);
        }
    }

    public void LoadFromSaveData(SaveData saveData)
    {
        if (CompareTag("Player"))
        {
            transform.position = saveData.player_pos;
        }
        else if(CompareTag("BornFire"))
        {
            for (int i = 0; i < saveData.reborn.Capacity; i++)
            {
                if (saveData.reborn[i].u_id == GetInstanceID())
                {
                    GetComponent<BoneFire>().lit = saveData.reborn[i].activity;
                }
            }
        }
    }
}
