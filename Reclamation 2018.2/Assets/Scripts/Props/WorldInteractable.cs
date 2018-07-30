using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cromos;
using Reclamation.Characters;
using Reclamation.World;

namespace Reclamation.Props
{
    public class WorldInteractable : Interactable
    {
        public ColorTransition colorTransition;
        public OutlineTarget outline;
        public Color defaultColor;

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;

            if (interactionTransform != null) Gizmos.DrawWireSphere(interactionTransform.position, radius);
        }

        void OnMouseOver()
        {
            colorTransition.enabled = true;
            outline.enabled = true;
        }

        void OnMouseExit()
        {
            colorTransition.enabled = false;
            outline.enabled = false;
            GetComponent<Renderer>().material.color = defaultColor;
        }

        public override bool Interact(GameObject other)
        {
            partyTransform.GetComponent<PartyController>().WorldInteraction();
            return true;
        }
    }
}