using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public enum InventoryType
{
    PlayerInventorySlots = -2,
    NPCInventorySlots = -1,
    RWeaponsSlots = 1,
    LWeaponsSlots = 2,
    QuickUseSlots = 3
}

public class InventorySlotsManager : MonoBehaviour
{
    public List<Slot> slots = new List<Slot>();
    private Item[] items;
    public InventoryUI inventoryUI;
    public Item[] Items
    {
        get { throw new NotImplementedException(); }
        set
        {
            if (items == null) items = new Item[slots.Capacity];
            items = value;
            ShowItem(items);
        }
    }

    public GameObject slotPrototype;
    public InventoryType inventoryType;
    private void Awake()
    {
        for (int i = 0; i < slots.Capacity; i++)
        {
            GameObject slot = Instantiate<GameObject>(slotPrototype,transform);
            slots[i] = slot.GetComponent<Slot>();
        }
        //gameObject.SetActive(false);
    }

    /// <summary>
    ///找到itemPos中的空位置
    /// </summary>
    /// <returns></returns>
    public int FindBlankSlot(Dictionary<Item,int> itemPos)
    {
        if(slots == null) slots = GetComponentsInChildren<Slot>().ToList();
        
        //有位置就返回
        int[] pos = itemPos.Values.ToArray();
        for (int i = 0; i < slots.Capacity; i++)
        {
            if (!pos.Contains(i))
            {
                return i;
            }
        }
        
        //没位置了就新增一个
        GameObject slot = Instantiate<GameObject>(slotPrototype,transform);
        slots.Add(slot.GetComponent<Slot>());
        return slots.Capacity ;
    }
    
    /// <summary>
    /// 交换item的位置索引
    /// </summary>
    /// <param name="itemPos"></param>
    /// <param name="itemDrag"></param>
    /// <param name="itemDrop"></param>
    /// <param name="newPos"></param>
    /// <param name="oldPos"></param>
    public void SwapPos(Dictionary<Item, int> itemPos, Item itemDrag,Item itemDrop,int newPos,int oldPos)
    {
        itemPos[itemDrag] = newPos;
        if (itemDrop && itemPos.ContainsKey(itemDrop))
            itemPos[itemDrop] = oldPos;
    }
    
    /// <summary>
    /// 按图索骥,根据itemPos显示
    /// </summary>
    /// <param name="itemPos"></param>
    public void ShowItem(Dictionary<Item,int> itemPos,
        Dictionary<Item,int> itemCount)
    {
        bool[] taken = new bool[slots.Capacity];
        for (int i = 0; i < taken.Length; i++)
            taken[i] = false;
        //显示物品
        foreach (KeyValuePair<Item,int> keyValuePair in itemPos)
        {
            Item item = keyValuePair.Key;
            int pos = keyValuePair.Value;
            int count = itemCount[item]; 
            slots[pos].SetData(item,count);
            taken[pos] = true;
            Debug.Log(item + pos.ToString());
        }
        //隐藏杂项
        for (int i = 0; i < slots.Capacity; i++)
        {
            if(taken[i] == false && !slots[i].IsEmpty())
                slots[i].Clear();
        }
    }
    
    /// <summary>
    /// 适配一组Item图标;如果不提供Item:Count,则不显示数量(显示1)
    /// </summary>
    /// <param name="items"></param>
    public void ShowItem(Item[] items,Dictionary<Item,int> itemCount = null)
    {
        int size = Math.Min(items.Length, slots.Capacity);
        for (int i = 0; i < size; i++)
        {
            int count = itemCount != null && itemCount.ContainsKey(items[i]) ? itemCount[items[i]] : 1;
            slots[i].SetData(items[i],count);
        }
    }
    
    /// <summary>
    /// 判断是否包含数据
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool ContainsItem(Item item)
    {
        for (int i = 0; i < slots.Capacity; i++)
        {
            if (slots[i].itemOnSlot.Holder && slots[i].itemOnSlot.Holder == item)
            {
                return true;
            }
        }

        return false;
    }
    
    /// <summary>
    /// 清空第pos个槽
    /// </summary>
    /// <param name="pos"></param>
    public void Clear(int pos)
    {
        slots[pos].Clear();
    }
    
    /// <summary>
    /// 更新Slot显示
    /// </summary>
    /// <param name="item"></param>
    /// <param name="pos"></param>
    /// <param name="count"></param>
    public void UpdateSlot(Item item,int pos,int count)
    {
        UpdateCount(pos,count);
        UpdateItem(pos,item);
    }
    
    /// <summary>
    /// 增加Item显示数量
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="count"></param>
    public void UpdateCount(int pos,int count)
    {
        slots[pos].itemOnSlot.Count += count;
    }
    
    /// <summary>
    /// 更新槽的Item
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="item"></param>
    public void UpdateItem(int pos, Item item)
    {
        slots[pos].itemOnSlot.Holder = item;
    }
}
