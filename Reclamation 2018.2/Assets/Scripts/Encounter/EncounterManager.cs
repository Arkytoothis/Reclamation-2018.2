using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reclamation.Audio;
using Reclamation.Characters;
using Reclamation.Equipment;
using Reclamation.Misc;
using Reclamation.Gui;
using Reclamation.Gui.Encounter;
using Guirao.UltimateTextDamage;

namespace Reclamation.Encounter
{
    public class EncounterManager : Singleton<EncounterManager>
    {
        const int MaxParties = 10;

        private List<PartyData_OLD> parties;
        private List<GameObject> pcs;
        public int PcsCount { get { return pcs.Count; } } 
        public UltimateTextDamageManager textManagerWorld;

        void Awake()
        {
            Invoke(nameof(Initialize), 0.1f);
            pcs = new List<GameObject>();
        }

        public void Initialize()
        {
            Database.Initialize();
            ItemGenerator.Initialize();
            PcGenerator.Initialize();
            NpcGenerator.Initialize();

            SpriteManager.instance.Initialize();
            AudioManager.instance.Initialize();
            ModelManager.instance.Initialize();
            ParticleManager.instance.Initialize();
            MessageSystem.instance.Initialize();

            parties = new List<PartyData_OLD>(MaxParties);

            PartyData_OLD party = new PartyData_OLD("Blue Party", Color.blue, 0);
            parties.Add(party);

            //CreatePc(new PcData(PcGenerator.Generate(0, Gender.Male, "Imperial", "Soldier")), 0);
            //CreatePc(new PcData(PcGenerator.Generate(1, Gender.Female, "Imperial", "Soldier")), 1);
            //CreatePc(new PcData(PcGenerator.Generate(2, Gender.Male, "Imperial", "Scout")), 2);
            //CreatePc(new PcData(PcGenerator.Generate(3, Gender.Female, "Halfling", "Rogue")), 3);
            //CreatePc(new PcData(PcGenerator.Generate(4, Gender.Male, "High Elf", "Apprentice")), 4);
            //CreatePc(new PcData(PcGenerator.Generate(5, Gender.Female, "Mountain Dwarf", "Priest")), 5);

            EncounterPartyManager.instance.Initialize();

            EncounterCursor.instance.pointToTarget = pcs[0].transform;
            EncounterGuiManager.instance.Initialize();

            for (int i = 0; i < pcs.Count; i++)
            {
                pcs[i].transform.position = EncounterPartyManager.instance.formationTransforms[i].position;

                if (i == 0)
                    pcs[i].GetComponent<PcController>().light.enabled = true;
                else
                    pcs[i].GetComponent<PcController>().light.enabled = false;
            }

            EncounterPartyManager.instance.SetControllers(pcs);

            MessageSystem.instance.AddMessage("Welcome to Reclamation!");

            AudioManager.instance.PlayMusic("01 Along the Journey quiet");
            AudioManager.instance.PlayAmbient("castle abandoned");
        }

        public void CreatePc(PcData pcData, int index)
        {
            pcs.Add(ModelManager.instance.SpawnCharacter(transform, EncounterPartyManager.instance.formationTransforms[index].position, pcData));
        }

        void Update()
        {            
            if (Input.GetKeyUp(KeyCode.Space))
            {
                for (int i = 0; i < pcs.Count; i++)
                {
                    if (pcs[i] != null)
                    {
                        pcs[i].GetComponent<PcController>().CurrentDefense.Heal(1000);
                    }
                }
            }
            else if (Input.GetKeyUp(KeyCode.K))
            {
                for (int i = 0; i < pcs.Count; i++)
                {
                    if (pcs[i] != null)
                    {
                        pcs[i].GetComponent<PcController>().CurrentDefense.Damage(1000);
                    }
                }
            }

            if (Input.GetKeyUp(KeyCode.F1))
            {
                EncounterPartyManager.instance.SetCurrentPc(0);
            }
            else if (Input.GetKeyUp(KeyCode.F2))
            {
                EncounterPartyManager.instance.SetCurrentPc(1);
            }
            else if (Input.GetKeyUp(KeyCode.F3))
            {
                EncounterPartyManager.instance.SetCurrentPc(2);
            }
            else if (Input.GetKeyUp(KeyCode.F4))
            {
                EncounterPartyManager.instance.SetCurrentPc(3);
            }
            else if (Input.GetKeyUp(KeyCode.F5))
            {
                EncounterPartyManager.instance.SetCurrentPc(4);
            }
            else if (Input.GetKeyUp(KeyCode.F6))
            {
                EncounterPartyManager.instance.SetCurrentPc(5);
            }
        }

        public PcData GetPcData(string name)
        {
            PcData pcData = null;

            foreach (GameObject go in pcs)
            {
                pcData = go.GetComponent<PcController>().PcData;

                if (pcData.Name.FullName.Equals(name) == true)
                {
                    Debug.Log("PcData for " + name + " found");
                    break;
                }
            }

            if (pcData == null)
            {
                Debug.Log("PcData for " + name + " not found");
            }

            return pcData;
        }

        public PcData GetPcData(int index)
        {
            if (index < 0 || index > pcs.Count - 1)
            {
                return null;
            }

            PcData pcData = pcs[index].GetComponent<PcController>().PcData;

            if (pcData == null)
            {
                Debug.Log("pcData == null");
            }

            return pcData;
        }

        public GameObject GetPcObject(int index)
        {
            if (index < 0 || index > pcs.Count - 1)
            {
                return null;
            }

            GameObject go = pcs[index];

            if (go == null)
            {
                Debug.Log("pcData == null");
            }

            return go;
        }

        public PcData GetSelectedPcData()
        {
            int index = EncounterPartyManager.instance.SelectedPc;

            if (index < 0 || index > pcs.Count - 1)
            {
                return null;
            }

            PcData pcData = pcs[index].GetComponent<PcController>().PcData;

            if (pcData == null)
            {
                Debug.Log("pcData == null");
            }

            return pcData;
        }
    }
}