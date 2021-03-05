using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    
    public SaveData()
    {
        
    }

    [Serializable]
    public struct InteractObject
    {
        public int uid;
        public bool activity;
    }
    
    [Serializable]
    public struct ItemSave
    {
        public int index;
        public int itemID;
        public int count;
    }

    [Serializable]
    public struct AcotrSave
    {
        public int uid;
        public List<ItemSave> ItemSaves;
    }

    public Vector3 PlayerPos;
    public List<InteractObject> Reborn = new List<InteractObject>();
    public List<AcotrSave> AcotrSaves = new List<AcotrSave>();

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public void ReadFromJson(string json)
    {
        JsonUtility.FromJsonOverwrite(json,this);
    }
}

public interface ISaveable
{
    void PopulateSaveData(SaveData saveData);
    void LoadFromSaveData(SaveData saveData);
}
