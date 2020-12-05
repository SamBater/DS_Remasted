using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    public CapsuleCollider capsule;
    private Vector3 topestPoint;
    private Vector3 botestPoint;
    [SerializeField]
    public float offset;
    private float radius;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        radius = capsule.radius + offset;
    }

    // Update is called once per frame
    private void FixedUpdate() 
    {
        topestPoint = transform.position + transform.up * radius;
        botestPoint = transform.position + transform.up * (capsule.height - radius);
        Collider[] cds = Physics.OverlapCapsule(topestPoint,botestPoint,radius+offset);
        if(cds.Length > 0)
            SendMessageUpwards("OnGround");
        else
            SendMessageUpwards("NotOnGround");
    }
}
