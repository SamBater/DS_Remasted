using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetailPanel : MonoBehaviour
{
    public Image icon;
    public Text itemName;
    public Text itemDes;
    private static ItemDetailPanel instance;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void SetItem(Item item)
    {
        icon.sprite = item.GetIcon();
        itemName.text = item.GetName();
        itemDes.text = item.GetDescription();
    }
    

    public static ItemDetailPanel GetInstance()
    {
        return instance;
    }
}
