using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum InterractionEvent{
    frontStab,
    openbox,
    pullLevel,
    pickup
}

[RequireComponent(typeof(CapsuleCollider))]
public class InteractionManager : IActorManagerInterface
{
    public CapsuleCollider interCol;
    public List<EventCasterManager> overlapEcastms = new List<EventCasterManager>();
    void Start()
    {
        interCol = GetComponent<CapsuleCollider>();
    }

    void OnTriggerEnter(Collider other)
    {
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
            if(ecm.interractionEvent == InterractionEvent.pickup)
            {
                PickUpItem();
                return;
            }

            
            //修正玩家朝向
            transform.position = ecm.am.transform.position + ecm.am.transform.TransformVector(ecm.offset);
            am.ac.model.transform.LookAt(ecm.am.transform,Vector3.up);
            transform.LookAt(ecm.am.transform,Vector3.up);

            transform.forward = ecm.am.transform.forward * -1.0f;
            am.ac.model.transform.forward = ecm.am.transform.forward * -1.0f;


            am.ac.enableTurnDirection = false;
            if(ecm.interractionEvent == InterractionEvent.frontStab)
            {
                am.ac.model.SendMessage("WeaponDisable");
                overlapEcastms[0].am.ac.model.SendMessage("WeaponDisable");
                am.dm.PlayTimeLine("frontStab",am,ecm.am);
            }
            else if(ecm.interractionEvent == InterractionEvent.openbox)
            {
                am.wm.SetAllWeaponOnUseVisiable(false);
                am.dm.PlayTimeLine("openBox",am,ecm.am);
            }
            else if(ecm.interractionEvent == InterractionEvent.pullLevel)
            {
                am.dm.PlayTimeLine("leverUp",am,ecm.am);
            }
            else if(ecm.interractionEvent == InterractionEvent.pickup)
            {
                PickUpItem();
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
            //UIManager.instance.ShowItemsOnGround(items[i],counts[i]);
            if(am.inventory.inventory.ContainsKey(items[i]))
                am.inventory.inventory[items[i]] += counts[i];
            else
                am.inventory.inventory.Add(items[i],counts[i]);
        }
        UIManager.instance.ShowItemOnGround(items,counts);
    }
}
