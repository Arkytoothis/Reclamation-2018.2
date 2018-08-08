using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Reclamation.Characters;
using Reclamation.Encounter;
using Reclamation.Misc;

namespace Reclamation.Gui.Encounter
{
    public class CharacterScreen : MonoBehaviour
    {
        public TMP_Text detailsLabel;
        public TMP_Text baseAttributesLabel;
        public TMP_Text derivedAttributesLabel;
        public TMP_Text skillsLabel;
        public TMP_Text resistancesLabel;
        public bool isOpen = false;

        private int pcIndexToView = 0;
        private PcData pc;

        public void Initialize()
        {
            SetData(EncounterManager.instance.GetPcData(0));
            UpdateData();
            Close();
        }

        public void UpdateData()
        {
            detailsLabel.text = pc.Name.FullName + ", Lvl " + pc.Level + " " + pc.RaceKey + " " + pc.ProfessionKey + "\n";

            string s = "";
            AttributeDefinition definition = null;
            Attribute attribute = null;

            for (int i = 0; i < (int)BaseAttribute.Number; i++)
            {
                definition = Database.GetBaseAttribute(i);
                attribute = pc.GetBase(i);

                s += definition.Name + "<pos=200>" + attribute.Current + "/" + attribute.Maximum + "\n";
            }

            baseAttributesLabel.text = s;
            s = "";

            for (int i = 0; i < (int)DerivedAttribute.Number; i++)
            {
                definition = Database.GetDerivedAttribute(i);
                attribute = pc.GetDerived(i);

                if (definition.Type == AttributeDefinitionType.Derived_Percent)
                    s += definition.Name + "<pos=200>" + attribute.Current + "%\n";
                else if (definition.Type == AttributeDefinitionType.Derived_Points)
                    s += definition.Name + "<pos=200>" + attribute.Current + "/" + attribute.Maximum + "\n";
                else if (definition.Type == AttributeDefinitionType.Derived_Score)
                    s += definition.Name + "<pos=200>" + attribute.Current + "\n";
            }

            derivedAttributesLabel.text = s;
            s = "";

            for (int i = 0; i < (int)DamageType.Number; i++)
            {
                definition = Database.GetDamageType(i);
                attribute = pc.GetResistance(i);

                s += definition.Name + "<pos=200>" + attribute.Current + "\n";
            }

            resistancesLabel.text = s;
            s = "";

            SkillDefinition skillDef = null;
            foreach (KeyValuePair<Skill, Attribute> kvp in pc.GetSkills())
            {
                skillDef = Database.GetSkill(kvp.Value.Index);
                Attribute skill = pc.GetSkill(kvp.Key);

                s += skillDef.Name + "<pos=200>" + skill.Current + "\n";
            }

            skillsLabel.text = s;
        }

        public void SetData(PcData pc)
        {
            if (pc == null)
            {
                Debug.LogError("pc == null");
                return;
            }

            this.pc = pc;
        }

        public void NextPc()
        {
            pcIndexToView++;
            if (pcIndexToView > EncounterManager.instance.PcsCount - 1)
                pcIndexToView = 0;

            SetData(EncounterManager.instance.GetPcData(pcIndexToView));
            UpdateData();
        }

        public void PreviousPc()
        {
            pcIndexToView--;
            if (pcIndexToView < 0)
                pcIndexToView = EncounterManager.instance.PcsCount - 1;

            SetData(EncounterManager.instance.GetPcData(pcIndexToView));
            UpdateData();
        }

        public void Toggle()
        {
            isOpen = !isOpen;

            if (isOpen == true) Open();
            else Close();
        }
        public void Open()
        {
            EncounterCursor.instance.isEnabled = false;
            isOpen = true;
            transform.localPosition = new Vector3(0, transform.localPosition.y, transform.localPosition.z);
        }

        public void Close()
        {
            EncounterCursor.instance.isEnabled = true;
            isOpen = false;
            transform.localPosition = new Vector3(-10000, transform.localPosition.y, transform.localPosition.z);
        }
    }
}