using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAddDynamic : MonoBehaviour
{
    Transform rightHand;
    public GameObject go;
    void Start()
    {
        rightHand = GetComponent<ActorController>().animator.GetBoneTransform(HumanBodyBones.RightHand);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            go.transform.parent = rightHand;
            go.transform.localPosition = Vector3.zero;
        }
    }
}
