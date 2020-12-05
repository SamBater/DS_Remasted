using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemEnum
{
    EstusFlask = 5,       //原素瓶
    EstusFlask_Blank = 4, // 空的原素瓶
    KingsSoul = 60,       //王魂
    FlySword = 21,
    FireBottle = 23
}

public class Item
{
    public ItemEnum iconID;
    public string itemName;
    public Sprite sprite;
    public Item()
    {

    }
    public Item(ItemEnum _iconId)
    {
        iconID = _iconId;
    }
}

public class InventoryManager : MonoBehaviour
{
    public Dictionary<ItemEnum,int> inventory;  //物品：数量
    public List<Item> quickUse = new List<Item>();
    public int current;
    
    
    private void Awake() 
    {
        inventory = new Dictionary<ItemEnum, int>();
        
        inventory.Add(ItemEnum.EstusFlask,10);
        inventory.Add(ItemEnum.FlySword,2);
        //TODO:测试用
        
        // quickUse.Add(ItemFactory.GetItem(5));
        // quickUse.Add(ItemFactory.GetItem(4));
        // quickUse.Add(ItemFactory.GetItem(60));
        // quickUse.Add(ItemFactory.GetItem(23));
        // quickUse.Add(ItemFactory.GetItem(21));
    }

    private void Start() {
        //TODO:动态读取
        current = 0;
        if(gameObject.tag == "Player")
        {
            Item item = GetCurrentItem();
            if(item != null)
            UIManager.instance.UpdateItemIcon(item.iconID);
        }
    }

    public void NextItem()
    {
        current = (current + 1) % quickUse.Count;
        UIManager.instance.UpdateItemIcon(GetCurrentItem().iconID);
    }

    public Item GetCurrentItem()
    {
        if(quickUse.Capacity > 0)
            return quickUse[current];
        return null;
    }

    public void UseItem(ItemEnum itemId)
    {
        
        switch(itemId)
        {
            case ItemEnum.EstusFlask:
                break;
                
            case ItemEnum.FireBottle:
                break;

            default:
                break;
        }
    }

}
