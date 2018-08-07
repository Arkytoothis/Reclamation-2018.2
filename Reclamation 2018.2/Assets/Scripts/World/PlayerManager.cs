using System.Collections.Generic;
using UnityEngine;
using Reclamation.Gui.World;
using Reclamation.Misc;
using Reclamation.Characters;
using Reclamation.Party;
using Reclamation.Props;
using Pathfinding;

namespace Reclamation.World
{
    public class PlayerManager : Singleton<PlayerManager>
    {
        const int MaxParties = 10;


        [SerializeField] Transform partySpawn;

        [SerializeField] GameObject partyPrefab;

        [SerializeField] int numPartiesUnlocked;
        [SerializeField] List<GameObject> parties;
        [SerializeField] List<GameObject> pcs;

        [SerializeField] PartyPanel partyPanel;

        public List<GameObject> Parties { get { return parties; } }
        public List<GameObject> Pcs { get { return pcs; } }

        public void Initialize()
        {
            parties = new List<GameObject>(MaxParties);
            pcs = new List<GameObject>();

            numPartiesUnlocked = 1;

            CreateParty(partySpawn.position);

            CreatePc(0, Gender.Male, "Imperial", "Soldier");
            CreatePc(1, Gender.Female, "Mountain Dwarf", "Priest");
            CreatePc(2, Gender.Male, "Halfling", "Scout");

            //Camera.main.GetComponent<CameraController>().SetTarget(pcs[0].transform);
            PartyCursor.instance.PlaceMoveCursor(pcs[0].transform.position);

            partyPanel.Initialize();
        }

        void CreateParty(Vector3 spawnPosition)
        {
            float y = Terrain.activeTerrain.SampleHeight(new Vector3(spawnPosition.x, 0, spawnPosition.z));
            spawnPosition = new Vector3(spawnPosition.x, y + 2, spawnPosition.z);

            GameObject partyObject = Instantiate(partyPrefab);
            partyObject.transform.position = spawnPosition;

            parties.Add(partyObject);

            PartyData newParty = parties[0].GetComponent<PartyData>();
            newParty.SetPartyData("Blue Party", Color.blue, 0);
        }

        void CreatePc(int index, Gender gender, string race, string profession)
        {
            GameObject pcObject = ModelManager.instance.SpawnCharacter(parties[0].transform, parties[0].transform.position, gender, race);
            pcObject.GetComponent<AIDestinationSetter>().target = null;

            PcData pcData = pcObject.GetComponent<PcData>();
            pcData = PcGenerator.Generate(pcObject, index, gender, race, profession);

            pcs.Add(pcObject);
        }

        public PartyData GetPartyData(int index)
        {
            if (parties[index].GetComponent<PartyData>() != null)
                return parties[index].GetComponent<PartyData>();
            else
                return null;
        }

        public PcData GetPcData(int index)
        {
            if (pcs[index] != null)
                return pcs[index].GetComponent<PcData>();
            else
                return null;
        }
    }
}
