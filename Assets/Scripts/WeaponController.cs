using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponController : MonoBehaviour
{
    public WeaponManager wm;
    public List<WeaponData> weaponDataList;
    public WeaponData weaponDataOnUse;
    public WeaponItem[] Weapons = new WeaponItem[3];
    private void Awake() 
    {
        
    }

    private void Start() 
    {
        LoadWeapon();
        HideWeaponOnUnuse();
    }
    
    /// <summary>
    /// 读取上次存储的武器
    /// </summary>
    private void LoadWeapon()
    {   
        //根据在inspert中的数据适配模型
        for (int i = 0; i < Weapons.Length; i++)
        {
            if(transform.childCount > 0 ) break;
            GameObject weapon = Instantiate<GameObject>(Weapons[i].model,transform);
            WeaponData wd = weapon.GetComponent<WeaponData>();
            wd.battleManager = wm.am.bm;
            weaponDataList.Add(wd);
        }
        weaponDataList.Add(transform.GetChild(0).gameObject.GetComponent<WeaponData>());
        weaponDataOnUse = weaponDataOnUse == null ? weaponDataList[0] : weaponDataOnUse;
    }
    
    /// <summary>
    /// 装备武器
    /// </summary>
    /// <param name="weaponItem"></param>
    /// <param name="pos"></param>
    public void EquipWeapon(WeaponItem weaponItem, int pos)
    {
        bool isOnUse = weaponDataList[pos] == weaponDataOnUse;
        Destroy(weaponDataList[pos].gameObject);
        GameObject model = Instantiate<GameObject>(weaponItem.model, transform);
        model.transform.SetSiblingIndex(pos);
        WeaponData wd = model.GetComponent<WeaponData>();
        wd.battleManager = wm.am.bm;
        if (isOnUse)
            weaponDataOnUse = wd;
        weaponDataList[pos] = wd;
        weaponDataList[pos].gameObject.SetActive(isOnUse);
        Weapons[pos] = weaponItem;
    }

    /// <summary>
    /// 卸载武器
    /// </summary>
    /// <param name="pos"></param>
    public void UnEquipWeapon(int pos)
    {
        Destroy(weaponDataList[pos].gameObject);
        WeaponItem fist = (WeaponItem)WeaponItem.GetItem(ItemEnum.Fist);
        Weapons[pos] = fist;
        GameObject fistObject = Instantiate<GameObject>(fist.model, transform);
        fistObject.transform.SetSiblingIndex(pos);
        weaponDataList[pos] = fistObject.GetComponent<WeaponData>();
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

    public Item[] GetWeapons()
    {
        return Weapons;
    }
}
