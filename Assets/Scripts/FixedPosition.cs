﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedPosition : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnAnimatorMove() {
        gameObject.SendMessageUpwards("OnRootMotionUpdate",animator.deltaPosition);
        if(animator.deltaPosition.y > 0.2f)
        print(animator.deltaPosition);
    }

}
