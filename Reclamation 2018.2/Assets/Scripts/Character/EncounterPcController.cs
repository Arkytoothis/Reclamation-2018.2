using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class EncounterPcController : Singleton<EncounterPcController>
{
    public EncounterInteractable focus;
    public float moveSpeed = 3f;

    [SerializeField]
    private PcAnimator animator;

    public void SetFocus(Interactable interactable)
    {
        if (interactable != focus)
        {
            if (focus != null) focus.OnDefocused();

            focus = (EncounterInteractable)interactable;
        }

        focus.OnFocused(transform);
    }

    public void RemoveFocus()
    {
        if (focus != null) focus.OnDefocused();

        focus = null;
    }

    public void EncounterInteraction()
    {
        animator.WorldInteraction();
    }

    public void StopAnimations()
    {
        animator.Stop();
    }
}