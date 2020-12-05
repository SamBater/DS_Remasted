using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponController : MonoBehaviour
{
    public WeaponManager wm;

    public List<GameObject> weaponTransforms;
    public List<WeaponData> wd_list;
    public WeaponData wdOnUse;
    private void Awake() 
    {
        weaponTransforms = new List<GameObject>(3);
        wd_list = new List<WeaponData>(3);
    }

    private void Start() 
    {
        for(int i=0;i<transform.childCount;i++)
        {
            Transform child = transform.GetChild(i);
            WeaponData wd = child.gameObject.AddComponent<WeaponData>();
            WeaponFactory.SetWeaponData(wd,wd.gameObject.name);
            wd_list.Add(wd);
            wd.battleManager = wm.am.bm;    
             weaponTransforms.Add(child.gameObject);
        }
        
        //填充拳头
        for(int i=0;i<3 - transform.childCount;i++)
        {
            GameObject go = new GameObject();
            go.name = "Fist";
            go.transform.parent = transform;
            WeaponData wd = go.AddComponent<WeaponData>();
            WeaponFactory.SetWeaponData(wd,"Fist");
        }

        //TODO:读取上次退出时的武器数据,这里暂时默认第一个武器.
        
        wdOnUse = wdOnUse == null ? wd_list[0] : wdOnUse;
        
        if(wm.am.gameObject.tag == "Player")
            UIManager.instance.UpdateWeaponIcon(wdOnUse.icon,wm.wcR == this);

        //隐藏未使用的武器.
        for(int i=0;i<wd_list.Count;i++)
        {
            if(wd_list[i] != wdOnUse)
            {
                SetWeaponVisiable(wd_list[i].gameObject,false);
            }
        }
    }

    //获取正在使用的武器
    public WeaponData GetWeaponOnUse()
    {
        if(wdOnUse) return wdOnUse;
        return null;
    }

    public WeaponData GetNextWeapon()
    {
        if(wdOnUse)
        {
            int next_weapon_index = ( wd_list.IndexOf(wdOnUse) + 1 ) % wd_list.Count;
            return wd_list[next_weapon_index];
        }
        return null;
    }

    public void SetWeaponVisiable(GameObject obj,bool value)
    {
        obj.SetActive(value);
    }
}
