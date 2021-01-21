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

    public void Fresh()
    {
        if (itemOnSlot == null) itemOnSlot = GetComponent<ItemOnSlot>();
        itemOnSlot.Fresh(itemOnSlot.item,itemOnSlot.count);
    }
}
