using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reclamation.Encounter;
using Reclamation.Misc;

namespace Reclamation.Characters
{
    public enum AttributeListType { Base, Derived, Resistance, Number, None };

    [System.Serializable]
    public class AttributeManager
    {
        private AttributeList[] lists;
        private Dictionary<Skill, Attribute> skills;

        public AttributeManager()
        {
            lists = new AttributeList[(int)AttributeListType.Number];

            for (int i = 0; i < lists.Length; i++)
            {
                lists[i] = new AttributeList();
            }

            skills = new Dictionary<Skill, Attribute>();
        }

        public void AddAttribute(AttributeListType listType, Attribute attribute)
        {
            lists[(int)listType].Attributes.Add(attribute);
        }

        public Attribute GetAttribute(AttributeListType listType, int attribute)
        {
            return lists[(int)listType].Attributes[attribute];
        }

        public int GetAttributeValue(AttributeListType listType, AttributeComponentType type, int attribute)
        {
            return lists[(int)listType].Attributes[attribute].Get(type);
        }

        public void SetStart(AttributeListType listType, int attribute, int value, int min, int max)
        {
            lists[(int)listType].Attributes[attribute].SetStart(value, min, max);
        }

        public void SetMaximum(AttributeListType listType, int attribute, int value)
        {
            lists[(int)listType].Attributes[attribute].SetMax(value, true);
        }

        public void AddSkill(Skill key, Attribute skill)
        {
            if (skills.ContainsKey(key) == false)
                skills.Add(key, skill);
            else
                skills[key].Current += skill.Current;
        }

        public Attribute GetSkill(Skill skill)
        {
            if (skills.ContainsKey(skill) == true)
            {
                return skills[skill];
            }
            else
                return null;
        }

        public Dictionary<Skill, Attribute> GetSkills()
        {
            return skills;
        }

        public int GetSkillValue(AttributeComponentType type, Skill skill)
        {
            if (skills.ContainsKey(skill) == true)
            {
                return skills[skill].Get(type);
            }
            else
                return 0;
        }

        public void ModifyAttribute(AttributeType type, int attribute, int value)
        {
            switch (type)
            {
                case AttributeType.Base:
                    lists[(int)AttributeListType.Base].Attributes[attribute].Modify(AttributeComponentType.Current, value);
                    break;
                case AttributeType.Derived:
                    lists[(int)AttributeListType.Derived].Attributes[attribute].Modify(AttributeComponentType.Current, value);
                    break;
                case AttributeType.Resistance:
                    lists[(int)AttributeListType.Resistance].Attributes[attribute].Modify(AttributeComponentType.Current, value);
                    break;
                case AttributeType.Skill:
                    //lists[(int)AttributeListType.ski].Attributes[attribute].Current += value;
                    break;
                default:
                    break;
            }
        }
    }
}