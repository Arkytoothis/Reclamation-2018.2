using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reclamation.Characters;
using Reclamation.Encounter;
using Reclamation.Equipment;

namespace Reclamation.Misc
{
    public class ModelManager : Singleton<ModelManager>
    {
        private Dictionary<string, GameObject> characterPrefabs;
        public Dictionary<string, GameObject> CharacterPrefabs { get { return characterPrefabs; } }

        private Dictionary<string, GameObject> itemPrefabs;
        public Dictionary<string, GameObject> ItemPrefabs { get { return itemPrefabs; } }

        private Dictionary<string, GameObject> hairPrefabs;
        public Dictionary<string, GameObject> HairPrefabs { get { return hairPrefabs; } }

        private Dictionary<string, GameObject> beardPrefabs;
        public Dictionary<string, GameObject> BeardPrefabs { get { return beardPrefabs; } }

        public List<GameObject> characters = new List<GameObject>();

        public GameObject emptyPcPrefb;

        public string defaultItemPath = "Equipment/Prefabs/";
        public string defaultCharacterPath = "Characters/Prefabs/";

        public GameObject GetItemPrefab(string key)
        {
            if (itemPrefabs.ContainsKey(key) == false)
            {
                Debug.Log("Prefab for " + key + " does not exist.");
                return null;
            }
            else
            {
                return itemPrefabs[key];
            }
        }

        public void Initialize()
        {
            itemPrefabs = new Dictionary<string, GameObject>();
            characterPrefabs = new Dictionary<string, GameObject>();
            hairPrefabs = new Dictionary<string, GameObject>();
            beardPrefabs = new Dictionary<string, GameObject>();

            LoadPrefabs();
        }

        public void LoadPrefabs()
        {
            LoadHairPrefabs();
            LoadBeardPrefabs();
            LoadCharacterPrefabs();
            LoadEquipmentPrefabs();
        }

        void LoadHairPrefabs()
        {
            Object[] prefabs;

            prefabs = Resources.LoadAll<GameObject>("Characters/Prefabs/Hair");

            foreach (GameObject go in prefabs)
            {
                hairPrefabs.Add(go.name, go);
            }
        }

        void LoadBeardPrefabs()
        {
            Object[] prefabs;

            prefabs = Resources.LoadAll<GameObject>("Characters/Prefabs/Beards");

            foreach (GameObject go in prefabs)
            {
                beardPrefabs.Add(go.name, go);
            }
        }

        void LoadEquipmentPrefabs()
        {
            foreach (KeyValuePair<string, ItemDefinition> kvp in Database.Items)
            {
                GameObject go = GetItemPrefab(kvp.Value);
                itemPrefabs.Add(kvp.Key, go);
            }
        }

        void LoadCharacterPrefabs()
        {
            emptyPcPrefb = Resources.Load<GameObject>(defaultCharacterPath + "PC");

            foreach (KeyValuePair<string, Race> kvp in Database.Races)
            {
                if (kvp.Value.maleModelPath != "" && kvp.Value.femaleModelPath != "")
                {
                    GameObject go = Resources.Load<GameObject>(defaultCharacterPath + kvp.Value.maleModelPath);
                    characterPrefabs.Add(kvp.Value.Name + " Male", go);
                    characters.Add(go);

                    go = Resources.Load<GameObject>(defaultCharacterPath + kvp.Value.femaleModelPath);
                    characterPrefabs.Add(kvp.Value.Name + " Female", go);
                    characters.Add(go);
                }
            }            
        }

        public GameObject SpawnCharacter(Transform parent, Vector3 position, PcData pc)
        {
            GameObject empty = null;

            if (pc.Gender == Gender.Male)
            {
                empty = Instantiate(emptyPcPrefb, parent);
            }
            else if (pc.Gender == Gender.Female)
            {
                empty = Instantiate(emptyPcPrefb, parent);
            }

            empty.transform.position = position;

            GameObject model = Instantiate(characterPrefabs[pc.RaceKey + " " + pc.Gender], empty.transform);
            empty.GetComponent<Pc>().SetPcData(pc, model);

            return empty;
        }

        public GameObject GetItemPrefab(ItemDefinition item)
        {
            if (item == null || item.MeshPath == defaultItemPath) return null;

            GameObject go = Resources.Load<GameObject>(defaultItemPath + item.MeshPath);

            if (go == null)
            {
                Debug.Log("Loading Item failed. " + item.MeshPath);
            }
            else
            {
                //Debug.Log("Loading Item " + go.name + " success. " + key);
            }

            return go;
        }

        public GameObject GetCharacterPrefab(Vector3 scale, string path)
        {
            if (path == defaultCharacterPath) return null;

            GameObject go = characterPrefabs[path];

            if (go == null)
            {
                Debug.Log("Loading Body failed. " + path);
            }
            else
            {
                go.transform.localScale = scale;
                //Debug.Log("Loading Body " + go.name + " success. " + path);
            }

            return go;
        }

        public GameObject GetHairModel(string hairKey)
        {
            if (hairPrefabs.ContainsKey(hairKey))
            {
                return hairPrefabs[hairKey];
            }
            else
            {
                return null;
            }
        }

        public GameObject GetBeardModel(string beardKey)
        {
            if (beardPrefabs.ContainsKey(beardKey))
            {
                return beardPrefabs[beardKey];
            }
            else
            {
                return null;
            }
        }

        
    }
}