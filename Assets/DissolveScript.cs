using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveScript : MonoBehaviour
{
    public SkinnedMeshRenderer[] meshRenderers;
    public Shader dissolve;
    public bool deathToggle;
    [Range(0,0.01f)]public float dissolveSpeed;
    public void ApplyShaders(Shader shader)
    {
        for(int i=0;i<meshRenderers.Length;i++)
        {
            Texture tex = meshRenderers[i].material.mainTexture;
            meshRenderers[i].material.shader = shader;
        }
    }

    public void StartDissolve()
    {
        StartCoroutine("ApplyDissolve");
    }

    IEnumerator ApplyDissolve()
    {
        ApplyShaders(dissolve);
        float clip = meshRenderers[0].material.GetFloat("_BurnAmount");
        while(clip < 1.0)
        {
            for(int i=0;i<meshRenderers.Length;i++)
            {
                clip += dissolveSpeed;
                meshRenderers[i].material.SetFloat("_BurnAmount",clip);
            }
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
}
