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
        [SerializeField] Texture2D walkCursor = null;
        [SerializeField] Texture2D unknownCursor = null;
        [SerializeField] Texture2D guiCursor = null;

        private Camera cam;
        private bool interactableFound;

        void Awake()
        {
            cam = Camera.main;
        }

        void Start()
        {
            InvokeRepeating("UpdateCursor", 0.1f, 0.1f);
        }

        public void UpdateCursor()
        {
            if (isEnabled == false) return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000))
            {
                if (hit.collider.gameObject.layer == 8)
                {
                    Cursor.SetCursor(walkCursor, Vector2.zero, CursorMode.Auto);
                }
                else if (hit.collider.gameObject.layer == 0)
                {
                    Cursor.SetCursor(unknownCursor, Vector2.zero, CursorMode.Auto);
                }
            }
        }

        void Update()
        {
            if (EventSystem.current.IsPointerOverGameObject() == true)
            {
                Cursor.SetCursor(guiCursor, Vector2.zero, CursorMode.Auto);
                return;
            }

            if (isEnabled == false) return;

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit; 

                if (Physics.Raycast(ray, out hit, 1000))
                {
                    Interactable interactable = hit.collider.GetComponent<Interactable>();

                    if (interactable != null)
                    {
                        interactableFound = true;
                        EncounterPartyManager.instance.MoveToInteractable(interactable);
                    }
                    else
                    {
                        interactableFound = false;
                    }
                }
            }

            if (interactableFound == false && Input.GetMouseButton(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 1000, movementMask))
                {
                    gameObject.transform.position = hit.point;

                    if (pointToTarget)
                    {
                        transform.LookAt(pointToTarget, Vector3.up);
                        EncounterPartyManager.instance.ResetFormation();
                    }
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 1000, movementMask))
                {
                    EncounterPartyManager.instance.Stop();
                    gameObject.transform.position = hit.point;
                    transform.LookAt(pointToTarget, Vector3.up);
                    EncounterPartyManager.instance.ResetFormation();
                }
            }

            if (Input.GetMouseButton(1))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 1000, movementMask))
                {
                    if (pointToTarget)
                    {
                        EncounterPartyManager.instance.DisableMovement(false);
                        transform.Rotate(Vector3.up, (-Input.GetAxis("Mouse X") * Time.deltaTime * 300f));
                    }
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