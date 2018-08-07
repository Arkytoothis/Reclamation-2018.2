using Reclamation.Characters;
using Reclamation.Props;
using Reclamation.World;
using UnityEngine;

namespace Reclamation.Party
{
    [RequireComponent(typeof(PartyInput))]
    public class PartyMovement : MonoBehaviour
    {
        void Awake()
        {
            GetComponent<PartyInput>().onMoveOrderGiven += MoveOrderGiven;
            GetComponent<PartyInput>().onInteractOrderGiven += InteractOrderGiven;
        }

        public bool MoveOrderGiven(Transform target)
        {
            //Debug.Log("Moving Party to " + target.position);

            PlayerManager.instance.Pcs[0].GetComponent<PcController>().MoveTo(target);

            for (int i = 1; i < PlayerManager.instance.Pcs.Count; i++)
            {
                PlayerManager.instance.Pcs[i].GetComponent<PcController>().MoveTo(PlayerManager.instance.Pcs[i - 1].GetComponent<PcController>().FollowTarget);
            }

            return true;
        }

        public bool InteractOrderGiven(GameObject target)
        {
            Transform interactionTarget = target.GetComponent<Interactable>().InteractionTransform;
            PartyCursor.instance.PlaceMoveCursor(interactionTarget.position);

            //Debug.Log("Moving party to Interactable at" + target.transform.position);
            PlayerManager.instance.Pcs[0].GetComponent<PcController>().MoveTo(interactionTarget);

            for (int i = 1; i < PlayerManager.instance.Pcs.Count; i++)
            {
                PlayerManager.instance.Pcs[i].GetComponent<PcController>().MoveTo(PlayerManager.instance.Pcs[i - 1].GetComponent<PcController>().FollowTarget);
            }

            return true;
        }
    }
}