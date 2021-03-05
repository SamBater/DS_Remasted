using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public enum WeaponHold
{
    _1h,
    _2hR = 2,
    _2hL = 3
}
[System.Serializable]
public class StateManager : IActorManagerInterface
{
    [Header("Common proproties")]
    public float hp;
    public float maxhp;

    public float Naili;
    public float maxEndurance;
    
    public Damage defenceRate = new Damage(); //减伤率

    public Damage rhATK; //右手面板伤害
    public Damage lhATK;
    
    [Header("Base states")]
    public int Level;
    public int vitality;    //生命力
    public int attunement;  //记忆力
    public int endurance;   //耐力
    public BaseStates baseStates = new BaseStates();
    public WeaponHold weaponHold = WeaponHold._1h;

    [Header("1st flag")]
    public bool isGround;
    public bool isJump;
    public bool isRoll;
    public bool isJab;
    public bool isAttack;
    public bool isDefence;
    public bool isBlocked;
    public bool isCountBack;
    public bool isHeal;
    public bool isCountBackEnable;
    public bool isHit;
    public bool isDead;
    public bool isLock;
    
    [Header("2st flag")]
    public bool isImmortal;

    public bool isWalk;
    public bool isAllowDefense;
    public bool isCountBackSuccess;

    public event Action<float> onHealthPctChanged = delegate {};
    public event Action<float> onEndurancePctChanged = delegate {};
    public static char[] levelMap = new char[6] { 'E', 'D' ,'C','B','A','S'};
    private void Awake() {
        hp = maxhp;
        Naili = maxEndurance;

    }

    void Update() 
    {
        isAttack = am.ac.CheckStateTag("attack");
        isBlocked = am.ac.CheckState("blocked");
        isDead = am.ac.CheckState("die");
        isGround = am.ac.CheckState("Ground");
        isHit = am.ac.CheckState("hit");
        isJab = am.ac.CheckState("jab");
        isRoll = am.ac.CheckState("roll");
        isAllowDefense = isGround || isBlocked;
        //isDefence = isAllowDefense && am.ac.CheckState("shieldUp","defence");
        isDefence = am.ac.CheckStateTag("defence");
        //isImmortal = isJab || isRoll;
        isCountBack = am.ac.CheckState("countBack");
        isCountBackSuccess =  isCountBack && isCountBackEnable; //isCountBackEnable 由动画事件控制 打开有限时间
        isLock = am.ac.CheckState("lock");
        isHeal = am.ac.CheckStateTag("heal");
        isWalk = isGround && am.ac.playerInput.GetInputMag() > 0.3f;
    }

    public void AddHp(float value)
    {
        hp += value;
        hp = Mathf.Clamp(hp,0,maxhp);
        float pct = hp / maxhp;
        onHealthPctChanged(pct);
    }

    public void AddEndurance(float value)
    {
        Naili += value;
        Naili = Mathf.Clamp(Naili, 0f, maxEndurance);
        float pct = Naili / maxEndurance;
        onEndurancePctChanged(pct);
    }


    /// <summary>
    /// 根据正在使用的武器，计算面板伤害
    /// </summary>
    /// <param name="rh"></param>
    /// <returns></returns>
    public Damage ComputerPanelATK(bool rh)
    {
        Damage ATK = rh ? rhATK : lhATK;
        WeaponData wd = am.wm.GetWeaponDataOnUse(rh);
        if(ATK == null)
        {
            ATK = new Damage();
        }
        ATK.physical = wd.weaponItem.ATK.physical + wd.weaponItem.bounusLv.strength * baseStates.strength; // TODO:加敏捷
        ATK.magical = wd.weaponItem.ATK.magical + wd.weaponItem.bounusLv.intelligence * baseStates.intelligence;
        ATK.fire = wd.weaponItem.ATK.fire + wd.weaponItem.bounusLv.intelligence * baseStates.intelligence;
        return ATK;
    }

    public char LookUpLevel(int level)
    {
        return levelMap[level];
    }

    public Damage GetPanelATK(bool rh)
    {
        
        if (rh) return rhATK = ComputerPanelATK(rh);
        else return lhATK = ComputerPanelATK(rh);
    }


    
}
