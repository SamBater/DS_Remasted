using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class NextEvent : UnityEvent<Item>{}

public class InventoryManager : IActorManagerInterface
{
    public Dictionary<Item,int> inventory;  //物品：数量
    public Item[] quickUse = new Item[5];
    public Item[] rhWeapons = new Item[5];
    public Item[] lhWeapons = new Item[5];
    public int current;
    public NextEvent NextItemEvent = new NextEvent();
    public InventoryUI InventoryUI;
    private void Awake()
    {

        inventory = new Dictionary<Item, int>();

        for (int i = 0; i < 4; i++)
        {
            AddItem(Item.GetItem((ItemEnum)i),i+1);
        }

        for (int i = 1001; i < 1006; i++)
        {
            AddItem(Item.GetItem((ItemEnum)i),i-1000);
        }


        quickUse = new Item[5];
        //测试quickuse
        AddQuickUse(ItemEnum.FlySword,3);
        AddQuickUse(ItemEnum.FireBottle,1);
    }

    private void Start() {
        //TODO:动态读取
        current = 0;
        if(gameObject.CompareTag("Player"))
        {
            Item item = GetCurrentItem();
            if(item != null) 
                NextItemEvent.Invoke(GetCurrentItem());
        }

        rhWeapons = am.wm.GetWeapons(true);
        lhWeapons = am.wm.GetWeapons(false);
    }
    
    /// <summary>
    /// 切换武器
    /// </summary>
    public void NextItem()
    {
        current = (current + 1) % quickUse.Length;
        NextItemEvent.Invoke(GetCurrentItem());
    }

    /// <summary>
    /// 获取选中的Item
    /// </summary>
    /// <returns></returns>
    public Item GetCurrentItem()
    {
        if(quickUse.Length > 0)
            return quickUse[current];
        return null;
    }
    
    /// <summary>
    /// 添加Item 更新Inventory
    /// </summary>
    /// <param name="item"></param>
    /// <param name="count"></param>
    public void AddItem(Item item,int count)
    {
        if (inventory == null) inventory = new Dictionary<Item, int>();
        if (count == 0)
        {
            Debug.LogError("Item count should > 0.");
            return;
        }
        bool isNewItemForInventory = !inventory.ContainsKey(item);
        if (isNewItemForInventory)
        {
            inventory.Add(item,count);
        }
        else
        {
            inventory[item] += count;
        }
        InventoryUI.AddItem(item,inventory[item]);
    }
    
    /// <summary>
    /// 从Inventory中转移数据到QuickUse
    /// </summary>
    /// <param name="item"></param>
    /// <param name="count"></param>
    public void AddQuickUse(ItemEnum item, int pos)
    {
        quickUse[pos] = Item.GetItem(item);
        InventoryUI.qkism.Items = quickUse;
    }
    
    
    /// <summary>
    /// 更新UI
    /// </summary>
    /// <param name="item"></param>
    /// <param name="pos"></param>
    /// <param name="rh"></param>
    public void AddWeapon(bool rh)
    {
        if (rh)
        {
            InventoryUI.rhwpISM.Items = am.wm.GetWeapons(rh);
        }
        else
        {
            InventoryUI.lhwpISM.Items = am.wm.GetWeapons(rh);
        }
    }

    /// <summary>
    /// 减少Item数量
    /// </summary>
    /// <param name="item"></param>
    /// <param name="count"></param>
    public void DecreaseItem(Item item,int count)
    {
        if (!inventory.ContainsKey(item))
        {
            Debug.LogError("drop item is not in inventory.");
        }
        int countAfterDrop = inventory[item] - count;
        if (countAfterDrop > 0)
        {
            inventory[item] = countAfterDrop;
        }
        else if (countAfterDrop == 0)
        {
            inventory.Remove(item);
        }
        else
        {
            Debug.LogError($"{item}'s count is not enough.");
        }

        InventoryUI.DecreaseItem(item, countAfterDrop);
    }

    
    public int GetItemCount(ItemEnum itemEnum)
    {
        return inventory[Item.GetItem(itemEnum)];
    }


}
