using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    private Slot[] slots = null;

    private void Awake()
    {
        slots = GetComponentsInChildren<Slot>();
    }

    public void Start()
    {
        slots = GetComponentsInChildren<Slot>();
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
