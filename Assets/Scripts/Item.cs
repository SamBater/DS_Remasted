using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface ILoadAble
{
    void LoadData(string[] col);
}

public enum ItemType
{
    Consumable = 0,
    Weapon = 1
}

public class Item : ScriptableObject,ILoadAble
{
    private int id;
    [SerializeField]
    protected ItemType itemType;
    [SerializeField]
    protected string itemName;
    protected string description;
    public Sprite icon;

    private int iconID;

    public virtual void LoadData(string[] col)
    {
        try
        {
            id = int.Parse(col[0]);
            itemName = col[1];
            description = col[2];
            iconID = int.Parse(col[3]);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

    }

    public ItemEnum GetID()
    {
        return (ItemEnum)id;
    }

    public string GetName()
    {
        return itemName;
    }

    public Sprite GetIcon()
    {
        return icon;
    }

    public string GetDescription()
    {
        return description;
    }

    public override string ToString()
    {
        return $"{id} : {itemName}";
    }

    public ItemType GetItemType()
    {
        return itemType;
    }

    public int GetItemIconID()
    {
        return iconID;
    }
}
