using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Reclamation.Characters;
using Reclamation.World;

namespace Reclamation.Gui.World
{
    public class PartyPanel : MonoBehaviour
    {
        public GameObject[] pcElements = new GameObject[6];
        public GameObject pcElementPrefab;
        public Transform pcElementParent;

        ListData listData = new ListData();

        public void Initialize()
        {
            listData.SetData(pcElementPrefab, pcElementParent);

            PartyData partyData = PlayerManager.instance.parties[0].GetComponentInChildren<PartyController>().PartyData;

            if (partyData != null)
            {
                for (int i = 0; i < PartyData.MaxPartySize; i++)
                {
                    pcElements[i].GetComponent<PartyPcElement>().SetData(partyData.pcs[i]);
                }
            }
        }
    }

    public class ListData
    {
        public List<GameObject> gameObjects;
        public GameObject prefab;
        public Transform parent;

        public ListData()
        {
            gameObjects = new List<GameObject>();
            prefab = null;
            parent = null;
        }

        public void SetData(GameObject prefab, Transform parent)
        {
            this.prefab = prefab;
            this.parent = parent;
        }
    }
}