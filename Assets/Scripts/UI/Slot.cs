using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class Slot : MonoBehaviour
{
    public ItemOnSlot itemOnSlot;
    private void Awake()
    {
        itemOnSlot = GetComponentInChildren<ItemOnSlot>();
    }

    public bool IsEmpty()
    {
         if (itemOnSlot == null) itemOnSlot = GetComponentInChildren<ItemOnSlot>();
        return itemOnSlot.IsEmpty();
    }

    public int GetIndex()
    {
        return transform.GetSiblingIndex();
    }

    public InventorySlotsManager GetInventorySlotsManager()
    {
        return GetComponentInParent<InventorySlotsManager>();
    }

    public void SetData(Item item, int count)
    {
        itemOnSlot.Holder = item;
        itemOnSlot.Count = count;
    }

    public void Clear()
    {
        itemOnSlot.Count = 0;
        itemOnSlot.Holder = null;
    }

    public InventoryType GetInventoryType()
    {
        return transform.GetComponentInParent<InventorySlotsManager>().inventoryType;
    }

}
