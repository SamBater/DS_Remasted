using System.Collections;
using System.Collections.Generic;
using UnityEngine;


interface IUseAble
{
    void Use(ActorManager am);
}

public class Consumable : Item,IUseAble
{
    public override void LoadData(string[] col)
    {
        base.LoadData(col);
        itemType = ItemType.Consumable;
    }

    public virtual void Use(ActorManager am)
    {
        throw new System.NotImplementedException();
    }
}
