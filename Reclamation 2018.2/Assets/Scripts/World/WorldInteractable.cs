using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public class WorldInteractable : Interactable
{    
    private Transform partyTransform;
    public Outline outline;

    void AWake()
    {
        outline = GetComponent<Outline>();
    }

    void Start()
    {
        if(outline != null) outline.enabled = false;
    }

    public virtual void Interact()
    {
        //Debug.Log("Interacting ");
        partyTransform.GetComponent<PartyController>().WorldInteraction();
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

            if(distance <= radius)
            {
                Interact();
                hasInteracted = true;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        if(interactionTransform != null) Gizmos.DrawWireSphere(interactionTransform.position, radius);
    }

    void OnMouseOver()
    {
        outline.enabled = true;
    }

    void OnMouseExit()
    {
        outline.enabled = false;
    }
}