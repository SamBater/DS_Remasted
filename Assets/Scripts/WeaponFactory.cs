using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFactory : MonoBehaviour
{
    public static Database weaponDB;
    public static WeaponFactory instance;
    public static Dictionary<string,GameObject> weapons = new Dictionary<string, GameObject>();
    static Sprite[] weaponIcons;

    private void Awake() {
        if(instance) 
        {
            Destroy(this);
        }
        else 
        {
            instance = this;
            weaponDB = new Database("WeaponDatabase");
            weaponIcons = Resources.LoadAll<Sprite>("UI/weapon1");
            for(int i=0;i<weaponDB.database.Count;i++)
            {
                GameObject prefab = Resources.Load<GameObject>(weaponDB.database.keys[i]);
                GameObject go = GameObject.Instantiate(prefab);
                go.transform.parent = GameManager.Instance().transform;
                WeaponData wd = go.AddComponent<WeaponData>();
                wd.ATK = new Damage();
                wd.ATK.physical = weaponDB.database[i]["ATK"]["physical"].f;
                wd.ATK.magical = weaponDB.database[i]["ATK"]["magical"].f;
                wd.ATK.thunder = weaponDB.database[i]["ATK"]["thunder"].f;
                wd.ATK.fire = weaponDB.database[i]["ATK"]["fire"].f;
                wd.ATK.dark = weaponDB.database[i]["ATK"]["dark"].f;

                wd.bounusLv = new BaseStates();
                wd.bounusLv.strength = (int)weaponDB.database[i]["Bonus"]["strength"].i;
                wd.bounusLv.dexterity = (int)weaponDB.database[i]["Bonus"]["dexterity"].i;
                wd.bounusLv.intelligence = (int)weaponDB.database[i]["Bonus"]["intelligence"].i;

                wd.wpAtkMotionID = (WpAtkMotionID)weaponDB.database[i]["AtkMotionID"].i;
                wd.localMotionID1H = (int)weaponDB.database[i]["LocalMotionID1h"].i;
                wd.localMotionID2H = (int)weaponDB.database[i]["LocalMotionID2h"].i;
                wd.iconId = (int)weaponDB.database[i]["iconID"].i;
                wd.icon = weaponIcons[wd.iconId];
                weapons.Add(weaponDB.database.keys[i],go);
            } 
        }
    } 

    public static void SetWeaponData(WeaponData wd,string weaponName)
    {
        wd.ATK = new Damage();
        wd.ATK.physical = weaponDB.database[weaponName]["ATK"]["physical"].f;
        wd.ATK.magical = weaponDB.database[weaponName]["ATK"]["magical"].f;
        wd.ATK.thunder = weaponDB.database[weaponName]["ATK"]["thunder"].f;
        wd.ATK.fire = weaponDB.database[weaponName]["ATK"]["fire"].f;
        wd.ATK.dark = weaponDB.database[weaponName]["ATK"]["dark"].f;

        wd.iconId = (int)weaponDB.database[weaponName]["iconID"].i;
        wd.icon = weaponIcons[wd.iconId];
        wd.wpAtkMotionID = (WpAtkMotionID)weaponDB.database[weaponName]["AtkMotionID"].i;
        
        wd.bounusLv = new BaseStates();
        wd.bounusLv.strength = (int)weaponDB.database[weaponName]["Bonus"]["strength"].i;
        wd.bounusLv.dexterity = (int)weaponDB.database[weaponName]["Bonus"]["dexterity"].i;
        wd.bounusLv.intelligence = (int)weaponDB.database[weaponName]["Bonus"]["intelligence"].i;
        wd.localMotionID1H = (int)weaponDB.database[weaponName]["LocalMotionID1h"].i;
        wd.localMotionID2H = (int)weaponDB.database[weaponName]["LocalMotionID2h"].i;
    }

    public static void CreateWeapon(string wpName,Transform parent)
    {
        GameObject weapon = weapons[wpName];
        weapon.name = wpName;
        weapon = Instantiate(weapon);
        weapon.transform.parent = parent;
    }
}
