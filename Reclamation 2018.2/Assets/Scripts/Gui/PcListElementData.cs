using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Reclamation.Gui
{
    [CreateAssetMenu(menuName = "Reclamation/Pc List Element")]
    public class PcListElementData : ScriptableObject
    {
        [Header("Image Data")]
        public Sprite background;
        public ColorBlock buttonColors;

        [Header("Sound Data")]
        public string clickSound;
    }
}