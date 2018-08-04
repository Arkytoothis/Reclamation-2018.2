using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reclamation.Characters;
using Reclamation.Encounter;
using TMPro;

namespace Reclamation.Gui.Encounter
{
    public class PartyPanel : MonoBehaviour
    {
        public GameObject pcElementPrefab;
        public List<GameObject> pcButtons;
        public Transform pcElementsParent;
        public TMP_Text label;

        public void Initialize()
        {
            for (int i = 0; i < EncounterManager.instance.PcsCount; i++)
            {
                GameObject go = Instantiate(pcElementPrefab, pcElementsParent);
                PcButton button = go.GetComponent<PcButton>();
                button.SetData(EncounterManager.instance.GetPcData(i));

                pcButtons.Add(go);
            }
        }

        public void UpdateLabel(int layer)
        {
            label.text = "Layer " + layer;
        }
	}
}