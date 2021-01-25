using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemClickMenu : MonoBehaviour
{
    public Button use;
    public Button equip;
    public Button drop;
    public InventoryManager m_im;
    private static ItemClickMenu instance;

    public ItemOnSlot onPactItem;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            m_im = FindObjectOfType<InventoryManager>();
        }
        gameObject.SetActive(false);
    }

    private ItemClickMenu()
    {
        
    }

    public static ItemClickMenu GetInstance()
    {
        return instance;
    }

    public void ShowConsumable()
    {
        use.gameObject.SetActive(true);
        equip.gameObject.SetActive(false);
    }

    public void ShowWeapon()
    {
        use.gameObject.SetActive(false);
        equip.gameObject.SetActive(true);
    }
    
    public void DropItem()
    {
        m_im.DecreaseItem(onPactItem.Holder, 1);
        gameObject.SetActive(false);
    }
}
