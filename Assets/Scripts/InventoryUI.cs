using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public InventorySlotsManager ism;
    public InventorySlotsManager qkism;
    public InventorySlotsManager rhwpISM;
    public InventorySlotsManager lhwpISM;
    public InventoryManager im;
    public Dictionary<Item, int> allItemPos;
    public Dictionary<Item, int> weaponItemPos;
    public Dictionary<Item, int>[] ItemsPoseDictionaries = new Dictionary<Item, int>[(int)ItemType.ItemTypeCount];
    public Toggle allItemToggle;
    public Toggle weaponToggle;
    public Toggle[] Toggles = new Toggle[3];

    private void Awake()
    {
        allItemPos = new Dictionary<Item, int>();
        weaponItemPos = new Dictionary<Item, int>();
        ItemsPoseDictionaries = new Dictionary<Item, int>[(int) ItemType.ItemTypeCount];
    }

    /// <summary>
    /// 告知InventoryUI更新第pos个Slot中的Item
    /// </summary>
    /// <param name="item"></param>
    /// <param name="pos"></param>
    /// <param name="count"></param>
    public void AddItem(Item item,int count)
    {
        //已经存在,更新count即可
        //优先更新QuickSlot和WeaponSlot
        // if (Consumables.Contains(item))
        // {
        //     
        // }
        // else if (RHWeapons.Contains(item))
        // {
        //     
        // }
        if (allItemPos.ContainsKey(item))
        {
            int pos = allItemPos[item];
            ism.UpdateCount(pos,count);
        }
        //否则，找给空槽进行补充
        else
        {
            int pagePos = -1;
            switch (item.GetItemType())
            {
                case ItemType.Weapon:
                    pagePos = ism.FindBlankSlot(weaponItemPos);
                    weaponItemPos.Add(item,pagePos);
                    break;
            }
            
            int pos = ism.FindBlankSlot(allItemPos);
            allItemPos.Add(item,pos);
            
            //更新正在浏览的页面
            if(allItemToggle.isOn)
                ism.UpdateSlot(item,pos,count);
            else 
                ism.UpdateSlot(item,pagePos,count);
        }

    }
    
    
    /// <summary>
    /// 在UI中删除count个Item
    /// </summary>
    /// <param name="item"></param>
    /// <param name="count"></param>
    public void DecreaseItem(Item item,int count)
    {
        int pos = -1;
        ItemType type = item.GetItemType();
        if (allItemToggle.isOn)
        {
            pos = allItemPos[item];
        }
        else if (weaponToggle.isOn && type == ItemType.Weapon)
        {
            pos = weaponItemPos[item];
        }
        //刷新Count
        if (count > 0)
        {
            ism.UpdateSlot(item,pos,count);
        }
        //清空slot，并且删除位置索引
        else if (count == 0)
        {
            ism.Clear(pos);
            if(allItemPos.ContainsKey(item))
                allItemPos.Remove(item);
            if (type == ItemType.Weapon)
                weaponItemPos.Remove(item);
        }
    
    }

    public void TogglePage(Toggle t)
    {
        if(!t.isOn) return; //只响应on
        if(t.name.Equals("All"))
            ism.ShowItem(allItemPos,im.inventory);
        else
            ism.ShowItem(weaponItemPos,im.inventory);
    }
}
