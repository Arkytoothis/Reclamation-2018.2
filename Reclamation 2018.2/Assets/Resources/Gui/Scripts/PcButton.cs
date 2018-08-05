using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Reclamation.Misc;
using Reclamation.Characters;
using Reclamation.Encounter;
using Pathfinding;
using Pathfinding.RVO;

namespace Reclamation.Gui
{
    public class PcButton : GuiElement
    {
        public PcButtonData skinData;
        public Image background;
        public UnityEngine.UI.Button button;

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
                nameLabel.text = pc.Name.FirstName;

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

                GameObject model = ModelManager.instance.CharacterPrefabs[pc.RaceKey + " " + pc.Gender];

                PortraitRoom.instance.AddModel(model, pc.EncounterIndex);
                portrait.texture = PortraitRoom.instance.characterMounts[pc.EncounterIndex].rtCamera.targetTexture;
            }
            else
            {
                nameLabel.text = "";
                portrait.texture = null;
            }
        }
    }
}