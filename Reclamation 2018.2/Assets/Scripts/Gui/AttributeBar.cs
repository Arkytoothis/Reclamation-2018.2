using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Reclamation.Characters;
using Reclamation.Misc;

namespace Reclamation.Gui
{
    public class AttributeBar : MonoBehaviour
    {
        public Image foreground;
        public Color color;
        public VitalData vitalData;

        public void SetData(Pc pc, int attribute)
        {
            if (attribute == (int)DerivedAttribute.Armor)
            {
                pc.onArmorChange += UpdateAttribute;
                color = vitalData.armorColor;
            }
            else if (attribute == (int)DerivedAttribute.Health)
            {
                pc.onHealthChange += UpdateAttribute;
                color = vitalData.healthColor;
            }
            else if (attribute == (int)DerivedAttribute.Stamina)
            {
                pc.onStaminaChange += UpdateAttribute;
                color = vitalData.staminaColor;
            }
            else if (attribute == (int)DerivedAttribute.Essence)
            {
                pc.onEssenceChange += UpdateAttribute;
                color = vitalData.essenceColor;
            }
            else if (attribute == (int)DerivedAttribute.Morale)
            {
                pc.onMoraleChange += UpdateAttribute;
                color = vitalData.moraleColor;
            }

            foreground.color = color;
        }

        public void SetExpData(Pc pc)
        {
            pc.onExperienceChange += UpdateAttribute;
            color = vitalData.experienceColor;
            foreground.color = color;
            UpdateAttribute(pc.Experience, pc.ExpToLevel);
        }

        public void UpdateAttribute(int current, int max)
        {
            if (max > 0)
            {
                float width = (float)current / (float)max;
                //Debug.Log("current " + current + " / " + "max " + max + " = width " + width);
                foreground.GetComponent<RectTransform>().localScale = new Vector3(width, foreground.GetComponent<RectTransform>().localScale.y, foreground.GetComponent<RectTransform>().localScale.z);
            }
        }
    }
}