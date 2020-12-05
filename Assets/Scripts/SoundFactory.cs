using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFactory : MonoBehaviour
{
    public List<AudioClip> a23attack = new List<AudioClip>();
    public List<AudioClip> shield = new List<AudioClip>();
    public  AudioClip counter_succesful;
    public  AudioClip heal;
    public AudioClip Bow_shoot;
    public AudioClip pickUp;
    public static SoundFactory instance;

    private void Awake() {
        if(instance == null)
            instance = this;
        else
            Destroy(this);
    }

}
