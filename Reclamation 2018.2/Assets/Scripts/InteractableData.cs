using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reclamation.Props
{
    [CreateAssetMenu(menuName = "Interactable Button")]
    public class InteractableData : ScriptableObject
    {
        public Color mainColor;
        public Color outlineColor;
        public Texture2D cursor;
    }
}