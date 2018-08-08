using Reclamation.Characters;
using Reclamation.Gui.World;
using Reclamation.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reclamation.Gui
{
    public class BarracksScreen : GameScreen
    {
        [SerializeField] BarracksListPanel listPanel;
        [SerializeField] BarracksDetailsPanel detailsPanel;
        [SerializeField] BarracksBaseAttributesPanel baseAttributesPanel;
        [SerializeField] BarracksDerivedAttributesPanel derivedAttributesPanel;

        public override void Initialize()
        {
            //Debug.Log("BarracksScreen Initialized");
            base.Initialize();

            listPanel.Initialize(this);
            detailsPanel.Initialize(this);
            baseAttributesPanel.Initialize(this);
            derivedAttributesPanel.Initialize(this);
        }

        public override void Open()
        {
            base.Open();
        }

        public override void Close()
        {
            base.Close();
        }

        public bool SelectPc(int index)
        {
            PcData pcData = PlayerManager.instance.GetPcData(index);
            detailsPanel.SetPcData(pcData);
            baseAttributesPanel.SetPcData(pcData);
            derivedAttributesPanel.SetPcData(pcData);

            return true;
        }
    }
}