using System.Collections;
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
        public FantasyName name;
        public Gender gender;

        public Species species;
        public Size size;

        public string key;
        public string raceKey;
        public string professionKey;

        public List<int> baseStart;
        public List<GameValue> BasePerLevel;
        public List<GameValue> derivedPerLevel;

        public List<int> baseResistances;
        public List<GameValue> resistancePerLevel;

        public List<int> skillStart;
        public List<GameValue> skillPerLevel;

        public int baseSpeed;
        public int minLevel;
        public int maxLevel;
        public int expPerLevel;

        public List<Ability> abilities;
        public CharacterInventory inventory;

        public string maleDefaultHair;
        public string maleDefaultBeard;
        public string femaleDefaultHair;
        public string femaleDefaultBeard;

        public int maxAccessories;

        public string description;
        public string background;

        public NPCDefinition()
        {
            name = new FantasyName();
            gender = Gender.None;
            species = Species.None;
            size = Size.None;

            key = "";
            raceKey = "";
            professionKey = "";
            description = "";
            background = "";

            maleDefaultHair = "";
            maleDefaultBeard = "";
            femaleDefaultHair = "";
            femaleDefaultBeard = "";

            maxAccessories = 1;

            description = "";
            background = "";

            minLevel = 0;
            maxLevel = 0;
            expPerLevel = 0;
            baseSpeed = 0;

            baseStart = new List<int>();
            BasePerLevel = new List<GameValue>();

            derivedPerLevel = new List<GameValue>();

            skillStart = new List<int>();
            skillPerLevel = new List<GameValue>();

            baseResistances = new List<int>();
            resistancePerLevel = new List<GameValue>();

            abilities = new List<Ability>();
            inventory = new CharacterInventory();
        }

        public NPCDefinition(FantasyName name, Species species, Size size, Gender gender, string key, string race, string profession,
            int min_level, int max_level, int exp_pre_level, int movement, string maleHair, string maleBeard, string femaleHair, string femaleBeard)
        {
            this.name = name;
            this.species = species;
            this.size = size;
            this.key = key;
            this.gender = gender;
            raceKey = race;
            professionKey = profession;
            maleDefaultHair = maleHair;
            maleDefaultBeard = maleBeard;
            femaleDefaultHair = femaleHair;
            femaleDefaultBeard = femaleBeard;

            maxAccessories = Random.Range(1, 4);

            minLevel = min_level;
            maxLevel = max_level;
            expPerLevel = exp_pre_level;
            baseSpeed = movement;

            baseStart = new List<int>();
            BasePerLevel = new List<GameValue>();
            for (int i = 0; i < (int)BaseAttribute.Number; i++)
            {
                baseStart.Add(0);
                BasePerLevel.Add(new GameValue());
            }

            derivedPerLevel = new List<GameValue>();
            for (int i = 0; i < (int)DerivedAttribute.Number; i++)
            {
                derivedPerLevel.Add(new GameValue());
            }

            skillStart = new List<int>();
            skillPerLevel = new List<GameValue>();
            for (int i = 0; i < (int)Skill.Number; i++)
            {
                skillStart.Add(0);
                skillPerLevel.Add(new GameValue());
            }

            baseResistances = new List<int>();
            resistancePerLevel = new List<GameValue>();
            for (int i = 0; i < (int)DamageType.Number; i++)
            {
                baseResistances.Add(0);
                resistancePerLevel.Add(new GameValue());
            }

            abilities = new List<Ability>();
            inventory = new CharacterInventory();
        }

        public void CalculateDerived()
        {
            for (int i = 0; i < (int)DerivedAttribute.Number; i++)
            {

            }
        }

        public NpcData ConvertToCharacter()
        {
            NpcData npc = new NpcData(name, key, gender, raceKey, professionKey, -1, -1, -1, -1, -1);

            return npc;
        }
    }
}