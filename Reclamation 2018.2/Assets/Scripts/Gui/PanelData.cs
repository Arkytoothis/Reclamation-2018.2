using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Reclamation.Gui
{
    [CreateAssetMenu(menuName = "Reclamation/Panel")]
    public class PanelData : ScriptableObject
    {
        [Header("Image Data")]
        public Color headingBackgroundColor;
        public Color headingLabelColor;
        public Color backgroundColor;
        public Color borderColor;
    }
}