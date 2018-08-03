using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Reclamation.Gui;
using FoW;
using Pathfinding;
using Reclamation.Encounter;
using Reclamation.Characters;

namespace Reclamation.Misc
{
    public class PortraitRoom : Singleton<PortraitRoom>
    {
        public PortraitMount[] characterMounts;

        void Awake()
        {
            Reload();
        }

        public void AddModel(GameObject model, int index)
        {
            //model.GetComponent<FogOfWarUnit>().enabled = false;
            //model.GetComponent<Seeker>().enabled = false;
            ////model.GetComponent<SimpleSmoothModifier>().enabled = false;
            //model.GetComponent<PcController>().enabled = false;
            //model.GetComponent<AIDestinationSetter>().enabled = false;
            //model.GetComponent<RichAI>().enabled = false;

            GameObject go = Instantiate(model, characterMounts[index].pivot);
            go.name = model.name;
        }

        //public RenderTexture CreatePortrait(GameObject go)
        //{
        //    rt = new RenderTexture(64, 64, 32, RenderTextureFormat.ARGB32);
        //    rt.Create();

        //    return rt;
        //}
    }
}