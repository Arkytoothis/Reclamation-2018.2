using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Reclamation.Props;
using Reclamation.Misc;

namespace Reclamation.Encounter
{
    public class EncounterCursor : Singleton<EncounterCursor>
    {
        public LayerMask movementMask;
        public Transform pointToTarget;
        public float turnSpeed = 20f;
        public List<GameObject> markers;
        public bool isEnabled = true;

        private Camera cam;
        private bool interactableFound;
        private CameraRaycaster raycaster;

        void Awake()
        {
            cam = Camera.main;
            raycaster = cam.GetComponent<CameraRaycaster>();
        }

        void Start()
        {
            raycaster.onMouseOverWalkable += OnMouseOverWalkable;
        }

        public void OnMouseOverWalkable(Vector3 position)
        {
            //if (Input.GetMouseButtonDown(0))
            //{
            //    gameObject.transform.position = position;

            //    if (pointToTarget)
            //    {
            //        transform.LookAt(pointToTarget, Vector3.up);
            //        EncounterPartyManager.instance.EnableMovement();
            //        EncounterPartyManager.instance.ResetFormation();
            //    }
            //}

            //if (Input.GetMouseButton(0))
            //{
            //    gameObject.transform.position = position;

            //    if (pointToTarget)
            //    {
            //        transform.LookAt(pointToTarget, Vector3.up);
            //        EncounterPartyManager.instance.ResetFormation();
            //    }
            //}

            if (Input.GetMouseButtonDown(1))
            {
                EncounterPartyManager.instance.Stop();
                EncounterPartyManager.instance.DisableMovement(false);
                gameObject.transform.position = position;
                transform.LookAt(pointToTarget, Vector3.up);
                EncounterPartyManager.instance.ResetFormation();
            }

            if (Input.GetMouseButton(1))
            {
                if (pointToTarget)
                {
                    EncounterPartyManager.instance.DisableMovement(false);
                    transform.Rotate(Vector3.up, (-Input.GetAxis("Mouse X") * Time.deltaTime * 300f));
                }
            }

            if (Input.GetMouseButtonUp(1))
            {
                EncounterPartyManager.instance.EnableMovement();
            }
        }

        public void EnableMarkers()
        {
            for (int i = 0; i < markers.Count; i++)
            {
                markers[i].SetActive(true);
            }
        }

        public void DisableMarkers()
        {
            for (int i = 0; i < markers.Count; i++)
            {
                markers[i].SetActive(false);
            }
        }
    }
}