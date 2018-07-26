using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EncounterCursor : Singleton<EncounterCursor>
{
    public LayerMask movementMask;
    public Transform pointToTarget;
    public float turnSpeed = 20f;
    public List<GameObject> markers;

    private Camera cam;

    void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject() == true) return;

        if (Input.GetMouseButton(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000, movementMask))
            {
                //Debug.Log("hit " + hit.collider.name + " " + hit.point);
                gameObject.transform.position = hit.point;
                if (pointToTarget)
                {
                    transform.LookAt(pointToTarget, Vector3.up);
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