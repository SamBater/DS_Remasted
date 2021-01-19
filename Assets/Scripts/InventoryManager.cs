using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;using UnityEngine.Events;

[Serializable]
public class NextEvent : UnityEvent<Item>{}

public enum ItemEnum
{
    EstusFlask = 5,       //原素瓶
    EstusFlask_Blank = 4, // 空的原素瓶
    KingsSoul = 60,       //王魂
    FlySword = 21,
    FireBottle = 23
}

public class InventoryManager : MonoBehaviour
{
    public Dictionary<ItemEnum,int> inventory;  //物品：数量
    public List<Item> quickUse = new List<Item>();
    public int current;
    public NextEvent NextItemEvent = new NextEvent();
    
    private void Awake() 
    {
        inventory = new Dictionary<ItemEnum, int>();
        
        inventory.Add(ItemEnum.EstusFlask,10);
        inventory.Add(ItemEnum.FlySword,2);
        
        //TODO:测试用
        GameDatabase ItemFactory = GameDatabase.GetInstance();
        quickUse.Add(ItemFactory.GetItem(0));
        quickUse.Add(ItemFactory.GetItem(1));
        quickUse.Add(ItemFactory.GetItem(2));
        quickUse.Add(ItemFactory.GetItem(3));
        quickUse.Add(ItemFactory.GetItem(4));
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
