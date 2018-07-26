using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public Transform interactionTransform;
    public float radius = 3f;

    protected bool isFocus = false;
    protected bool hasInteracted = false;

    protected Transform partyTransform;
    protected new Renderer renderer;

    public virtual void Interact()
    {
        partyTransform.GetComponent<PartyController>().WorldInteraction();
    }
}