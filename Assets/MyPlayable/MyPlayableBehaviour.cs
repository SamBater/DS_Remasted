using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class MyPlayableBehaviour : PlayableBehaviour
{

    public ActorManager am;
    PlayableDirector pd;
    public override void OnPlayableCreate (Playable playable)
    {
        
    }

    public override void OnBehaviourPlay(Playable playable,FrameData info)
    {
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        if (am.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            am.wm.SetAllWeaponOnUseVisiable(true);
        }
        am.ToggleLock(false);
    }

    public override void PrepareFrame(Playable playable, FrameData info)
    {
        am.ToggleLock(true);
    }
}
