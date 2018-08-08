using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Reclamation.Gui
{
    [CreateAssetMenu(menuName = "Reclamation/Attribute Element")]
    public class AttributeElementData : ScriptableObject
    {
        [Header("Image Data")]
        public ColorBlock buttonColors;
        public Color textColor;

        [Header("Sound Data")]
        public string clickSound;
    }
}
