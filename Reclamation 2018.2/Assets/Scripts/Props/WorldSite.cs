using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reclamation.Misc;

namespace Reclamation.Props
{
    public class WorldSite : Interactable
    {
        public string transitionTo;

        void Start()
        {
            transitionTo = "World";

            if (Random.Range(0, 100) < 50)
                locked = false;
            else
                locked = true;
        }

        public override bool Interact(GameObject other)
        {
            this.other = other;

            if (locked == true)
            {
                Debug.Log("Damn, locked...");
                return false;
            }
            else
            {
                Debug.Log(other.name + " has opened me, lets go!");
                hasInteracted = true;
                return true;
            }
        }
    }
}