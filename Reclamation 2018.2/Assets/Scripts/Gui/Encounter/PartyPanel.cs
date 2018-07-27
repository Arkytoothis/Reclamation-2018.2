using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reclamation.Encounter;

namespace Reclamation.Gui.Encounter
{
    public class PartyPanel : MonoBehaviour
    {
        public GameObject pcElementPrefab;
        public List<GameObject> pcElements;
        public Transform pcElementsParent;

        public void Initialize()
        {
            for (int i = 0; i < 6; i++)
            {
                GameObject go = Instantiate(pcElementPrefab, pcElementsParent);
                PcElement element = go.GetComponent<PcElement>();
                element.SetData(EncounterManager.instance.parties[0].pcs[i]);
            }
        }
	}
}