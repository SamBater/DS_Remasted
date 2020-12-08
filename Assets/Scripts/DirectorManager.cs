using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[RequireComponent(typeof(PlayableDirector))]
public class DirectorManager : IActorManagerInterface
{
    [Header("Timeline assets")]
    public PlayableDirector pd;
    public TimelineAsset frontStab;
    public TimelineAsset openBox;
    public TimelineAsset leverUp;

    [Header("Assets")]
    public ActorManager attacker;
    public ActorManager victim;
    void Start()
    {
        pd = GetComponent<PlayableDirector>();
        pd.playOnAwake = false;
    }

    public bool IsPlaying()
    {
        return pd.state == PlayState.Playing;
    }
    
    public void PlayTimeLine(string timelineName,ActorManager attacker,ActorManager victim)
    {
        if (IsPlaying()) return;
        if(timelineName == "frontStab")
        {
            pd.playableAsset = Instantiate(frontStab);
            TimelineAsset timeline = (TimelineAsset)pd.playableAsset;

            foreach(var track in timeline.GetOutputTracks())
            {
                //Debug.Log(track.name);
                if(track.name == "Attack Animation")
                {
                    pd.SetGenericBinding(track,attacker.ac.animator);
                }
                else if(track.name == "Victim Animation")
                {
                    pd.SetGenericBinding(track,victim.ac.animator);
                }
                else if(track.name == "Attack Script")
                {
                    pd.SetGenericBinding(track,attacker);
                    foreach(var clip in track.GetClips())
                    {
                        MyPlayableClip myclip = (MyPlayableClip)clip.asset;
                        MyPlayableBehaviour mybehav = myclip.template;
                        myclip.am.exposedName = System.Guid.NewGuid().ToString();
                        pd.SetReferenceValue(myclip.am.exposedName,attacker);
                    }
                }
                else if(track.name == "Victim Script")
                {
                    pd.SetGenericBinding(track,victim);
                    foreach(var clip in track.GetClips())
                    {
                        MyPlayableClip myclip = (MyPlayableClip)clip.asset;
                        MyPlayableBehaviour mybehav = myclip.template;
                        myclip.am.exposedName = System.Guid.NewGuid().ToString();
                        pd.SetReferenceValue(myclip.am.exposedName,victim);
                    }
                }
            }

        }

        else if(timelineName == "openBox")
        {
            Debug.Log("openBox");
            pd.playableAsset = Instantiate(openBox);
            TimelineAsset timeline = (TimelineAsset)pd.playableAsset;

            foreach(var track in timeline.GetOutputTracks())
            {
                //Debug.Log(track.name);
                if(track.name == "Player Animation")
                {
                    pd.SetGenericBinding(track,attacker.ac.animator);
                }
                else if(track.name == "Box Animation")
                {
                    pd.SetGenericBinding(track,victim.ac.animator);
                }
                else if(track.name == "Player Script")
                {
                    pd.SetGenericBinding(track,attacker);
                    foreach(var clip in track.GetClips())
                    {
                        MyPlayableClip myclip = (MyPlayableClip)clip.asset;
                        MyPlayableBehaviour mybehav = myclip.template;
                        myclip.am.exposedName = System.Guid.NewGuid().ToString();
                        pd.SetReferenceValue(myclip.am.exposedName,attacker);
                    }
                }
                else if(track.name == "Box Script")
                {
                    pd.SetGenericBinding(track,victim);
                    foreach(var clip in track.GetClips())
                    {
                        MyPlayableClip myclip = (MyPlayableClip)clip.asset;
                        MyPlayableBehaviour mybehav = myclip.template;
                        myclip.am.exposedName = System.Guid.NewGuid().ToString();
                        pd.SetReferenceValue(myclip.am.exposedName,victim);
                    }
                }
            }
        }
        else if(timelineName == "leverUp")
        {
            Debug.Log("try to play leverUp");
            pd.playableAsset = Instantiate(leverUp);
            TimelineAsset timeline = (TimelineAsset)pd.playableAsset;

            foreach(var track in timeline.GetOutputTracks())
            {
                //Debug.Log(track.name);
                if(track.name == "Player Animation")
                {
                    pd.SetGenericBinding(track,attacker.ac.animator);
                }
                else if(track.name == "Lever Animation")
                {
                    pd.SetGenericBinding(track,victim.ac.animator);
                }
                else if(track.name == "Player Script")
                {
                    pd.SetGenericBinding(track,attacker);
                    foreach(var clip in track.GetClips())
                    {
                        MyPlayableClip myclip = (MyPlayableClip)clip.asset;
                        MyPlayableBehaviour mybehav = myclip.template;
                        myclip.am.exposedName = System.Guid.NewGuid().ToString();
                        pd.SetReferenceValue(myclip.am.exposedName,attacker);
                    }
                }
                else if(track.name == "Lever Script")
                {
                    pd.SetGenericBinding(track,victim);
                    foreach(var clip in track.GetClips())
                    {
                        MyPlayableClip myclip = (MyPlayableClip)clip.asset;
                        MyPlayableBehaviour mybehav = myclip.template;
                        myclip.am.exposedName = System.Guid.NewGuid().ToString();
                        pd.SetReferenceValue(myclip.am.exposedName,victim);
                    }
                }
            }
        }

        pd.Evaluate();
        pd.Play();
    }
}
