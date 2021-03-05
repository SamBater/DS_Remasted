using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 从表格读取配置数据
/// </summary>
interface ILoadFromTable
{
    void LoadData(string[] col);
}

public enum ItemType
{
    Consumable = 0,
    Weapon = 1,
    ItemTypeCount = 2
}


public enum ItemEnum
{
    Null = -1,
    EstusFlask = 0,       //原素瓶
    EstusFlask_Blank = 1, // 空的原素瓶
    KingsSoul = 2,       //王魂
    FlySword = 3,
    FireBottle = 4,
    Fist = 1000,
    BlackSword = 1001,
    GreatSword = 1002,
    LongSpider = 1004,
    KnightShield = 1005,
    DragonSlayer = 1008
}

[Serializable]
public class Item : ScriptableObject,ILoadFromTable
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

    public static Item GetItem(ItemEnum itemEnum)
    {
        return GameDatabase.GetInstance().GetItem((int) itemEnum);
    }
}
