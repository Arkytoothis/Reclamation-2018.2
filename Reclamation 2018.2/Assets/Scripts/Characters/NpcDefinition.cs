﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using Reclamation.Abilities;
using Reclamation.Misc;
using Reclamation.Name;

namespace Reclamation.Characters
{
    public enum Species { Animal, Beast, Undead, Humanoid, Elemental, Draconic, Insect, Number, None }
    public enum Size { Miniscule, Tiny, Small, Medium, Large, Huge, Gigantic, Number, None }

    [System.Serializable]
    public class NPCDefinition
    {
        public FantasyName Name;
        public Gender Gender;

        public Species Species;
        public Size Size;

        public string Key;
        public string RaceKey;
        public string ProfessionKey;

        public List<int> BaseStart;
        public List<GameValue> BasePerLevel;
        public List<GameValue> DerivedPerLevel;

        public List<int> BaseResistances;
        public List<GameValue> ResistancePerLevel;

        public List<int> SkillStart;
        public List<GameValue> SkillPerLevel;

        public int BaseSpeed;
        public int MinLevel;
        public int MaxLevel;
        public int ExpPerLevel;

        public List<Ability> Abilities;
        public CharacterInventory Inventory;

        public int Hair;
        public int Beard;

        public int MaxAccessories;

        public string Description;
        public string Background;

        public NPCDefinition()
        {
            Name = new FantasyName();
            Gender = Gender.None;
            Species = Species.None;
            Size = Size.None;

            Key = "";
            RaceKey = "";
            ProfessionKey = "";
            Description = "";
            Background = "";

            Hair = -1;
            Beard = -1;
            MaxAccessories = 1;

            Description = "";
            Background = "";

            MinLevel = 0;
            MaxLevel = 0;
            ExpPerLevel = 0;
            BaseSpeed = 0;

            BaseStart = new List<int>();
            BasePerLevel = new List<GameValue>();

            DerivedPerLevel = new List<GameValue>();

            SkillStart = new List<int>();
            SkillPerLevel = new List<GameValue>();

            BaseResistances = new List<int>();
            ResistancePerLevel = new List<GameValue>();

            Abilities = new List<Ability>();
            Inventory = new CharacterInventory();
        }

        public NPCDefinition(FantasyName name, Species species, Size size, Gender gender, string key, string race, string profession, int hair, int beard,
            int min_level, int max_level, int exp_pre_level, int movement)
        {
            Name = name;
            Species = species;
            Size = size;
            Key = key;
            Gender = gender;
            RaceKey = race;
            ProfessionKey = profession;
            Hair = hair;
            Beard = beard;
            MaxAccessories = Random.Range(1, 4);

            MinLevel = min_level;
            MaxLevel = max_level;
            ExpPerLevel = exp_pre_level;
            BaseSpeed = movement;

            BaseStart = new List<int>();
            BasePerLevel = new List<GameValue>();
            for (int i = 0; i < (int)BaseAttribute.Number; i++)
            {
                BaseStart.Add(0);
                BasePerLevel.Add(new GameValue());
            }

            DerivedPerLevel = new List<GameValue>();
            for (int i = 0; i < (int)DerivedAttribute.Number; i++)
            {
                DerivedPerLevel.Add(new GameValue());
            }

            SkillStart = new List<int>();
            SkillPerLevel = new List<GameValue>();
            for (int i = 0; i < (int)Skill.Number; i++)
            {
                SkillStart.Add(0);
                SkillPerLevel.Add(new GameValue());
            }

            BaseResistances = new List<int>();
            ResistancePerLevel = new List<GameValue>();
            for (int i = 0; i < (int)DamageType.Number; i++)
            {
                BaseResistances.Add(0);
                ResistancePerLevel.Add(new GameValue());
            }

            Abilities = new List<Ability>();
            Inventory = new CharacterInventory();
        }

        public void CalculateDerived()
        {
            for (int i = 0; i < (int)DerivedAttribute.Number; i++)
            {

            }
        }

        public NPC ConvertToCharacter()
        {
            NPC npc = new NPC(Name, Key, Gender, RaceKey, ProfessionKey, Hair, Beard, -1, -1, -1, -1, -1);

            return npc;
        }
    }
}