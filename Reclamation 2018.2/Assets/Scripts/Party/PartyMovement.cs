using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reclamation.Party
{
    [RequireComponent(typeof(PartyInput))]
    public class PartyMovement : MonoBehaviour
    {
        void Awake()
        {
            GetComponent<PartyInput>().onMoveOrderGiven += MoveOrderGiven;
            GetComponent<PartyInput>().onInteractOrderGiven += InteractOrderGiven;
        }

        public bool MoveOrderGiven(Vector3 destination)
        {
            //Debug.Log("Moving party to " + destination);
            return true;
        }

        public bool InteractOrderGiven(GameObject target)
        {
            //Debug.Log("Moving party to Interactable at" + target.transform.position);
            return true;
        }
    }
}