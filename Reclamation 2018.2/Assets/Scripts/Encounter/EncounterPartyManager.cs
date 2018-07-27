using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Reclamation.Misc;
using Reclamation.Props;
using Reclamation.Characters;
using Reclamation.World;

namespace Reclamation.Encounter
{
    public class EncounterPartyManager : Singleton<EncounterPartyManager>
    {
        public enum MoveMode { Follow, Formation, Solo };

        public MoveMode moveMode = MoveMode.Follow;
        public LayerMask movementMask;
        public List<EncounterPcController> pcControllers;
        public GameObject movementCursor;

        public List<Formation> defaultFormations;
        public List<Transform> formationTransforms;

        private Camera cam;
        private bool[] selectedPcs;
        private int formationIndex = 0;

        void Awake()
        {
            pcControllers = new List<EncounterPcController>();
            selectedPcs = new bool[PartyData.MaxPartySize];
        }

        public void Initialize()
        {
            cam = Camera.main;

            for (int i = 0; i < selectedPcs.Length; i++)
            {
                selectedPcs[i] = false;
            }

            selectedPcs[0] = true;
            selectedPcs[1] = true;
            selectedPcs[2] = true;

            SetCurrentPc(0);
            SetMoveMode(MoveMode.Formation);
            SetFormation(0);
            DisableMovement(false);
            Invoke("EnableMovement", 1f);
        }

        public void SetControllers(List<GameObject> pcs)
        {
            for (int i = 0; i < pcs.Count; i++)
            {
                pcControllers.Add(pcs[i].GetComponent<EncounterPcController>());
            }
        }

        public void SetCurrentPc(int index)
        {
            cam.GetComponent<CameraController>().target = pcControllers[index].gameObject.transform;
        }

        public void SetMoveMode(MoveMode mode)
        {
            moveMode = mode;

            switch (mode)
            {
                case MoveMode.Follow:
                    FollowMode();
                    break;
                case MoveMode.Formation:
                    FormationMode();
                    break;
                case MoveMode.Solo:
                    SoloMode();
                    break;
                default:
                    break;
            }
        }

        public void FollowMode()
        {
            pcControllers[0].GetComponent<AIDestinationSetter>().target = EncounterCursor.instance.transform;

            for (int i = 1; i < pcControllers.Count; i++)
            {
                pcControllers[i].GetComponent<AIDestinationSetter>().target = pcControllers[i - 1].gameObject.transform;
            }
        }

        public void FormationMode()
        {
            for (int i = 0; i < pcControllers.Count; i++)
            {
                pcControllers[i].GetComponent<AIDestinationSetter>().target = formationTransforms[i];
            }
        }

        public void SoloMode()
        {
            pcControllers[0].GetComponent<AIDestinationSetter>().target = EncounterCursor.instance.transform;
        }

        public void EnableMovement()
        {
            for (int i = 0; i < pcControllers.Count; i++)
            {
                pcControllers[i].GetComponent<AIPath>().canSearch = true;
                pcControllers[i].GetComponent<AIPath>().canMove = true;
            }
        }

        public void DisableMovement(bool canSearch)
        {
            for (int i = 0; i < pcControllers.Count; i++)
            {
                pcControllers[i].GetComponent<AIPath>().canSearch = canSearch;
                pcControllers[i].GetComponent<AIPath>().canMove = false;
            }
        }

        public void Stop()
        {
            for (int i = 0; i < pcControllers.Count; i++)
            {
                pcControllers[i].StopAnimations();
            }
        }

        public void MoveToInteractable(Interactable interactable)
        {
            formationTransforms[0].position = new Vector3(interactable.interactionTransform.position.x, 0, interactable.interactionTransform.position.z);
            pcControllers[0].SetFocus(interactable);
        }

        public void Interact(Interactable interactable)
        {
            pcControllers[0].EncounterInteraction();
        }

        public void SetFormation(int index)
        {
            formationIndex = index;
            ResetFormation();
        }

        public void ResetFormation()
        {
            for (int i = 0; i < PartyData.MaxPartySize; i++)
            {
                formationTransforms[i].localPosition = new Vector3(defaultFormations[formationIndex].Positions[i].x, 0, defaultFormations[formationIndex].Positions[i].y);
            }
        }
    }
}