using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Reclamation.Characters;

namespace Reclamation.Gui.World
{
    public class BarracksDetailsPanel : Panel
    {
        [SerializeField] TMP_Text nameLabel;
        [SerializeField] TMP_Text detailsLabel;

        public override void Initialize(GameScreen screen)
        {
            base.Initialize(screen);
        }

        public void SetPcData(PcData pcData)
        {
            if (pcData != null)
            {
                nameLabel.text = pcData.Name.ShortName;
                detailsLabel.text = "Level " + pcData.Level + " " + pcData.RaceKey + " " + pcData.ProfessionKey;
            }
        }
    }
}