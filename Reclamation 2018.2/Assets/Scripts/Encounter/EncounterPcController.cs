using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Reclamation.Characters;
using Reclamation.Misc;
using Reclamation.Props;

namespace Reclamation.Encounter
{
    [System.Serializable]
    public class EncounterPcController : MonoBehaviour
    {
        public Interactable focus;
        public float moveSpeed = 3f;

        [SerializeField]
        private PcAnimator animator;
        private Pc pcData;

        public void SetPcData(Pc pc)
        {
            pcData = pc;
            pcData.onDeath += animator.Death;
            pcData.onRevive += animator.Revive;
            pcData.onLevelUp += animator.LevelUp;
        }

        public void SetFocus(Interactable interactable)
        {
            if (interactable != focus)
            {
                if (focus != null) focus.OnDefocused();

                focus = interactable;
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
            animator.EncounterInteraction();
        }

        public void AttackInteraction()
        {
            animator.Attack();
        }

        public void StopAnimations()
        {
            animator.Stop();
        }
    }
}