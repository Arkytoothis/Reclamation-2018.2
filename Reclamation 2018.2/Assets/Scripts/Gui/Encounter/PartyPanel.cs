using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reclamation.Encounter;

namespace Reclamation.Gui.Encounter
{
    public class PartyPanel : MonoBehaviour
    {
        public GameObject pcElementPrefab;
        public List<GameObject> pcButtons;
        public Transform pcElementsParent;

        public void Initialize()
        {
            for (int i = 0; i < 6; i++)
            {
                GameObject go = Instantiate(pcElementPrefab, pcElementsParent);
                FlexiblePcButton button = go.GetComponent<FlexiblePcButton>();
                button.SetData(EncounterManager.instance.parties[0].pcs[i]);

                pcButtons.Add(go);
            }

            InvokeRepeating("UpdatePcs", 0.1f, 0.1f);
        }

        public void UpdatePcs()
        {
            for (int i = 0; i < 6; i++)
            {
                pcButtons[i].GetComponent<FlexiblePcButton>().UpdateData();
            }
        }
	}
}