using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Database : MonoBehaviour
{
    private string databaseFileName = "WeaponDatabase";
    public readonly JSONObject database;
    public Database(string _name)
    {
        databaseFileName = _name;
        TextAsset weaponContent = Resources.Load(databaseFileName) as TextAsset;
        database = new JSONObject(weaponContent.text);
    }
}
