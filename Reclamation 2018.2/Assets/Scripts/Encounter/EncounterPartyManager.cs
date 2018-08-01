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
        public List<Pc> pcControllers;
        public GameObject movementCursor;

        public List<Formation> defaultFormations;
        public List<Transform> formationTransforms;

        private Camera cam;
        private bool[] selectedPcs;
        private int formationIndex = 0;

        void Awake()
        {
            pcControllers = new List<Pc>();
            selectedPcs = new bool[PartyData.MaxPartySize];
        }

        public void Initialize()
        {
            cam = Camera.main;
            cam.gameObject.GetComponent<CameraRaycaster>().onMouseOverEnemy += OnMouseOverEnemy;
            cam.gameObject.GetComponent<CameraRaycaster>().onMouseOverInteractable += OnMouseOverInteractable;

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
            //Invoke("EnableMovement", .1f);
        }

        public void SetControllers(List<GameObject> pcs)
        {
            for (int i = 0; i < pcs.Count; i++)
            {
                pcControllers.Add(pcs[i].GetComponent<Pc>());
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
                pcControllers[i].CanMove(true);
            }
        }

        public void DisableMovement(bool canMove)
        {
            for (int i = 0; i < pcControllers.Count; i++)
            {
                if (pcControllers[i].isFighting == false)
                {
                    pcControllers[i].CanMove(false);
                }
            }
        }

        public void Stop()
        {
            for (int i = 0; i < pcControllers.Count; i++)
            {
                if (pcControllers[i].isFighting == false)
                {
                    pcControllers[i].StopAnimations();
                }
            }
        }

        public void OnMouseOverInteractable(GameObject target)
        {
            if (Input.GetMouseButtonUp(0))
            {
                Interactable interactable = target.GetComponent<Interactable>();

                if (interactable != null)
                {
                    pcControllers[0].SetInteractionTarget(target);
                    pcControllers[0].MoveTo(target);
                }
            }
        }

        public void OnMouseOverEnemy(GameObject target)
        {
            if (Input.GetMouseButtonUp(0))
            {                
                if (moveMode == MoveMode.Solo)
                {
                    if (pcControllers[0].CheckAttack(target) == true)
                    {
                        if (target.GetComponent<Reclamation.Characters.CharacterController>().CheckIsAlive() == true)
                        {
                            if(pcControllers[0].CheckIsAlive() == true)
                                pcControllers[0].SetAttackTarget(target);
                        }
                        else
                        {
                            if (pcControllers[0].CheckIsAlive() == true)
                                pcControllers[0].SetInteractionTarget(target);
                        }
                    }
                }
                else if (moveMode == MoveMode.Formation)
                {
                    if (target.GetComponent<Reclamation.Characters.CharacterController>().CheckIsAlive() == false)
                    {
                        if (pcControllers[0].CheckIsAlive() == true)
                        {
                            pcControllers[0].SetInteractionTarget(target);
                            pcControllers[0].MoveTo(target);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < pcControllers.Count; i++)
                        {
                            if (pcControllers[i].CheckRange(target) == true)
                            {
                                if (pcControllers[i].CheckIsAlive() == true)
                                    pcControllers[i].SetAttackTarget(target);
                            }
                            else
                            {
                                if (pcControllers[i].CheckIsAlive() == true)
                                {
                                    pcControllers[i].SetAttackTarget(target);
                                    pcControllers[i].MoveTo(target);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void MoveTo(GameObject target)
        {
            if (moveMode == MoveMode.Solo)
            {
                pcControllers[0].MoveTo(target);
            }
            else if (moveMode == MoveMode.Formation)
            {
                for (int i = 0; i < pcControllers.Count; i++)
                {
                    pcControllers[i].MoveTo(target);
                }
            }
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

            for (int i = 0; i < pcControllers.Count; i++)
            {
                pcControllers[i].CanMove(true);
                pcControllers[i].SetAttackTarget(null);
                pcControllers[i].GetComponent<AIDestinationSetter>().target = formationTransforms[i];
            }
        }
    }
}