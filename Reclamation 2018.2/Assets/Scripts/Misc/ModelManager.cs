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

        public GameObject emptyPcPrefab;

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
            emptyPcPrefab = Resources.Load<GameObject>(defaultCharacterPath + "Character Controllers/PC");

            foreach (KeyValuePair<string, Race> kvp in Database.Races)
            {
                if (kvp.Value.maleModelPath != "" && kvp.Value.femaleModelPath != "")
                {
                    GameObject go = Resources.Load<GameObject>(defaultCharacterPath + "Races/" + kvp.Value.maleModelPath);
                    characterPrefabs.Add(kvp.Value.Name + " Male", go);
                    characters.Add(go);

                    go = Resources.Load<GameObject>(defaultCharacterPath + "Races/" + kvp.Value.femaleModelPath);
                    characterPrefabs.Add(kvp.Value.Name + " Female", go);
                    characters.Add(go);
                }
            }            
        }

        public GameObject SpawnCharacter(Transform parent, Vector3 position, PcData pc)
        {
            GameObject empty = null;

            empty = Instantiate(emptyPcPrefab, parent, false);
            //empty.transform.position = new Vector3(parent.position.x, parent.position.y + 2f, parent.position.z);

            GameObject model = Instantiate(characterPrefabs[pc.RaceKey + " " + pc.Gender], empty.transform);
            empty.GetComponent<PcController>().SetPcData(model);

            return empty;
        }
        
        public GameObject SpawnCharacter(Transform parent, Vector3 position, Gender gender, string race)
        {
            GameObject empty = null;
            empty = Instantiate(emptyPcPrefab);
            empty.transform.position = Vector3.zero;
            empty.transform.SetParent(parent);
            empty.transform.position = position;

            GameObject model = Instantiate(characterPrefabs[race + " " + gender], empty.transform, false);
            model.transform.SetParent(empty.transform);

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
                go.transform.localScale = Vector3.one;
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
                Debug.Log("Loading Hair failed. " + hairKey);
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
                Debug.Log("Loading Beard failed. " + beardKey);
                return null;
            }
        }
    }
}