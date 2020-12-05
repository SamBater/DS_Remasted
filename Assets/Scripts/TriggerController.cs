using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerController : MonoBehaviour
{
    private Animator animator;

    private void Start() {
        animator = GetComponent<Animator>();
    }
    public void ResetTrigger(string name)
    {
        animator.ResetTrigger("attack");
    }

    public void ResetAttack()
    {
        animator.ResetTrigger("attack");
        animator.ResetTrigger("Hattack");
    }
}
