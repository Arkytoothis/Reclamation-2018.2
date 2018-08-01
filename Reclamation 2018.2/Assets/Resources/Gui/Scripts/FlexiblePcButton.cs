using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Reclamation.Misc;
using Reclamation.Characters;
using Reclamation.Encounter;

namespace Reclamation.Gui
{
    public class FlexiblePcButton : FlexibleGui
    {
        public PcButtonData skinData;
        public Image background;
        public Button button;

        public TMP_Text nameLabel;
        public RawImage portrait;

        public GameObject attributeBarPrefab;
        public List<GameObject> attributeBars;
        public Transform attributeBarsParent;

        public GameObject expBar;

        //private PcData pc;

        protected override void OnSkinGui()
        {
            base.OnSkinGui();

            background.sprite = skinData.background;
            background.type = Image.Type.Sliced;

            button.transition = Selectable.Transition.ColorTint;
            button.colors = skinData.buttonColors;
        }

        public void SetData(PcData pc)
        {
            if (pc != null)
            {
                //this.pc = pc;
                GameObject go = null;
                AttributeBar bar = null;
                nameLabel.text = pc.name.FirstName;

                for (int i = 0; i < (int)Vital.Number; i++)
                {
                    go = Instantiate(attributeBarPrefab, attributeBarsParent);
                    bar = go.GetComponent<AttributeBar>();
                    bar.SetData(pc, i);

                    attributeBars.Add(go);
                }

                go = Instantiate(attributeBarPrefab, attributeBarsParent);
                bar = go.GetComponent<AttributeBar>();
                bar.SetExpData(pc);
                expBar = go;

                PortraitRoom.instance.AddModel(ModelManager.instance.SpawnCharacter(null, Vector3.zero, pc), pc.encounterIndex);
                portrait.texture = PortraitRoom.instance.characterMounts[pc.encounterIndex].rtCamera.targetTexture;
            }
            else
            {
                nameLabel.text = "";
                portrait.texture = null;
            }
        }
    }
}