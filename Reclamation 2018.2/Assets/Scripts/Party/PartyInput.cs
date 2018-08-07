using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Reclamation.Misc;

namespace Reclamation.Party
{
    public class PartyInput : MonoBehaviour
    {
        public delegate bool OnInteractOrderGiven(GameObject target);
        public event OnInteractOrderGiven onInteractOrderGiven;

        public delegate bool OnMoveOrderGiven(Transform target);
        public event OnMoveOrderGiven onMoveOrderGiven;

        public delegate bool OnTerrainClicked(RaycastHit hit);
        public event OnTerrainClicked onTerrainClicked;

        void Awake()
        {
            Camera.main.GetComponent<CameraRaycaster>().onMouseOverWalkable += MouseOverWalkable;
            Camera.main.GetComponent<CameraRaycaster>().onMouseOverInteractable += MouseOverInteractable;
        }

        public bool MouseOverWalkable(RaycastHit hit)
        {
            if (EventSystem.current.IsPointerOverGameObject() == true) return false;

            if (Input.GetMouseButtonDown(0))
            {
            }

            if (Input.GetMouseButtonDown(1))
            {
                //Debug.Log("Move order given " + destination);

                PartyCursor.instance.PlaceMoveCursor(hit.point);
                //onTerrainClicked(hit);
                onMoveOrderGiven(PartyCursor.instance.transform);
            }

            return true;
        }

        public bool MouseOverInteractable(GameObject target)
        {
            if (EventSystem.current.IsPointerOverGameObject() == true) return false;

            if (Input.GetMouseButtonDown(0))
            {
            }

            if (Input.GetMouseButtonDown(1))
            {
                Debug.Log("Interaction order given " + target.transform.position);
                onInteractOrderGiven(target);
            }
            return true;
        }
    }
}