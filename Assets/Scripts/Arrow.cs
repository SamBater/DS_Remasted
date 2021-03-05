using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody rbd;
    public float maxTime = 5.0f;
    public float curTime;
    public Vector3 origin;
    public float speed = 25.0f;
    WeaponData wd;
    Collider col;
    ParticleSystem trail;
    private void Awake()
    {
        origin = transform.position;
        rbd = GetComponent<Rigidbody>();
//        wd.HitSurfaceEvent += ReadyToHide;
        col = GetComponent<Collider>();
        trail = GetComponentInChildren<ParticleSystem>();
    }

    private void Start() {
        
    }

    private void OnEnable() 
    {
        col.enabled = true;
        rbd.AddForce(100.0f * transform.forward);
    }
    private void Update()
    {
        curTime += Time.deltaTime;
        float distanceNow = Vector3.Distance(origin,transform.position);
        rbd.velocity = transform.forward * speed;
        //bool hit = Physics.Raycast(transform.position,transform.forward,0.5f,1);
        if(curTime >= maxTime) 
        {
            ReadyToHide();
        }
    }
    void ReadyToHide()
    {
        col.enabled = false;
        trail.Stop();
        gameObject.SetActive(false);
        curTime = 0;
    }
    
    void ReadyToHide(int a,Vector3 b,Vector3 c)
    {
        if(a==0 || a== 9)
        {
            col.enabled = false;
            trail.Stop();
            gameObject.SetActive(false);
            curTime = 0;
        }
    }

}
