﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reclamation.Abilities;
using Reclamation.Misc;

namespace Reclamation.Characters
{
    [System.Serializable]
    public class CharacterAbilities
    {
        [System.NonSerialized]
        private PcData pc;

        private List<Ability> traits;
        private List<Ability> availablePowers;
        private List<Ability> knownPowers;
        private List<Ability> memorizedPowers;
        private List<Ability> availableSpells;
        private List<Ability> knownSpells;
        private List<Ability> memorizedSpells;

        public static int MaxPowerSlots = 24;
        public static int MaxSpellSlots = 24;
        public int PowerSlots;
        public int SpellSlots;

        public List<Ability> Traits { get { return traits; } }
        public List<Ability> AvailablePowers { get { return availablePowers; } }
        public List<Ability> KnownPowers { get { return knownPowers; } }
        public List<Ability> MemorizedPowers { get { return memorizedPowers; } }
        public List<Ability> AvailableSpells { get { return availableSpells; } }
        public List<Ability> KnownSpells { get { return knownSpells; } }
        public List<Ability> MemorizedSpells { get { return memorizedSpells; } }

        public CharacterAbilities()
        {
            pc = null;

            PowerSlots = 0;
            SpellSlots = 0;

            traits = new List<Ability>();
            availablePowers = new List<Ability>();
            knownPowers = new List<Ability>();
            memorizedPowers = new List<Ability>();
            availableSpells = new List<Ability>();
            knownSpells = new List<Ability>();
            memorizedSpells = new List<Ability>();
        }

        public CharacterAbilities(PcData pc, int power_slots, int spell_slots)
        {
            this.pc = pc;

            PowerSlots = power_slots;
            SpellSlots = spell_slots;

            traits = new List<Ability>();
            availablePowers = new List<Ability>();
            knownPowers = new List<Ability>();
            memorizedPowers = new List<Ability>();
            availableSpells = new List<Ability>();
            knownSpells = new List<Ability>();
            memorizedSpells = new List<Ability>();
        }

        public CharacterAbilities(PcData pc)
        {
            this.pc = pc;
            PowerSlots = pc.abilities.PowerSlots;
            SpellSlots = pc.abilities.SpellSlots;

            traits = new List<Ability>();
            availablePowers = new List<Ability>();
            knownPowers = new List<Ability>();
            memorizedPowers = new List<Ability>();
            availableSpells = new List<Ability>();
            knownSpells = new List<Ability>();
            memorizedSpells = new List<Ability>();

            for (int i = 0; i < pc.abilities.Traits.Count; i++)
                traits.Add(pc.abilities.traits[i]);

            for (int i = 0; i < pc.abilities.AvailablePowers.Count; i++)
                availablePowers.Add(pc.abilities.AvailablePowers[i]);

            for (int i = 0; i < pc.abilities.KnownPowers.Count; i++)
                knownPowers.Add(pc.abilities.KnownPowers[i]);

            for (int i = 0; i < pc.abilities.MemorizedPowers.Count; i++)
                memorizedPowers.Add(pc.abilities.MemorizedPowers[i]);

            for (int i = 0; i < pc.abilities.AvailableSpells.Count; i++)
                availableSpells.Add(pc.abilities.availableSpells[i]);

            for (int i = 0; i < pc.abilities.KnownSpells.Count; i++)
                knownSpells.Add(pc.abilities.KnownSpells[i]);

            for (int i = 0; i < pc.abilities.MemorizedSpells.Count; i++)
                memorizedSpells.Add(pc.abilities.memorizedSpells[i]);
        }

        public void AddTrait(Ability trait)
        {
            traits.Add(trait);
        }

        public void AddPower(Ability power)
        {
            knownPowers.Add(power);
        }

        public void AddSpell(Ability spell)
        {
            knownSpells.Add(spell);
        }

        public void FindTraits()
        {
            Race race = Database.GetRace(pc.raceKey);
            //Profession profession = Database.GetProfession(pc.ProfessionKey);

            for (int i = 0; i < race.Traits.Count; i++)
            {
                pc.abilities.AddTrait(Database.GetAbility(race.Traits[i].Ability));
            }
        }

        public void FindAvailableAbilities()
        {
            Race race = Database.GetRace(pc.raceKey);
            Profession profession = Database.GetProfession(pc.professionKey);

            for (int i = 0; i < profession.Traits.Count; i++)
            {
                availablePowers.Add(Database.GetAbility(profession.Traits[i].Ability));
            }

            for (int i = 0; i < race.Powers.Count; i++)
            {
                availablePowers.Add(Database.GetAbility(race.Powers[i].Ability));
            }

            for (int i = 0; i < profession.Powers.Count; i++)
            {
                availablePowers.Add(Database.GetAbility(profession.Powers[i].Ability));
            }

            for (int i = 0; i < race.Spells.Count; i++)
            {
                availableSpells.Add(Database.GetAbility(race.Spells[i].Ability));
            }

            for (int i = 0; i < profession.Spells.Count; i++)
            {
                availableSpells.Add(Database.GetAbility(profession.Spells[i].Ability));
            }

            foreach (KeyValuePair<string, Ability> kvp in Database.Abilities)
            {
                Attribute skill = pc.attributes.GetSkill(kvp.Value.SkillUsed);

                if (skill != null)
                {
                    //Debug.Log(kvp.Value.SkillUsed + " " + kvp.Value.SkillRequired + "/" + skill + " " + skill.Current);

                    if (kvp.Value.SkillUsed != Skill.None && skill.Current > kvp.Value.SkillRequired)
                    {
                        if (kvp.Value.Type == AbilityType.Power)
                        {
                            availablePowers.Add(Database.GetAbility(kvp.Key));
                        }
                        else if (kvp.Value.Type == AbilityType.Spell)
                        {
                            availableSpells.Add(Database.GetAbility(kvp.Key));
                        }
                    }
                }
            }
        }
    }
}