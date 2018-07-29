using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Reclamation.Gui
{
    [CreateAssetMenu(menuName = "Flexible Button")]
    public class ButtonData : ScriptableObject
    {
        [Header("Image Data")]
        public Sprite background;
        public Sprite icon;
        public ColorBlock buttonColors;
        public string text;

        [Header("Sound Data")]
        public Sound clickSound;
    }
}
