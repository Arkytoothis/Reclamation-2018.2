using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortraitRoom : Singleton<PortraitRoom>
{
    public PortraitMount[] characterMounts;

    void Awake()
    {
        Reload();
    }

    public void SpawnCharacter(int index)
    {

    }

    //public RenderTexture CreatePortrait(GameObject go)
    //{
    //    rt = new RenderTexture(64, 64, 32, RenderTextureFormat.ARGB32);
    //    rt.Create();

    //    return rt;
    //}
}