using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Pathfinding;
using Reclamation.Characters;
using Reclamation.Equipment;
using Reclamation.Misc;
using Reclamation.Gui.Encounter;
using Reclamation.World;

namespace Reclamation.Encounter
{
    public class EncounterManager : Singleton<EncounterManager>
    {
        const int MaxParties = 10;

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

            party.pcs[0] = new Pc(PcGenerator.Generate(0, Gender.Female, "Imperial", "Soldier"));
            party.pcs[1] = new Pc(PcGenerator.Generate(1, Gender.Male, "Imperial", "Scout"));
            //party.pcs[2] = new Pc(PcGenerator.Generate(2, "Imperial", "Priest"));
            //party.pcs[3] = new Pc(PcGenerator.Generate(3, "Imperial", "Soldier"));
            //party.pcs[4] = new Pc(PcGenerator.Generate(4, "Imperial", "Scout"));
            //party.pcs[5] = new Pc(PcGenerator.Generate(5, "Imperial", "Priest"));

            parties.Add(party);

            CreatePcs();
            EncounterPartyManager.instance.Initialize();

            EncounterCursor.instance.pointToTarget = pcs[0].transform;
            EncounterGuiManager.instance.Initialize();

            for (int i = 0; i < parties[0].pcs.Length; i++)
            {
                if (parties[0].pcs[i] != null)
                {
                    pcs[i].transform.position = EncounterPartyManager.instance.formationTransforms[i].position;

                    if (i == 0)
                        pcs[i].GetComponent<EncounterPcController>().light.enabled = true;
                    else
                        pcs[i].GetComponent<EncounterPcController>().light.enabled = false;
                }
            }

        }

        public void CreatePcs()
        {
            for (int i = 0; i < parties[0].pcs.Length; i++)
            {
                if (parties[0].pcs[i] != null)
                {
                    pcs.Add(ModelManager.instance.SpawnPc(transform, EncounterPartyManager.instance.formationTransforms[i].position, parties[0].pcs[i]));
                }
            }

            EncounterPartyManager.instance.SetControllers(pcs);
        }

        public void CreatePortraitModel(Transform parent, Pc pc)
        {
            ModelManager.instance.SpawnPc(parent, parent.position, pc);
        }

        void Update()
        {            
            if (Input.GetKeyUp(KeyCode.Space))
            {
                for (int i = 0; i < parties[0].pcs.Length; i++)
                {
                    if (parties[0].pcs[i] != null)
                    {
                        parties[0].pcs[i].ModifyAttribute(AttributeType.Derived, (int)DerivedAttribute.Armor, -Random.Range(0, 1));
                        parties[0].pcs[i].ModifyAttribute(AttributeType.Derived, (int)DerivedAttribute.Health, -Random.Range(0, 6));
                        parties[0].pcs[i].ModifyAttribute(AttributeType.Derived, (int)DerivedAttribute.Stamina, -Random.Range(0, 6));
                        parties[0].pcs[i].ModifyAttribute(AttributeType.Derived, (int)DerivedAttribute.Essence, -Random.Range(0, 6));
                        parties[0].pcs[i].ModifyAttribute(AttributeType.Derived, (int)DerivedAttribute.Morale, -Random.Range(0, 6));

                        parties[0].pcs[i].AddExperience(Random.Range(0, 150), true);
                    }
                }
            }
        }
    }
}