using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtoriasControllor : ActorController
{    
    private void OnBecameInvisible() {
        isOnScreen = false;
    }
    private void OnBecameVisible() {
        isOnScreen = true;
    }
    private void Update() {
        if (cc.cameraStatus == CameraStatus.FOLLOW || cc.cameraStatus == CameraStatus.AIM)
        {
            HandleMoveOnNormal();
        }

        else if (cc.cameraStatus == CameraStatus.LOCKON)
        {
            HandleMoveWhenLockOn();
        }
        if (playerInput.enableInput == false) return;
        WolfAttackx3();
        WolfxRollBack();
        
    }
    private void FixedUpdate() 
    {
        PhysicalMove();
    }
    
    //拼命三狼
    void WolfAttackx3()
    {
        animator.SetTrigger("wolfx3");
    }

    void WolfxRollBack()
    {
        animator.SetTrigger("wolfBack");
    }
}
