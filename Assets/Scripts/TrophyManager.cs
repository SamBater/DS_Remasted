using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

[Serializable]
public struct Trophy
{
    public ItemEnum itemEnum;
    [Range(1,99)]
    public int count;
    [Range(0,1)]
    public float probability;
}

public class TrophyManager : MonoBehaviour
{
    public List<Trophy> trophies;

    private void Awake()
    {
        ActorManager am = GetComponent<ActorManager>();
        am.deathHandler = Die;
    }

    public void Die()
    {
        Random r = new Random();
        ItemOnGround loot = ObjectPool.instance.GenerateLoot();
        loot.gameObject.transform.position = transform.position;
        for (int i = 0; i < trophies.Count; i++)
        {
            Trophy t = trophies[i];
            double b = r.NextDouble();
            if (t.probability > b)
            {
                loot.items.Add(t.itemEnum);
                loot.counts.Add(t.count);
            }
        }
    }
}
