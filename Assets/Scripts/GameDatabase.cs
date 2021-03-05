using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Versioning;
using System.Text;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEditor.Experimental;
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
        instance.LoadData<WeaponItem>(weaponCSVPath);
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

            T item = null;
            
            string assetPath = $"Assets/Resources/ScriptObjects/{csv.Split('.')[0]}/{line[1]}.asset";
            FileInfo fileInfo = new FileInfo(assetPath);
            if (!fileInfo.Exists)
            {
                item = ScriptableObject.CreateInstance<T>();
                LoadItem(item,line);
                try
                {
                    AssetDatabase.CreateAsset(item, assetPath);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

            }
            else
            {
                item = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                LoadItem(item,line);
            }
            EditorUtility.SetDirty(item);
        }
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    public Item GetItem(int id)
    {
        return instance.items[id];
    }

    public void LoadItem(Item item,string[] line)
    {
        try
        {
            item.LoadData(line);
            if (item.GetItemType() == ItemType.Consumable)
                item.icon = consumableIcons[item.GetItemIconID()];
            else if (item.GetItemType() == ItemType.Weapon)
                item.icon = weaponIcons[item.GetItemIconID()];
            items.Add((int)item.GetID(),item);
            //Debug.Log("Add " + item);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
