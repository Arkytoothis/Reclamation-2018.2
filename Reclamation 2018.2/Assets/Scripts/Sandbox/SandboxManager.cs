using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Reclamation.Misc;
using Reclamation.Equipment;
using Reclamation.Characters;

namespace Reclamation.Sandbox
{
    public class SandboxManager : Singleton<SandboxManager>
    {
        [SerializeField] SandboxGuiManager guiManager;
        [SerializeField] Transform characterMount;

        [SerializeField] CharacterRenderer characterRenderer;

        public Transform characterModelsParent;
        public Transform hairModelsParent;
        public Transform beardModelsParent;

        Dictionary<string, GameObject> characterModels;
        Dictionary<string, GameObject> hairModels;
        Dictionary<string, GameObject> beardModels;

        void Awake()
        {
            Database.Initialize();
            ItemGenerator.Initialize();
            PcGenerator.Initialize();
            ModelManager.instance.Initialize();

            characterModels = new Dictionary<string, GameObject>();
            hairModels = new Dictionary<string, GameObject>();
            beardModels = new Dictionary<string, GameObject>();

            LoadModels();
        }

        void Start()
        {
            Invoke(nameof(Initialize), 0.1f);
        }

        void Initialize()
        {
            guiManager.Initialize();
        }

        public void LoadModels()
        {
            foreach (KeyValuePair<string, GameObject> kvp in ModelManager.instance.CharacterPrefabs)
            {
                GameObject go = Instantiate(kvp.Value, characterModelsParent);
                characterModels.Add(kvp.Key, go);
            }

            foreach (KeyValuePair<string, GameObject> kvp in ModelManager.instance.HairPrefabs)
            {
                GameObject go = Instantiate(kvp.Value, hairModelsParent);
                hairModels.Add(kvp.Key, go);
            }

            foreach (KeyValuePair<string, GameObject> kvp in ModelManager.instance.BeardPrefabs)
            {
                GameObject go = Instantiate(kvp.Value, beardModelsParent);
                beardModels.Add(kvp.Key, go);
            }
        }

        public void SetBodyModel(string key)
        {
            if (characterMount.childCount > 0)
            {
                characterMount.GetChild(0).transform.position = characterModels[key].transform.position;
                characterMount.GetChild(0).SetParent(characterModels[key].transform.parent);
            }

            characterModels[key].transform.position = characterMount.position;
            characterModels[key].transform.SetParent(characterMount);

            if (characterMount.childCount > 0)
            {
                characterRenderer = characterMount.GetChild(0).gameObject.GetComponent<CharacterRenderer>();
                characterMount.GetChild(0).gameObject.transform.rotation = characterMount.rotation;
            }
        }

        public void SetHair(GameObject hair)
        {
            characterRenderer.SetHair(hair);
        }

        public void SetBeard(GameObject beard)
        {
            characterRenderer.SetBeard(beard);
        }
    }
}