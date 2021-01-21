using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Damage
{
    public float physical;
    public float magical;
    public float fire;
    public float dark;
    public float thunder;

    public Damage()
    {
        
    }

    public static Damage ComputerFinalDamage(Damage ATK,Damage DEF)
    {
        Damage damage = new Damage();
        damage.physical = ATK.physical * (1-DEF.physical);
        damage.magical = ATK.magical * (1-DEF.magical);
        damage.fire = ATK.fire * (1-DEF.fire);
        damage.dark = ATK.dark * (1-DEF.dark);
        damage.thunder = ATK.thunder * (1-DEF.thunder);
        return damage;
    }

    public static Damage operator*(Damage a , float b)
    {
        Damage damage = new Damage();
        damage.physical = a.physical * b;
        damage.magical = a.magical * b;
        damage.fire = a.fire * b;
        damage.dark = a.dark * b;
        damage.thunder = a.thunder * b;
        return damage;
    }

    public static Damage operator*(Damage a,Damage b)
    {
        Damage damage = new Damage();
        damage.physical = a.physical * b.physical;
        damage.magical = a.magical * b.magical;
        damage.fire = a.fire * b.fire;
        damage.dark = a.dark * b.dark;
        damage.thunder = a.thunder * b.thunder;
        return damage;
    }
    
    public float Total()
    {
        return physical + magical + fire + thunder + dark;
    }
}



