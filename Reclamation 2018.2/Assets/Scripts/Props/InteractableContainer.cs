using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cromos;
using Reclamation.Encounter;

namespace Reclamation.Props
{
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(ColorTransition))]
    [RequireComponent(typeof(OutlineTarget))]

    public class InteractableContainer : Interactable
    {
        public ColorTransition colorTransition;
        public OutlineTarget outline;
        public Color defaultColor;
        public InteractableData interactableData;

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

        void Update()
        {
            if (isFocus == true && hasInteracted == false)
            {
                float distance = Vector3.Distance(partyTransform.position, interactionTransform.position);
                //Debug.Log(distance);
                if (distance <= radius)
                {
                    Interact();
                    hasInteracted = true;
                }
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;

            if (interactionTransform != null) Gizmos.DrawWireSphere(interactionTransform.position, radius);
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
            renderer.material.color = defaultColor;
        }

        public override void Interact()
        {
            EncounterPartyManager.instance.Interact(this);
        }
    }
}