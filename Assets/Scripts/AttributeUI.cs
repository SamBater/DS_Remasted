using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AttributeUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI m_name;
    public TextMeshProUGUI m_val;
    private void Awake()
    {
        
    }

    public void SetName(string name)
    {
        m_name.SetText(name);
    }

    public void SetVal(string val)
    {
        m_val.SetText(val);
    }
}
