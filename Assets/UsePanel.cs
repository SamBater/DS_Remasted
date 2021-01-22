using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsePanel : MonoBehaviour
{
    public InventoryUI lhWeapon;
    public InventoryUI rhWeapon;
    public InventoryUI quickUse;
    public InventoryManager im;

    private void Update()
    {
        rhWeapon.Fresh(im.am.wm.GetWeapons(true));
        lhWeapon.Fresh(im.am.wm.GetWeapons(false));
        quickUse.Fresh(im.quickUse,im.inventory);
    }
}
