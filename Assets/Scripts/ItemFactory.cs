using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFactory : MonoBehaviour
{
    public static Database db;
    public static Dictionary<ItemEnum,Item> items;
    private void Awake() 
    {
        db = new Database("Items");
        items = new Dictionary<ItemEnum, Item>();
        Sprite[] itemIcons = Resources.LoadAll<Sprite>("UI/item");
        for(int i=0;i<db.database.Count;i++)
        {
            Item item = new Item();
            item.iconID = (ItemEnum)db.database[i]["IconID"].i;
            item.itemName = db.database.keys[i];
            item.sprite = itemIcons[(int)item.iconID];
            items.Add(item.iconID,item);
        }
    }

    public static Item GetItem(int id)
    {
        return items[(ItemEnum)id];
    }

    public static Item GetItem(ItemEnum item)
    {
        return items[item];
    }
    
}
