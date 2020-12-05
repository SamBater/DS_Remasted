using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseOffFatherCol : MonoBehaviour
{
    [SerializeField]
    private GameObject father;
    void Awake()
    {
        father = transform.parent.gameObject;
    }

    private void OnCollisionEnter(Collision other) {
        father.SetActive(false);
    }
}
