using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Reclamation.Characters;
using Reclamation.Encounter;
using Reclamation.Misc;

namespace Reclamation.Gui.Encounter
{
    public class CharacterViewer : MonoBehaviour
    {
        public TMP_Text detailsLabel;
        public TMP_Text baseAttributesLabel;
        public TMP_Text derivedAttributesLabel;
        public TMP_Text skillsLabel;
        public TMP_Text resistancesLabel;

        private int currentPc = 0;

        public void Initialize()
        {
            SetData(EncounterManager.instance.parties[0].pcs[0]);
        }

        public void SetData(Pc pc)
        {
            if (pc == null)
            {
                Debug.LogError("pc == null");
                return;
            }

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

                if(definition.Type == AttributeDefinitionType.Derived_Percent)
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

                s += kvp.Key + "<pos=200>" + skill.Current + "\n";
            }

            skillsLabel.text = s;
        }

        public void NextPc()
        {
            Debug.Log("Next Pc");
        }

        public void PreviousPc()
        {
            Debug.Log("Previous Pc");
        }
    }
}