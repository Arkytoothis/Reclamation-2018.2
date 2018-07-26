using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Pathfinding;

public class EncounterManager : Singleton<EncounterManager>
{
    const int MaxParties = 10;

    public Transform[] formationPositions;
    public List<PartyData> parties;
    public List<GameObject> pcs;

    void Awake()
    {
        Invoke("Initialize", 0.1f);
        pcs = new List<GameObject>();
    }

    public void Initialize()
    {
        Database.Initialize();
        ItemGenerator.Initialize();
        PcGenerator.Initialize();
        ModelManager.instance.Initialize();

        //WorldManager.instance.Initialize();
        //ScreenManager.instance.Initialize();

        parties = new List<PartyData>(MaxParties);

        PartyData party = new PartyData("Blue Party", Color.blue, 0, 3);

        party.pcs[0] = new Pc(PcGenerator.Generate(0, "Imperial", "Soldier"));
        party.pcs[1] = new Pc(PcGenerator.Generate(1, "Imperial", "Scout"));
        party.pcs[2] = new Pc(PcGenerator.Generate(2, "Imperial", "Priest"));
        party.pcs[3] = new Pc(PcGenerator.Generate(0, "Imperial", "Soldier"));
        party.pcs[4] = new Pc(PcGenerator.Generate(1, "Imperial", "Scout"));
        party.pcs[5] = new Pc(PcGenerator.Generate(2, "Imperial", "Priest"));

        parties.Add(party);
        //ModelManager.instance.SpawnCharacter(PortraitRoom.instance.characterMounts[0].pivot, party.pcs[0]);
        //ModelManager.instance.SpawnCharacter(PortraitRoom.instance.characterMounts[1].pivot, party.pcs[1]);
        //ModelManager.instance.SpawnCharacter(PortraitRoom.instance.characterMounts[2].pivot, party.pcs[2]);

        CreatePcs();
        EncounterPartyManager.instance.Initialize();

        EncounterCursor.instance.pointToTarget = pcs[0].transform;
    }

    public void CreatePcs()
    {
        for (int i = 0; i < parties[0].pcs.Length; i++)
        {
            if (parties[0].pcs[i] != null)
            {
                pcs.Add(ModelManager.instance.SpawnCharacter(transform, parties[0].pcs[i]));
            }
        }

        EncounterPartyManager.instance.SetControllers(pcs);
    }

    public void CreatePortraitModel(Transform parent, Pc pc)
    {
        ModelManager.instance.SpawnCharacter(parent, pc);
    }
}