using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Pathfinding;

[System.Serializable]
public class EncounterPcController : MonoBehaviour
{
    public LayerMask movementMask;
    public WorldInteractable focus;
    
    private Camera cam;
    private PcAnimator animator;
    [SerializeField]
    private EncounterPcMotor pcMotor;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
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
                //WorldInteractable interactable = hit.collider.GetComponent<WorldInteractable>();

                //if (interactable != null)
                //{
                //    SetFocus(interactable);
                //}
            }
        }
    }

    void SetFocus(WorldInteractable interactable)
    {
        if (interactable != focus)
        {
            if (focus != null) focus.OnDefocused();

            focus = interactable;

            pcMotor.FollowTarget(focus);
        }

        interactable.OnFocused(transform);
    }

    void RemoveFocus()
    {
        if (focus != null) focus.OnDefocused();

        focus = null;

        pcMotor.StopFollowingTarget();
    }

    public void WorldInteraction()
    {
        animator.WorldInteraction();
    }

    public void SetPcAnimator(PcAnimator animator)
    {
        this.animator = animator;
    }

    public void SetPcMotor(EncounterPcMotor pcMotor)
    {
        this.pcMotor = pcMotor;
    }
}