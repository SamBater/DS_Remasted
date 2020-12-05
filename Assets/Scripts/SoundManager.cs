using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[RequireComponent(typeof(AudioSource))]
public class SoundManager : IActorManagerInterface
{
    public AudioSource audioSource;
    public List<AudioClip> damage = new List<AudioClip>();
    public List<AudioClip> foot = new List<AudioClip>();
    public AudioClip death;

    private void Start() 
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.spatialBlend = 1.0f;
        audioSource.volume = 0.25f;
        
    }

    private void Update() {
        if(am.sm.isWalk)
        {
            Play(foot,true);
            audioSource.pitch = Mathf.Clamp(Mathf.Abs(am.ac.animator.GetFloat("forward")),0.0f,1.5f);
            audioSource.volume = .1f;
        }
        else
        {
            audioSource.pitch = 1.0f;
        }
    }

    public void OnHit()
    {
        Play(damage);
    }

    public void OnFoot()
    {
        Play(foot);
    }

    public void OnDead()
    {
        if(death)
        {
            Play(death);
        }
    }

    //供Event使用
    public void Counter()
    {
        Play(SoundFactory.instance.counter_succesful);
    }

    public void Drink()
    {
        Play(SoundFactory.instance.heal);
    }

    void Play(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Stop();
        audioSource.Play();
    }

    void Play(List<AudioClip> clips,bool waitForPreDone = false)
    {
        if(clips.Count == 0) Debug.LogError("Audioclips is empty.");
        if(!waitForPreDone)
        {
            Play(clips[Random.Range(0,clips.Count-1)]);
        }
        else if(!audioSource.isPlaying)
        {
            Play(clips[Random.Range(0,clips.Count-1)]);
        }
    }

    //供Event使用
    void Play(int id)
    {
                        Debug.Log("this should be shown!");
        switch(id)
        {
            case 23:
                Play(SoundFactory.instance.a23attack);

                break;

           // default:
                //Debug.LogError("Sound does't exists.check the animation event id is whether correct.");
        }

    }

    void Playa23()
    {
        Play(SoundFactory.instance.a23attack);
    }

    public void PickUp()
    {
        Play(SoundFactory.instance.pickUp);
    }

    public void PlayShield()
    {
        Play(SoundFactory.instance.shield);
    }

    public void PlayGunShoot()
    {
        Play(SoundFactory.instance.Bow_shoot);
    }
}
