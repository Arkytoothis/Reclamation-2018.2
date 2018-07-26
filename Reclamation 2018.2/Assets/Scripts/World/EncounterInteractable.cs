using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cromos;

public class EncounterInteractable : Interactable
{
    public ColorTransition colorTransition;
    public OutlineTarget outline;
    public Color defaultColor;

    void Awake()
    {
        renderer = gameObject.GetComponent<Renderer>();
    }

    public void OnFocused(Transform partyTransform)
    {
        isFocus = true;
        this.partyTransform = partyTransform;
        hasInteracted = false;
    }

    public void OnDefocused()
    {
        isFocus = false;
        partyTransform = null;
        hasInteracted = false;
    }

    void Update()
    {
        if (isFocus == true && hasInteracted == false)
        {
            float distance = Vector3.Distance(partyTransform.position, interactionTransform.position);
            //Debug.Log(distance);
            if (distance <= radius)
            {
                Interact();
                hasInteracted = true;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        if (interactionTransform != null) Gizmos.DrawWireSphere(interactionTransform.position, radius);
    }

    void OnMouseOver()
    {
        colorTransition.enabled = true;
        outline.enabled = true;
    }

    void OnMouseExit()
    {
        colorTransition.enabled = false;
        outline.enabled = false;
        renderer.material.color = defaultColor;
    }
}