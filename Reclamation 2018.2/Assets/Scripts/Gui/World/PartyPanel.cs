using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Reclamation.Party;
using Reclamation.World;

namespace Reclamation.Gui.World
{
    public class PartyPanel : MonoBehaviour
    {
        [SerializeField] GameObject[] pcElements = new GameObject[6];
        [SerializeField] GameObject pcElementPrefab;
        [SerializeField] Transform pcElementParent;

        [SerializeField] ListData listData = new ListData();

        public void Initialize()
        {
            listData.SetData(pcElementPrefab, pcElementParent);

            PartyData partyData = PlayerManager.instance.GetPartyData(0);

            if (partyData != null)
            {
                for (int i = 0; i < PartyData.MaxPartySize; i++)
                {
                    pcElements[i].GetComponent<PartyPcElement>().SetData(null);
                }
            }
        }
    }

    [System.Serializable]
    public class ListData
    {
        [SerializeField] List<GameObject> gameObjects;
        [SerializeField] GameObject prefab;
        [SerializeField] Transform parent;

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