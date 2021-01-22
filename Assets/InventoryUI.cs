using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    private Slot[] slots = null;
    private List<Item> items = new List<Item>();
    private void Awake()
    {
        slots = GetComponentsInChildren<Slot>();
    }

    public void Start()
    {
        slots = GetComponentsInChildren<Slot>();
    }

    public void Fresh(List<Item> items,Dictionary<ItemEnum,int> item_count = null)
    {
        for (int i = 0; i < items.Count; i++)
        {
            try
            {
                slots[i].itemOnSlot.item = items[i];
                slots[i].itemOnSlot.count = item_count != null ? item_count[items[i].GetID()] : 1;
                slots[i].Fresh();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }

    public Slot FindBlankSlot()
    {
        if(slots == null) slots = GetComponentsInChildren<Slot>();
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].itemOnSlot.item == null)
            {
                return slots[i];
            }
        }

        return null;
    }
}
