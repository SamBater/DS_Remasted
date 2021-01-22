using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;using UnityEngine.Events;

[Serializable]
public class NextEvent : UnityEvent<Item>{}

public enum ItemEnum
{
    EstusFlask = 0,       //原素瓶
    EstusFlask_Blank = 1, // 空的原素瓶
    KingsSoul = 2,       //王魂
    FlySword = 3,
    FireBottle = 4,
    BlackSword = 1001
}

[Serializable]
public class AddItemEvent : UnityEvent<ItemEnum,int,bool>{}

public class InventoryManager : IActorManagerInterface
{
    public Dictionary<ItemEnum,int> inventory;  //物品：数量
    public List<Item> quickUse = new List<Item>();
    public int current;
    public NextEvent NextItemEvent = new NextEvent();
    public AddItemEvent MyAddItemEvent;
    private void Awake()
    {

        inventory = new Dictionary<ItemEnum, int>();
        
        AddItem(ItemEnum.EstusFlask,10);
        AddItem(ItemEnum.FlySword,2);
        AddItem(ItemEnum.KingsSoul,1);
        AddItem(ItemEnum.EstusFlask_Blank,1);

        //TODO:测试用
        GameDatabase ItemFactory = GameDatabase.GetInstance();
        quickUse.Add(ItemFactory.GetItem(0));
        quickUse.Add(ItemFactory.GetItem(1));
        quickUse.Add(ItemFactory.GetItem(2));
        quickUse.Add(ItemFactory.GetItem(3));
    }

    private void Start() {
        //TODO:动态读取
        current = 0;
        if(gameObject.tag == "Player")
        {
            Item item = GetCurrentItem();
            if(item != null)
            NextItemEvent.Invoke(GetCurrentItem());
        }
    }

    public void NextItem()
    {
        current = (current + 1) % quickUse.Count;
        NextItemEvent.Invoke(GetCurrentItem());
    }

    public Item GetCurrentItem()
    {
        if(quickUse.Capacity > 0)
            return quickUse[current];
        return null;
    }

    public void AddItem(ItemEnum id,int count)
    {
        if (inventory == null) inventory = new Dictionary<ItemEnum, int>();
        bool newItem = !inventory.ContainsKey(id);
        if (!newItem)
        {
            inventory[id] += count;
        }
        else
        {
            inventory.Add(id,count);
        }

        MyAddItemEvent.Invoke(id,count,newItem);
    }

    public void AddQuickUse(ItemEnum itemEnum,int pos)
    {
        quickUse[pos] = GameDatabase.GetInstance().GetItem((int)itemEnum);
    }

    public void UseItem(ItemEnum itemId)
    {
        
        switch(itemId)
        {
            case ItemEnum.EstusFlask:
                break;
                
            case ItemEnum.FireBottle:
                break;

            default:
                break;
        }
    }
    
    

}
