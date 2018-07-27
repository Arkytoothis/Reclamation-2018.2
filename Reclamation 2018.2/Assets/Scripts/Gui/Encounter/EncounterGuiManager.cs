using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reclamation.Misc;

namespace Reclamation.Gui.Encounter
{
    public class EncounterGuiManager : Singleton<EncounterGuiManager>
    {
        public PartyPanel partyPanel;

        public void Initialize()
        {
            partyPanel.Initialize();
        }
    }
}