using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reclamation.Props
{
    public abstract class Interactable : MonoBehaviour
    {
        public Transform interactionTransform;
        public float radius = 3f;

        protected bool isFocus = false;
        protected bool hasInteracted = false;

        protected Transform partyTransform;
        protected new Renderer renderer;


        public virtual void OnFocused(Transform partyTransform)
        {
            isFocus = true;
            this.partyTransform = partyTransform;
            hasInteracted = false;
        }

        public virtual void OnDefocused()
        {
            isFocus = false;
            partyTransform = null;
            hasInteracted = false;
        }

        public virtual void Interact()
        {
        }
    }
}