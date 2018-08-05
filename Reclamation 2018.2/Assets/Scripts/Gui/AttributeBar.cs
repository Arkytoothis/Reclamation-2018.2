using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Reclamation.Characters;
using Reclamation.Misc;
using TMPro;

namespace Reclamation.Gui
{
    public class AttributeBar : MonoBehaviour
    {
        public Image foreground;
        public Color color;
        public VitalData vitalData;
        public TMP_Text label;

        public void SetData(PcData pc, int attribute)
        {
            if (attribute == (int)DerivedAttribute.Armor)
            {
                pc.Attributes.onArmorChange += UpdateAttribute;
                color = vitalData.armorColor;
                UpdateAttribute(pc.GetDerived((int)DerivedAttribute.Armor).Current, pc.GetDerived((int)DerivedAttribute.Armor).Maximum);
            }
            else if (attribute == (int)DerivedAttribute.Health)
            {
                pc.Attributes.onHealthChange += UpdateAttribute;
                color = vitalData.healthColor;
                UpdateAttribute(pc.GetDerived((int)DerivedAttribute.Health).Current, pc.GetDerived((int)DerivedAttribute.Health).Maximum);
            }
            else if (attribute == (int)DerivedAttribute.Stamina)
            {
                pc.Attributes.onStaminaChange += UpdateAttribute;
                color = vitalData.staminaColor;
                UpdateAttribute(pc.GetDerived((int)DerivedAttribute.Stamina).Current, pc.GetDerived((int)DerivedAttribute.Stamina).Maximum);
            }
            else if (attribute == (int)DerivedAttribute.Essence)
            {
                pc.Attributes.onEssenceChange += UpdateAttribute;
                color = vitalData.essenceColor;
                UpdateAttribute(pc.GetDerived((int)DerivedAttribute.Essence).Current, pc.GetDerived((int)DerivedAttribute.Essence).Maximum);
            }
            else if (attribute == (int)DerivedAttribute.Morale)
            {
                pc.Attributes.onMoraleChange += UpdateAttribute;
                color = vitalData.moraleColor;
                UpdateAttribute(pc.GetDerived((int)DerivedAttribute.Morale).Current, pc.GetDerived((int)DerivedAttribute.Morale).Maximum);
            }

            foreground.color = color;

        }

        public void SetExpData(PcData pc)
        {
            pc.onExperienceChange += UpdateAttribute;
            color = vitalData.experienceColor;
            foreground.color = color;
            UpdateAttribute(pc.Experience, pc.ExpToLevel);
        }

        public void UpdateAttribute(int current, int max)
        {
            if (max != 0)
            {
                float width = (float)current / (float)max;
                label.text = current + "/" + max;
                foreground.GetComponent<RectTransform>().localScale = new Vector3(width, foreground.GetComponent<RectTransform>().localScale.y, foreground.GetComponent<RectTransform>().localScale.z);
            }
        }
    }
}