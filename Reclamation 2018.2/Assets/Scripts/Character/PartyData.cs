using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization;

public enum PartyStatus
{
    Adventuring, Combat, Resting, Idle, Number, None
}

[System.Serializable]
public class PartyData
{
    public const int MaxPartySize = 6;
    public PartyStatus status;
    public string name;
    public int index;

    public Color color;

    public int maxCharacters;
    public Pc[] pcs;

    public List<Characteristic> attributes;
    public List<int> combinedSkills;
    public List<CharacterSkillPair> highestSkills;

    public PartyData()
    {
        name = "New Party";
    }

    public PartyData(string name, Color color, int index, int maxCharacters)
    {
        status = PartyStatus.Idle;

        this.name = name;
        this.color = color;
        this.index = index;
        this.maxCharacters = maxCharacters;

        pcs = new Pc[MaxPartySize];

        for (int i = 0; i < MaxPartySize; i++)
        {
            pcs[i] = null;
        }

        attributes = new List<Characteristic>();
        for (int i = 0; i < (int)PartyAttribute.Number; i++)
        {
            attributes.Add(new Characteristic(CharacteristicType.Party_Attribute, i, 0));
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

    public PartyData(PartyData data)
    {
        status = PartyStatus.Idle;

        name = data.name;
        color = data.color;
        index = data.index;
        maxCharacters = data.maxCharacters;

        pcs = new Pc[MaxPartySize];

        for (int i = 0; i < MaxPartySize; i++)
        {
            AddPc(data.pcs[i]);
        }

        attributes = new List<Characteristic>();
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

    public void AddPc(Pc pc)
    {
        if (pc == null)
        {
            //Debug.Log("pc == null");
            return;
        }

        for (int i = 0; i < pcs.Length; i++)
        {
            if (pcs[i] == null)
            {
                pcs[i] = new Pc(pc);
                //Debug.Log(pcs[i].Name.ShortName + " added");
                break;
            }
        }
    }

    public int CurrentCharacters()
    {
        int num = 0;

        //for (int i = 0; i < characters.Count; i++)
        //{
        //    if (characters[i] != -1)
        //        num++;
        //}

        return num;
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