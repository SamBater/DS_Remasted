using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemOnGround : MonoBehaviour
{
    [SerializeField]
    public List<ItemEnum> items = new List<ItemEnum>();
    [SerializeField]
    public List<int> counts = new List<int>();
    public Material material;

    public bool isFade = false;
    private void Start() 
    {
        material = GetComponent<MeshRenderer>().material;
    }

    private void Update() 
    {
        if(isFade)
        {
            float f = material.GetFloat("Vector1_2580D4CA");
            float n = Mathf.Lerp(f,1.0f,0.02f);
            material.SetFloat("Vector1_2580D4CA",n);
        }
    }

    //当被拾取时，发生的行为.
    public void OnAction()
    {
        isFade = true;
    }
}
