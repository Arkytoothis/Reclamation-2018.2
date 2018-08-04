using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reclamation.Abilities;
using Reclamation.Characters;
using TMPro;

namespace Reclamation.Gui.Encounter
{
    public class CharacterPanel : MonoBehaviour
    {
        [SerializeField] TMP_Text nameLabel;
        [SerializeField] GameObject actionButtonPrefab;
        [SerializeField] List<ActionButton> actionButtons;
        [SerializeField] Transform actionButtonsParent;

        [SerializeField] PcData pcData;

        const int MAX_ACTION_BUTTONS = 12;
        public void Initialize()
        {
            for (int i = 0; i < 12; i++)
            {
                GameObject go = Instantiate(actionButtonPrefab, actionButtonsParent);
                ActionButton actionButton = go.GetComponent<ActionButton>();
                actionButton.SetData(i, null);
                actionButtons.Add(actionButton);
            }
        }

        public void SetData(PcData pcData)
        {
            if (pcData != null)
            {
                this.pcData = pcData;

                nameLabel.text = this.pcData.name.FullName;
                LoadAbilities();
            }
        }

        public void LoadAbilities()
        {
            if (pcData == null)
            {
                Debug.Log("pcData == null");
                return;
            }

            for (int i = 0; i < MAX_ACTION_BUTTONS; i++)
            {
                actionButtons[i].SetData(null);
            }

            int buttonIndex = 0;
            for (int i = 0; i < pcData.abilities.AvailablePowers.Count; i++)
            {
                //Debug.Log("Adding action button for " + pcData.abilities.AvailablePowers[i].GetName());
                actionButtons[buttonIndex].SetData(pcData.abilities.AvailablePowers[i]);
                buttonIndex++;
            }

            for (int i = 0; i < pcData.abilities.AvailableSpells.Count; i++)
            {
                //Debug.Log("Adding action button for " + pcData.abilities.AvailableSpells[i].GetName());
                actionButtons[buttonIndex].SetData(pcData.abilities.AvailableSpells[i]);
                buttonIndex++;
            }
        }
    }
}