using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reclamation.Props
{
    public class Container : Interactable
    {
        public int gold;

        void Start()
        {
            gold = Random.Range(1, 100);

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
                Debug.Log(other.name + " has opened me, heres " + gold + " gold");
                hasInteracted = true;
                return true;
            }
            
        }
    }
}