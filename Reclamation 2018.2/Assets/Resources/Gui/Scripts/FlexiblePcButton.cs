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

        private Pc pc;

        protected override void OnSkinGui()
        {
            base.OnSkinGui();

            background.sprite = skinData.background;
            background.type = Image.Type.Sliced;

            button.transition = Selectable.Transition.ColorTint;
            button.colors = skinData.buttonColors;
        }

        public void SetData(Pc pc)
        {
            if (pc != null)
            {
                this.pc = pc;
                GameObject go = null;
                AttributeBar bar = null;
                nameLabel.text = this.pc.Name.FirstName;

                for (int i = 0; i < (int)Vital.Number; i++)
                {
                    go = Instantiate(attributeBarPrefab, attributeBarsParent);
                    bar = go.GetComponent<AttributeBar>();
                    bar.SetData(this.pc, i);

                    attributeBars.Add(go);
                }

                go = Instantiate(attributeBarPrefab, attributeBarsParent);
                bar = go.GetComponent<AttributeBar>();
                bar.SetExpData(this.pc);
                expBar = go;

                PortraitRoom.instance.AddModel(ModelManager.instance.SpawnPc(null, Vector3.zero, this.pc), this.pc.EncounterIndex);
                portrait.texture = PortraitRoom.instance.characterMounts[this.pc.EncounterIndex].rtCamera.targetTexture;
            }
            else
            {
                nameLabel.text = "";
                portrait.texture = null;
            }
        }
    }
}