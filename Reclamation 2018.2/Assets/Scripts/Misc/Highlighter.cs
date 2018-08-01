using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cromos;

namespace Reclamation.Props
{
    public class Highlighter : MonoBehaviour
    {
        public new Renderer renderer;
        public InteractableData interactableData;

        public ColorTransition colorTransition;
        public OutlineTarget outline;
        public Color defaultColor;

        void Awake()
        {
            renderer = gameObject.GetComponent<Renderer>();
            colorTransition.enabled = false;
            outline.enabled = false;
        }

        void Start()
        {
            colorTransition.EndColor = interactableData.mainColor;
            outline.OutlineColor = interactableData.outlineColor;
        }

        void OnMouseOver()
        {
            colorTransition.enabled = true;
            outline.enabled = true;
            Cursor.SetCursor(interactableData.cursor, Vector2.zero, CursorMode.Auto);
        }

        void OnMouseExit()
        {
            colorTransition.enabled = false;
            outline.enabled = false;

            if (renderer != null)
            {
                renderer.material.color = defaultColor;
            }
        }
    }
}