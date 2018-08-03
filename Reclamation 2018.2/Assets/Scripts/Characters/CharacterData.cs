using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reclamation.Misc;
using Reclamation.Name;

namespace Reclamation.Characters
{
    [System.Serializable]
    public abstract class CharacterData
    {
        public FantasyName name;
        public Gender gender;
        public Background background;
        public CharacterPersonality personality;
        public string faction;

        public Species species;
        public Size size;

        public string raceKey;
        public string professionKey;

        public AttributeManager attributeManager;

        public string hair;
        public string beard;

        public string description;

        public CharacterInventory inventory;
        public Vector3 position;

        public bool isDead = false;
        public bool isExhausted = false;
        public bool isDrained = false;
        public bool isBroken = false;

        public void SetStart(AttributeType type, int attribute, int start, int min, int max)
        {
            attributeManager.SetStart((AttributeListType)type, attribute, start, min, max);
        }

        public Attribute GetBase(int attribute)
        {
            return attributeManager.GetAttribute(AttributeListType.Base, attribute);
        }

        public Attribute GetDerived(int attribute)
        {
            return attributeManager.GetAttribute(AttributeListType.Derived, attribute);
        }

        public Attribute GetSkill(Skill skill)
        {
            return attributeManager.GetSkill(skill);
        }

        public Dictionary<Skill, Attribute> GetSkills()
        {
            return attributeManager.GetSkills();
        }

        public Attribute GetResistance(int attribute)
        {
            return attributeManager.GetAttribute(AttributeListType.Resistance, attribute);
        }

        public void CalculateStartAttributes(bool randomize)
        {
            AttributeDefinition definition = null;
            int start = 10;

            for (int i = 0; i < (int)BaseAttribute.Number; i++)
            {
                definition = Database.GetBaseAttribute(i);

                attributeManager.SetStart(AttributeListType.Base, i, start, definition.Minimum, start);
            }

            if (randomize == true)
            {
                List<int> rolls = new List<int>((int)BaseAttribute.Number);

                for (int i = 0; i < (int)BaseAttribute.Number; i++)
                {
                    rolls.Add(Random.Range(5, 21));
                }

                if (Database.Professions[professionKey].AttributePriorities.Count > 0)
                {
                    rolls.Sort();
                    rolls.Reverse();
                    int total = 0;

                    for (int i = 0; i < (int)BaseAttribute.Number; i++)
                    {
                        total = rolls[(int)Database.Professions[professionKey].AttributePriorities[i]];
                        total += Database.GetRace(raceKey).StartingAttributes[i].Number;

                        if (Database.Professions[professionKey].MinimumAttributes[i] > 0 &&
                            total < Database.Professions[professionKey].MinimumAttributes[i])
                            total = Database.Professions[professionKey].MinimumAttributes[i];

                        attributeManager.SetStart(AttributeListType.Base, i, total, 0, total);
                    }
                }
                else
                {
                    int total = 0;

                    for (int i = 0; i < (int)BaseAttribute.Number; i++)
                    {
                        total = rolls[i];
                        total += Database.GetRace(raceKey).StartingAttributes[i].Number;

                        if (Database.Professions[professionKey].MinimumAttributes[i] > 0 &&
                            total < Database.Professions[professionKey].MinimumAttributes[i])
                            total = Database.Professions[professionKey].MinimumAttributes[i];

                        attributeManager.SetStart(AttributeListType.Base, i, total, 0, total);
                    }
                }
            }

            for (int i = 0; i < (int)BaseAttribute.Number; i++)
            {
                int total = attributeManager.GetAttributeValue(AttributeListType.Base, AttributeComponentType.Start, i) + attributeManager.GetAttributeValue(AttributeListType.Base, AttributeComponentType.Modifier, i);
                attributeManager.SetStart(AttributeListType.Base, i, total, 0, total);
            }
        }

        public void CalculateDerivedAttributes()
        {
            AttributeDefinition definition = null;
            int start = 0;
            for (int i = 0; i < (int)DerivedAttribute.Number; i++)
            {
                definition = Database.GetDerivedAttribute(i);

                if (definition.Calculation.Attribute1 != null)
                {
                    if (definition.Calculation.Attribute1.Type == AttributeModifierType.Base_Attribute)
                    {
                        start = GetBase(definition.Calculation.Attribute1.Attribute).Current;
                    }
                    else if (definition.Calculation.Attribute1.Type == AttributeModifierType.Race)
                    {
                        start = Database.GetRace(raceKey).StartingAttributes[definition.Calculation.Attribute1.Attribute].Roll(false);
                    }
                    else if (definition.Calculation.Attribute1.Type == AttributeModifierType.Value)
                    {
                        start = definition.Calculation.Attribute1.Attribute;
                    }
                }

                if (definition.Calculation.Attribute2 != null)
                {
                    if (definition.Calculation.Attribute2.Type == AttributeModifierType.Base_Attribute)
                    {
                        if (definition.Calculation.Operator1 == AttributeCalculationOpperator.Add)
                        {
                            start += GetBase(definition.Calculation.Attribute2.Attribute).Current;
                        }
                        else if (definition.Calculation.Operator1 == AttributeCalculationOpperator.Subtract)
                        {
                            start -= GetBase(definition.Calculation.Attribute2.Attribute).Current;
                        }
                    }
                    else if (definition.Calculation.Attribute2.Type == AttributeModifierType.Value)
                    {
                        if (definition.Calculation.Operator1 == AttributeCalculationOpperator.Add)
                        {
                            start += definition.Calculation.Attribute2.Attribute;
                        }
                        else if (definition.Calculation.Operator1 == AttributeCalculationOpperator.Subtract)
                        {
                            start -= definition.Calculation.Attribute2.Attribute;
                        }
                    }
                }

                if (start < 0) start = 0;

                attributeManager.SetStart(AttributeListType.Derived, i, start, definition.Minimum, start);
            }
        }

        public virtual void CalculateStartSkills()
        {
            //for (int i = 0; i < (int)Skill.Number; i++)
            //{
            //    attributeManager.SetStart(AttributeListType.Skill, i, 0, 0, 100);
            //}

            //int result = 0;
            //for (int i = 0; i < Database.GetProfession(ProfessionKey).SkillProficiencies.Count; i++)
            //{
            //    int skill = (int)Database.GetProfession(ProfessionKey).SkillProficiencies[i].Skill;
            //    int value = Database.GetProfession(ProfessionKey).SkillProficiencies[i].Value;
            //    result = GameValue.Roll(new GameValue(1, 2), false) * value;
            //    attributeManager.SetStart(AttributeListType.Skill, i, result, 0, 100);
            //}

            //for (int i = 0; i < Database.GetRace(RaceKey).SkillProficiencies.Count; i++)
            //{
            //    int skill = (int)Database.GetRace(RaceKey).SkillProficiencies[i].Skill;
            //    result += Database.GetRace(RaceKey).SkillProficiencies[i].Value;
            //}

            //for (int i = 0; i < (int)Skill.Number; i++)
            //{
            //}
        }

        public void CalculateResistances()
        {
            for (int i = 0; i < Database.Races[raceKey].Resistances.Count; i++)
            {
                //int resistance = (int)Database.Races[RaceKey].Resistances[i].DamageType;
                int value = Database.Races[raceKey].Resistances[i].Value;
                //Resistances[resistance].SetStart(value, 0, 100);
                attributeManager.SetStart(AttributeListType.Resistance, i, value, 0, 100);
            }
        }

        
    }
}