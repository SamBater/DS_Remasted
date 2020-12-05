using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXToggle : MonoBehaviour
{
    // Start is called before the first frame update
    public Collider col;
    public GameObject vfx;
    void Start()
    {
        col = transform.parent.gameObject.GetComponent<Collider>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(col.enabled)
        {
            vfx.SetActive(true);
        }
        else
        {
            vfx.SetActive(false);
        }
    }
}
