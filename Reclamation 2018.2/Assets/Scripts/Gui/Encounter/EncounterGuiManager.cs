using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reclamation.Encounter;
using Reclamation.Misc;

namespace Reclamation.Gui.Encounter
{
    public class EncounterGuiManager : Singleton<EncounterGuiManager>
    {
        public PartyPanel partyPanel;
        public CharacterViewer characterViewer;
        public CharacterPanel characterPanel;

        public void Initialize()
        {
            partyPanel.Initialize();
            characterViewer.Initialize();
            characterPanel.Initialize();
            characterPanel.SetData(EncounterManager.instance.GetSelectedPcData());
        }

        void Update()
        {
            if (Input.GetKeyUp(KeyCode.C))
            {
                characterViewer.Toggle();
            }
        }
    }
}