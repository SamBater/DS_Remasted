using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTest : MonoBehaviour
{
    ParticleSystem ps;
    Transform player;
    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
        player = GameObject.FindWithTag("Player").transform;
    }

    private void LateUpdate()
    {
        Vector3 target = player.gameObject.GetComponent<ActorController>().neckPos.position;
        if(Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.LookAt(target);

            transform.Translate(Vector3.forward * 2.0f *Time.deltaTime);
        }
    }
}
