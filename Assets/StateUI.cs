using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StateUI : MonoBehaviour
{
    public StateManager sm;
    public AttributeUI hp;
    public AttributeUI nl;
    public AttributeUI rhAtk;
    public AttributeUI lhAtk;


    private void Start()
    {
        hp.SetName("Health");
        nl.SetName("Endurance");
        rhAtk.SetName("RHAtk");
        lhAtk.SetName("LHAtk");
    }

    private void Update()
    {
        hp.SetVal($"{sm.hp}/{sm.maxhp}");
        nl.SetVal($"{sm.Naili}/{sm.Naili}");
        rhAtk.SetVal(sm.rhATK.ToString());
        lhAtk.SetVal(sm.lhATK.ToString());
    }
}
