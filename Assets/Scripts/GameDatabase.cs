using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Versioning;
using System.Text;
using UnityEditor;
using UnityEngine;

public class GameDatabase
{
    static GameDatabase instance = null;
    private Dictionary<int, Item> items = new Dictionary<int, Item>();
    private Sprite[] consumableIcons = null;
    private Sprite[] weaponIcons = null;
    private GameDatabase()
    {
        
    }
    public static GameDatabase GetInstance()
    {
        if (instance == null)
        {
            LoadData();
        }

        return instance;
    }
    [MenuItem("Utilities/Generate ScriptObjects")]
    public static void LoadData()
    {
        string weaponCSVPath = "Weapons.csv";
        string consumableCSVPath = "Consumables.csv";

        instance = new GameDatabase();
        instance.consumableIcons = Resources.LoadAll<Sprite>("UI/item");
        instance.weaponIcons = Resources.LoadAll<Sprite>("UI/weapon1");
        instance.LoadData<Weapon>(weaponCSVPath);
        instance.LoadData<Consumable>(consumableCSVPath);
    }
    
    void LoadData<T>(string csv) where T : Item
    {
        string rootPath = "/Tables/";
        string path = Application.dataPath+rootPath+csv;
        string[] allLines = File.ReadAllLines(path,Encoding.UTF8);

        for (int i = 1; i < allLines.Length; i++)
        {
            string s = allLines[i];
            string[] line = s.Split(',');

            T item = ScriptableObject.CreateInstance<T>();
            try
            {
                item.LoadData(line);
                if (item.GetItemType() == ItemType.Consumable)
                    item.icon = consumableIcons[item.GetItemIconID()];
                else if (item.GetItemType() == ItemType.Weapon)
                    item.icon = weaponIcons[item.GetItemIconID()];
                items.Add((int)item.GetID(),item);
                Debug.Log("Add " + item);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            AssetDatabase.CreateAsset(item, $"Assets/ScriptObjects/{csv.Split('.')[0]}/{item.GetName()}.asset");
        }

        AssetDatabase.SaveAssets();
    }

    public Item GetItem(int id)
    {
        return instance.items[id];
    }
}
