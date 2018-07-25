﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class PartyController : MonoBehaviour
{
    public LayerMask movementMask;
    public WorldInteractable focus;

    private Camera cam;
    private PcAnimator animator;
    [SerializeField]
    private PartyData partyData;
    private WorldPcMotor pcMotor;

    public PartyData PartyData { get { return partyData; } } 

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject() == true) return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000, movementMask))
            {
                //Debug.Log("hit " + hit.collider.name + " " + hit.point);
                pcMotor.SetMoveTarget(hit.point);
                RemoveFocus();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000))
            {
                //Debug.Log("hit " + hit.collider.name + " " + hit.point);
                Interactable interactable = hit.collider.GetComponent<Interactable>();

                if (interactable != null)
                {
                    pcMotor.SetMoveTarget(interactable.interactionTransform.position);
                    SetFocus(interactable);
                }
            }
        }
    }

    void SetFocus(Interactable interactable)
    {
        if (interactable != focus)
        {
            if(focus != null) focus.OnDefocused();

            focus = (WorldInteractable)interactable;

            pcMotor.FollowTarget(focus);
        }

        focus.OnFocused(transform);
    }

    void RemoveFocus()
    {
        if(focus != null) focus.OnDefocused();

        focus = null;

        pcMotor.StopFollowingTarget();
    }

    public void WorldInteraction()
    {
        animator.WorldInteraction();
    }

    public void SetPartyData(PartyData partyData)
    {
        this.partyData = new PartyData(partyData);
    }

    public void SetPcAnimator(PcAnimator animator)
    {
        this.animator = animator;
    }

    public void SetPcMotor(WorldPcMotor pcMotor)
    {
        this.pcMotor = pcMotor;
    }
}