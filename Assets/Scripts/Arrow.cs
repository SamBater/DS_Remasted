using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody rbd;
    public float v;
    public float maxFlyDistance;
    public float maxTime = 5.0f;
    public float curTime;
    public Vector3 origin;
    public bool flyable = true;
    public float rotationChangeSpeed = 1.5f;
    WeaponData wd;
    Collider col;
    ParticleSystem trail;
    private void Awake()
    {
        origin = transform.position;
        wd = gameObject.AddComponent<WeaponData>();
        rbd = GetComponent<Rigidbody>();
        WeaponFactory.SetWeaponData(wd,"Arrow_stick");
        wd.hitSurfaceEvent += ReadyToHide;
        col = GetComponent<Collider>();
        trail = GetComponentInChildren<ParticleSystem>();
    }

    private void Start() {
        
    }

    private void OnEnable() 
    {
        col.enabled = true;
    }
    private void Update()
    {
        curTime += Time.deltaTime;
        float distanceNow = Vector3.Distance(origin,transform.position);
        transform.Translate(Vector3.forward * v * Time.deltaTime);
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

    private void OnTriggerEnter(Collider other) 
    {
        
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
