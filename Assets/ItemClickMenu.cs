using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemClickMenu : MonoBehaviour
{
    public Button use;
    public Button equip;
    public Button setQk;
    public Button drop;

    private static ItemClickMenu instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
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
}
