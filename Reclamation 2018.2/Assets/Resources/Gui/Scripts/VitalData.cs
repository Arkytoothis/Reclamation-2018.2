using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reclamation.Characters
{
    [CreateAssetMenu(menuName = "Vital Data")]
    public class VitalData : ScriptableObject
    {
        public Color armorColor;
        public Color healthColor;
        public Color staminaColor;
        public Color essenceColor;
        public Color moraleColor;
        public Color experienceColor;
    }
}