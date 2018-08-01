using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Reclamation.Characters;
using Reclamation.Misc;

namespace Reclamation.Gui
{
    public class WorldSpaceAttributeBar : MonoBehaviour
    {
        public PcButtonData skinData;
        public Image background;
        public Image foreground;

        private CharacterData character;

        void Update()
        {
            transform.LookAt(Camera.main.transform);
        }

        public void SetData(ref NpcData character)
        {
            this.character = character;
            character.onHealthChange += UpdateDisplay;


            UpdateDisplay(this.character.GetDerived((int)DerivedAttribute.Health).Current / 2, this.character.GetDerived((int)DerivedAttribute.Health).Maximum);
        }

        public void UpdateDisplay(int current, int max)
        {
            if (max > 0)
            {
                float width = (float)current / (float)max;

                foreground.GetComponent<RectTransform>().localScale = new Vector3(width, foreground.GetComponent<RectTransform>().localScale.y, foreground.GetComponent<RectTransform>().localScale.z);
            }
        }
    }
}