using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Pathfinding;

public class EncounterPartyManager : Singleton<EncounterPartyManager>
{
    public enum MoveMode { Follow, Formation, Solo };

    public MoveMode moveMode = MoveMode.Follow;
    public LayerMask movementMask;
    public List<EncounterPcController> pcControllers;
    public GameObject movementCursor;

    private Camera cam;
    private bool[] selectedPcs;

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
            pcControllers[i].GetComponent<AIDestinationSetter>().target = pcControllers[i-1].gameObject.transform;
        }
    }

    public void FormationMode()
    {
        for (int i = 0; i < pcControllers.Count; i++)
        {
            pcControllers[i].GetComponent<AIDestinationSetter>().target = EncounterManager.instance.formationPositions[i];
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
}