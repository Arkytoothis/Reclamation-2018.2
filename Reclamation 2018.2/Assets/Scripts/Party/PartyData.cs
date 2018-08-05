using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Reclamation.Misc;
using Reclamation.Characters;

namespace Reclamation.Party
{
    public enum PartyStatus
    {
        Adventuring, Combat, Resting, Idle, Number, None
    }

    [System.Serializable]
    public class PartyData : MonoBehaviour
    {
        public const int MaxPartySize = 6;
        [SerializeField] PartyStatus status;
        [SerializeField] new string name;
        [SerializeField] int index;

        [SerializeField] Color color;

        [SerializeField] List<Attribute> attributes;
        [SerializeField] List<int> combinedSkills;
        [SerializeField] List<CharacterSkillPair> highestSkills;

        public PartyData()
        {
            name = "New Party";
        }

        public void SetPartyData(string name, Color color, int index)
        {
            status = PartyStatus.Idle;

            this.name = name;
            this.color = color;
            this.index = index;

            attributes = new List<Attribute>();
            for (int i = 0; i < (int)PartyAttribute.Number; i++)
            {
                attributes.Add(new Attribute(AttributeType.Party, i, 0));
            }

            combinedSkills = new List<int>();
            for (int i = 0; i < (int)Skill.Number; i++)
            {
                combinedSkills.Add(0);
            }

            highestSkills = new List<CharacterSkillPair>();
            for (int i = 0; i < (int)Skill.Number; i++)
            {
                highestSkills.Add(new CharacterSkillPair(-1, 0));
            }
        }

        public void SetPartyData(PartyData data)
        {
            status = PartyStatus.Idle;

            name = data.name;
            color = data.color;
            index = data.index;

            attributes = new List<Attribute>();
            for (int i = 0; i < (int)PartyAttribute.Number; i++)
            {
                attributes.Add(data.attributes[i]);
            }

            combinedSkills = new List<int>();
            for (int i = 0; i < (int)Skill.Number; i++)
            {
                combinedSkills.Add(data.combinedSkills[i]);
            }

            highestSkills = new List<CharacterSkillPair>();
            for (int i = 0; i < (int)Skill.Number; i++)
            {
                highestSkills.Add(data.highestSkills[i]);
            }
        }

        public void UpdateCombinedSkills()
        {
            //for (int pc = 0; pc < PlayerManager.Instance.GetCharacters().Count; pc++)
            //{
            //    if (PlayerManager.Instance.GetCharacter(pc) != null)
            //    {
            //        for (int i = 0; i < (int)Skill.Number; i++)
            //        {
            //            CombinedSkills[i] += PlayerManager.Instance.GetCharacter(pc).Skills[i].Current;
            //        }
            //    }
            //    else
            //    {
            //        Debug.Log("PlayerManager.GetCharacter(pc) == null");
            //    }
            //}
        }

        public void UpdateHighestSkills()
        {
            //for (int pcIndex = 0; pcIndex < Characters.Count; pcIndex++)
            //{
            //    if (Characters[pcIndex] != -1)
            //    {
            //        PC pc = PlayerManager.Instance.GetCharacter(Characters[pcIndex]);

            //        if (pc != null)
            //        {
            //            for (int i = 0; i < (int)Skill.Number; i++)
            //            {
            //                if (pc.Skills[i].Current > HighestSkills[i].SkillValue)
            //                {
            //                    HighestSkills[i].CharacterIndex = Characters[pcIndex];
            //                    HighestSkills[i].SkillValue = pc.Skills[i].Current;
            //                }
            //            }
            //        }
            //        else
            //        {
            //            Debug.Log("PlayerManager.GetCharacter(pc) == null");
            //        }
            //    }
            //}

            //foreach (KeyValuePair<string, CharacterSkillPair> kvp in HighestSkills)
            //{
            //    if (kvp.Value.CharacterIndex != -1)
            //    {
            //        PlayerCharacter pc = PlayerManager.GetCharacter(kvp.Value.CharacterIndex);
            //    }
            //}
        }

        public int GetCombinedSkill(int index)
        {
            return combinedSkills[index];
        }

        public CharacterSkillPair GetHighestSkill(int index)
        {
            return new CharacterSkillPair(highestSkills[index].CharacterIndex, highestSkills[index].SkillValue);
        }

        public CharacterSkillPair GetLeaderSkill(int index)
        {
            return new CharacterSkillPair(0, highestSkills[index].SkillValue);
        }
    }
}