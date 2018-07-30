using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reclamation.Props
{
    public abstract class Interactable : MonoBehaviour, IInteractable
    {
        public Transform interactionTransform;
        public float radius = 1f;
        public bool hasInteracted = false;
        public bool locked = false;

        protected Transform partyTransform;
        protected GameObject other;

        public abstract bool Interact(GameObject other);
    }
}