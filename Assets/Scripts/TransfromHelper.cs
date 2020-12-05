using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransfromHelper
{
    public static Transform DeepFind(this Transform parent,string name)
    {
        Transform result  = null;
        foreach(Transform child in parent)
        {
            if(child.name == name) return child;
            else 
            {
                result = DeepFind(child,name);
                if(result) return result;
            }
        }
        return result;
    }
}
