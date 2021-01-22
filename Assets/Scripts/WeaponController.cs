using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponController : MonoBehaviour
{
    public WeaponManager wm;
    public List<WeaponData> weaponDataList;
    public WeaponData weaponDataOnUse;
    private int weaponCount = 3;
    private void Awake() 
    {
        weaponDataList = new List<WeaponData>(weaponCount);
    }

    private void Start() 
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform t = transform.GetChild(i);
            weaponDataList.Add(t.GetComponent<WeaponData>());
        }
        
        FillFists();
        LoadWeapon();
        HideWeaponOnUnuse();
    }
    
    /// <summary>
    /// 读取上次存储的武器
    /// </summary>
    private void LoadWeapon()
    {
        weaponDataOnUse = weaponDataOnUse == null ? weaponDataList[0] : weaponDataOnUse;
    }
    
    /// <summary>
    /// 隐藏未使用的武器
    /// </summary>
    private void HideWeaponOnUnuse()
    {
        for (int i = 0; i < weaponDataList.Count; i++)
        {
            if (weaponDataList[i] != weaponDataOnUse)
            {
                SetWeaponVisiable(weaponDataList[i].gameObject, false);
            }
        }
    }

    /// <summary>
    /// 为空武器槽填充拳头
    /// </summary>
    private void FillFists()
    {
        int blankSlotCount = weaponCount - transform.childCount;
        for (int i = 0; i < blankSlotCount ; i++)
        {
            GameObject go = new GameObject();
            go.name = "Fist";
            go.transform.parent = transform;
            WeaponData wd = go.AddComponent<WeaponData>();
            wd.weapon = (Weapon) GameDatabase.GetInstance().GetItem(1000);
            weaponDataList.Add(wd);
        }
    }
    
    /// <summary>
    /// 切换武器
    /// </summary>
    /// <returns>武器数据</returns>
    public WeaponData GetNextWeapon()
    {
        if(weaponDataOnUse)
        {
            int next_weapon_index = ( weaponDataList.IndexOf(weaponDataOnUse) + 1 ) % weaponDataList.Count;
            return weaponDataList[next_weapon_index];
        }
        return null;
    }
    
    /// <summary>
    /// 隐藏武器
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="value">true 左手武器 flase </param>
    public void SetWeaponVisiable(GameObject obj,bool value)
    {
        obj.SetActive(value);
    }

    public List<Item> GetWeapons()
    {
        List<Item> weapon = new List<Item>();
        for (int i = 0; i < weaponDataList.Capacity; i++)
        {
            weapon.Add(weaponDataList[i].weapon);
        }

        return weapon;
    }
}
