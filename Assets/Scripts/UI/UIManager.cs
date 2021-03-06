﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Image weaponIconL;
    public Image weaponIconR;
    public Image itemIcon;
    public Text itemName;
    public Text soulsBar;
    public GameObject itemTips;
    public Image itemOnGroundIcon;
    public Text itemOnGroundName;
    public Text itemOnGroundCount;
    public GameObject optionsPanel;
    public GameObject firePanel;
    public GameObject inventoryPanel;

    private void Awake()
    {
        
    }

    public void UpdateWeaponIcon(int id,bool rh)
    {
        Sprite icon = GameDatabase.GetInstance().GetItem(id).icon;
        if(rh) weaponIconR.sprite = icon;
        else weaponIconL.sprite = icon;
    }

    public void UpdateItemIcon(Item newItem)
    {
        itemIcon.sprite = newItem.GetIcon();
        itemName.text = newItem.GetName();
    }

    public void UpdateSoulsBar()
    {
        
    }

    public void ShowItemOnGround(ItemEnum item,int count)
    {
        StartCoroutine(ShowTheItemTips(item,count));
    }

    public void ShowItemOnGround(List<ItemEnum> items,List<int> counts)
    {
        StartCoroutine(ShowTheItemTips(items,counts));
    }

    IEnumerator ShowTheItemTips(ItemEnum item,int count)
    {
        // itemOnGroundIcon.sprite = ItemFactory.GetItem(item).GetIcon();
        // itemOnGroundName.text = ItemFactory.GetItem((int)item).GetName();
        itemOnGroundCount.text = count.ToString();
        itemTips.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        itemTips.SetActive(false);
    }

    IEnumerator ShowTheItemTips(List<ItemEnum> item,List<int> count)
    {
        for(int i=0;i<item.Count;i++)
        {
            itemOnGroundIcon.sprite = GameDatabase.GetInstance().GetItem((int)item[i]).GetIcon();
            itemOnGroundName.text = GameDatabase.GetInstance().GetItem((int)item[i]).GetName();
            itemOnGroundCount.text = count[i].ToString();
            itemTips.SetActive(true);
            yield return new WaitForSeconds(1.5f);
        }
        itemTips.SetActive(false);
    }

    public void ShowOptionPanel()
    {
        optionsPanel.SetActive(!optionsPanel.activeSelf);
    }

    public void ShowFirePanel()
    {
        firePanel.SetActive(!firePanel.activeSelf);
    }

    public void ShowInventoryPanel()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
    }
}
