using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldInteractable : MonoBehaviour
{
    public Transform interactionTransform;
    public float radius = 3f;

    private bool isFocus = false;
    private bool hasInteracted = false;
    private Transform party;

    public virtual void Interact()
    {
        //Debug.Log("Interacting ");
        party.GetComponent<PartyController>().WorldInteraction();
    }

    public void OnFocused(Transform partyTransform)
    {
        isFocus = true;
        party = partyTransform;
        hasInteracted = false;
    }

    public void OnDefocused()
    {
        isFocus = false;
        party = null;
        hasInteracted = false;
    }

    void Update()
    {
        if (isFocus == true && hasInteracted == false)
        {
            float distance = Vector3.Distance(party.position, interactionTransform.position);

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
        Gizmos.DrawWireSphere(interactionTransform.position, radius);
    }
}