﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


public enum InterractionEvent{
    FrontStab,
    Openbox,
    PullLevel,
    Pickup,
    BornFireLit,
    BornFireSit
}

public class InteractionManager : IActorManagerInterface
{
    public List<EventCasterManager> overlapEcastms = new List<EventCasterManager>();
    void Awake()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer != LayerMask.NameToLayer("Caster")) return;
        EventCasterManager[] ecastms = other.GetComponents<EventCasterManager>();
        foreach(var ecastm in ecastms)
        {
            if(!overlapEcastms.Contains(ecastm))
            {
                overlapEcastms.Add(ecastm);
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        EventCasterManager[] ecastms = other.GetComponents<EventCasterManager>();
        foreach(var ecastm in ecastms)
        {
            if(overlapEcastms.Contains(ecastm))
            {
                overlapEcastms.Remove(ecastm);
            }
        }
    }

    public void AddEvent(InterractionEvent _event)
    {
        EventCasterManager em = new EventCasterManager();
        em.interractionEvent = _event;
        em.active = true;
        overlapEcastms.Add(em);
    }

    public EventCasterManager GetFirstContactEventCaster()
    {
        if(overlapEcastms.Count > 0)
            return overlapEcastms[0];
        else
            return null;
    }

    public void DoAction()
    {
        EventCasterManager ecm = GetFirstContactEventCaster();
        if(overlapEcastms.Count > 0)
        {
            if(!ecm.active) return;
            //if(!BattleManager.CheckAnglePlayer(ac.model,im.overlapEcastms[0].am.gameObject,30.0f)) return;
            //im.overlapEcastms[0].active = false;
            if(ecm.interractionEvent == InterractionEvent.Pickup)
            {
                PickUpItem();
                return;
            }
            
            //修正位置、朝向

            if (ecm.offset != Vector3.zero)
            {
                am.ToggleLock(true);
                transform.position = overlapEcastms[0].am.transform.position +
                                     overlapEcastms[0].am.transform.TransformVector(overlapEcastms[0].offset);
            }
            am.ac.model.transform.LookAt(ecm.am.transform.position, Vector3.up);

            am.ac.enableTurnDirection = false;
            if(ecm.interractionEvent == InterractionEvent.FrontStab)
            {
                am.ac.model.SendMessage("WeaponDisable");
                overlapEcastms[0].am.ac.model.SendMessage("WeaponDisable");
                am.dm.PlayTimeLine("frontStab",am,ecm.am);
            }
            else if(ecm.interractionEvent == InterractionEvent.Openbox)
            {
                am.wm.SetAllWeaponOnUseVisiable(false);
                am.dm.PlayTimeLine("openBox",am,ecm.am);
            }
            else if(ecm.interractionEvent == InterractionEvent.PullLevel)
            {
                am.dm.PlayTimeLine("leverUp",am,ecm.am);
            }
            else if(ecm.interractionEvent == InterractionEvent.Pickup)
            {
                PickUpItem();
            }
            else if (ecm.interractionEvent == InterractionEvent.BornFireLit)
            {
                //玩家播放点火动画,特定时间点播放声效.
                am.ac.LitFire();
                //篝火燃起来
                BoneFire boneFire = ecm.GetComponentInParent<BoneFire>();
                boneFire.Fire();
                boneFire.FireBurst();
                //保存篝火信息
            }
            else if (ecm.interractionEvent == InterractionEvent.BornFireSit)
            {
                am.ac.SitFire();
                am.wm.SetAllWeaponOnUseVisiable(false);
                am.ActorUIManager.ShowFirePanel();
            }
            
        }
    }

    public void PickUpItem()
    {
        am.ac.animator.SetTrigger("pickUp");
        ItemOnGround itemOnGround = overlapEcastms[0].gameObject.GetComponent<ItemOnGround>();
        itemOnGround.OnAction();

        am.sfxm.PickUp();

        List<ItemEnum> items = itemOnGround.items;
        List<int> counts = itemOnGround.counts;
        for(int i=0;i<items.Count;i++)
        {
            am.inventory.AddItem(Item.GetItem(items[i]),counts[i]);
        }
        am.ActorUIManager.ShowItemOnGround(items,counts);
    }
    
}
