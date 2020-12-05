using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum WpAtkMotionID{
    sword = 23,
    GreatSword = 25,
    Spear = 36,
    Fist = 42,
    Bow = 44,
    CrossBow = 45,
    Shield = 48,
    Arrow = 1,
}
//"AtkMotionID": 25,
//"SpAtkMotionID": 25,
//"LocalMotionID1h": 2,3
//"LocalMotionID2h": 15,


public class WeaponData : MonoBehaviour
{
    public Damage ATK;
    public BaseStates bounusLv;
    public int iconId = 0;
    public Sprite icon;
    public WpAtkMotionID wpAtkMotionID;
    public int localMotionID1H;
    public int localMotionID2H;
    public BattleManager battleManager;
    public Action<int,Vector3,Vector3> HitSurfaceEvent;
    private void Awake() {
        HitSurfaceEvent = InitialSparkOnSurface;

    }
    public static bool IsShield(WeaponData wd)
    {
        return wd.wpAtkMotionID == WpAtkMotionID.Shield;
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(battleManager == null) return;
        GameObject go = other.gameObject;
        Vector3 hitPoint = other.ClosestPoint(transform.position);
        Vector3 normal = hitPoint - transform.position;
        
        battleManager.TryDoDmage(go, hitPoint, normal, wpAtkMotionID == WpAtkMotionID.Arrow);
        HitSurfaceEvent.Invoke(other.gameObject.layer,hitPoint,normal);
    }

    public void InitialSparkOnSurface(int hitLayer,Vector3 hitPoint,Vector3 normal)
    {
        //只在墙面和盾牌生成火花
        if(hitLayer != 0) return;
        GameObject spark = ObjectPool.instance.GetObject("Spark");
        spark.transform.position = hitPoint;
        spark.transform.forward = normal != Vector3.zero ? normal : spark.transform.forward;
        spark.GetComponent<ParticleSystem>().Play();
    }
}


