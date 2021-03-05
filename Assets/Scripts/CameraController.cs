using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CameraStatus{FREE,LOCKON,FOLLOW,AIM};

public class CameraController : MonoBehaviour
{
    public Transform follow;//跟随目标
    public float xMove = 0; //鼠标横向偏移量
    public float yMove = 0; //鼠标垂直偏移量

    public float max_ymove = 120;
    public float zoomValue = 2; //摄像机离目标的距离
    public float height = 1.5f;//摄像机高度
    public float offset_x = 0;
    [Tooltip("锁定时相机旋转速度")]
    public float trackTargetSpeed = 2.0f;
    public CameraStatus cameraStatus = CameraStatus.FOLLOW;
    public float maxLockDistance = 8.0f;
    public float maxLockWidth = 3.5f;
    public LockTarget lockOnTarget = null;
    public Image lockDot;
    public bool lockOn = false;
    public LayerMask whatISEnemy;

    public float minVerticalMove = -30f;
    public float maxVerticalMove = 60.0f;

    public bool fade = false;
    public float fadeValue;
    public Material material;
    private void Awake() 
    {
        lockDot.enabled = false;

        if(follow)
        transform.forward = follow.forward;
        xMove = 0;
        yMove = 45.0f;


    }

    private void Update()
    {
        if (fade && material)
        {
            fadeValue = Mathf.Lerp(fadeValue, 1.0f, Time.deltaTime * 1.0f);
            material.SetFloat("_Speed", fadeValue);
        }
        float xInput = Input.GetAxis("Mouse X");
        float YInput = -1.0f * Input.GetAxis("Mouse Y");

        YInput = Mathf.Clamp(YInput,minVerticalMove,maxVerticalMove);
        
        yMove += YInput;
        xMove += xInput;
        if(cameraStatus == CameraStatus.FOLLOW)
        {
            if(Mathf.Abs(yMove) > 0.01f || Mathf.Abs(xMove) > 0.01f)
            transform.rotation = Quaternion.Euler(yMove, xMove, 0);
            //TODO:射线判断 是否碰到障碍物 是则拉近 否则拉远
        }

        else if(cameraStatus == CameraStatus.LOCKON)
        {
            //CameraOnLock();
        }

        else if(cameraStatus == CameraStatus.AIM)
        {
            if(Mathf.Abs(yMove) > 0.2f && Mathf.Abs(xMove) > 0.2f)
            transform.rotation = Quaternion.Euler(yMove, xMove, 0);
        }
        
    }
 
    private void LateUpdate()//写在这防抖动
    {
        transform.position = transform.rotation *  new Vector3(offset_x, 0.0f, -zoomValue);
        transform.position += (follow.position + Vector3.up * height);
        if(cameraStatus == CameraStatus.LOCKON)
        {
            CameraOnLock();
        }
    }

    public void LockOnToggle()
    {
        if(lockOnTarget == null)
        {
            Collider[] cols = Physics.OverlapSphere(follow.transform.position, maxLockDistance, whatISEnemy);

            //find the most close target.
            Collider nearestCol = null;
            float nearestDistance = float.MaxValue;
            foreach (var obj in cols)
            {
                ActorManager am = obj.gameObject.GetComponent<ActorManager>();
                if(am && am.sm.isDead) continue;
                float dis = Vector3.Distance(obj.gameObject.transform.position,transform.position);
                if(dis < nearestDistance)
                {
                    nearestDistance = dis;
                    nearestCol = obj;
                    lockOn = true;
                }
            }

            //change status
            if (nearestCol)
            {
                cameraStatus = CameraStatus.LOCKON;
                lockOnTarget = new LockTarget(nearestCol.gameObject.transform, nearestCol.bounds.extents.y);
                lockDot.enabled = true;
            }
            else
            {
                cameraStatus = CameraStatus.FOLLOW;
            }
        }
        else
        {
            LockOff();
        }
    }

    public void LockOff()
    {
        //退出时保持视角
        xMove = transform.rotation.eulerAngles.y;
        yMove = transform.rotation.eulerAngles.x;

        //取消锁定
        lockOnTarget = null;
        cameraStatus = CameraStatus.FOLLOW;
        lockDot.enabled = false;
    }

    public void CameraOnLock()
    {
        if(lockOnTarget != null)
        {
            //相机跟随速度 TODO:修改为模型位置 但是Y轴貌似没有bake进来...
            //TODO:抖动问题.暂时通过增加物理补偿解决. 尝试垂直同步 和帧率设置.
            Quaternion direction = Quaternion.LookRotation(lockOnTarget.obj.position + lockOnTarget.halfHeight * Vector3.up * 0.6f - transform.position);
            direction.z = direction.x = 0.0f;
            transform.rotation = Quaternion.Lerp(transform.rotation,direction,trackTargetSpeed * Time.fixedDeltaTime);

            Vector3 forward = transform.forward;
            forward.y = 0.0f;
            
            //follow.forward = Vector3.Lerp(follow.forward,forward,Time.deltaTime * 12.0f);
            follow.forward = forward;

            //调整lockDot位置
            Vector3 dotPos = Vector3.up;
            dotPos = lockOnTarget.am.ac.neckPos.position;
            lockDot.rectTransform.position = Camera.main.WorldToScreenPoint(dotPos);

            float distance = Vector3.Distance(lockOnTarget.obj.position,follow.position);
            

            //TODO:超出屏幕自动解锁

            //超出一定范围自动解锁
            if(distance > maxLockDistance*2.5f)
                LockOff();

            //如果目标已经死亡
            if(lockOnTarget != null && lockOnTarget.am && lockOnTarget.am.sm.isDead)
            {
                LockOff();
            }
        }
        
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (material && fade)
        {
            Graphics.Blit(src, dest, material);
        }
        else
            Graphics.Blit(src, dest);
    }

    public class LockTarget
    {
        public Transform obj;
        public float halfHeight;
        public ActorManager am;

        public LockTarget(Transform _obj,float half_h)
        {
            obj = _obj;
            halfHeight = half_h;
            am = obj.gameObject.GetComponent<ActorManager>();
        }
    }
}
