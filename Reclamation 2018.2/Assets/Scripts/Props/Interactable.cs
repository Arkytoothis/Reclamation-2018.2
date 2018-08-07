using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reclamation.Props
{
    public abstract class Interactable : MonoBehaviour, IInteractable
    {
        public float radius = 1f;
        public bool hasInteracted = false;
        public bool locked = false;

        [SerializeField] Transform interactionTransform;
        protected GameObject other;

        public Transform InteractionTransform { get { return interactionTransform; } }

        public abstract bool Interact(GameObject other);
    }
}