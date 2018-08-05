using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reclamation.Party
{
    public class PartyCursor : MonoBehaviour
    {
        [SerializeField] GameObject cursorObject;

        void Awake()
        {
            GetComponent<PartyInput>().onMoveOrderGiven += Move;
        }

        public bool Move(Vector3 destination)
        {
            Debug.Log("Moving Cursor");
            cursorObject.transform.position = destination;

            return true;
        }
    }
}