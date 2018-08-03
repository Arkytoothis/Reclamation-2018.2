using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Reclamation.Gui
{
    [CreateAssetMenu(menuName = "Reclamation/Action Button")]
    public class ActionButtonData : ScriptableObject
    {
        [Header("Image Data")]
        public ColorBlock buttonColors;
        public Color cooldownColor;
        public Color cooldownTextColor;

        [Header("Sound Data")]
        public Sound clickSound;
    }
}