using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponController : MonoBehaviour
{
    public WeaponManager wm;
    public List<WeaponData> weaponDataList;
    public WeaponData weaponDataOnUse;
    private readonly int weaponCount = 3;
    public Weapon[] Weapons = new Weapon[3];
    private void Awake() 
    {
        weaponDataList = new List<WeaponData>(weaponCount);
    }

    private void Start() 
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform t = transform.GetChild(i);
            WeaponData wd = t.GetComponent<WeaponData>();
            wd.battleManager = wm.am.bm;
            weaponDataList.Add(wd);
            Weapons[i] = wd.weapon;
        }
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
    /// 装备武器
    /// </summary>
    /// <param name="weapon"></param>
    /// <param name="pos"></param>
    public void EquipWeapon(Weapon weapon, int pos)
    {
        bool isOnUse = weaponDataList[pos] == weaponDataOnUse;
        UnEquipWeapon(pos);
        weaponDataList[pos].m_Weapon = weapon;
        weaponDataList[pos].gameObject.SetActive(isOnUse);
        Weapons[pos] = weapon;
    }

    /// <summary>
    /// 卸载武器
    /// </summary>
    /// <param name="pos"></param>
    public void UnEquipWeapon(int pos)
    {
        weaponDataList[pos].m_Weapon = null;
        Weapons[pos] = null;
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
    /// 获得下一个武器的索引
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
