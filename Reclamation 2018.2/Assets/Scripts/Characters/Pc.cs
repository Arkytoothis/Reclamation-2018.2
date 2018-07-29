using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Reclamation.Misc;
using Reclamation.Name;
using Reclamation.Equipment;

namespace Reclamation.Characters
{
    public enum PcStatus
    {
        Idle, Adventuring, Resting, Camping, Working, Training,
        Number, None
    }

    [System.Serializable]
    public class Pc : Character
    {
        public UpkeepData Upkeep;
        public int Wealth;
        public PcStatus Status;
        public int EncounterIndex;
        public int WorldIndex;
        public int PartyIndex;
        public int PartySlot;

        public int Level;

        private int experience;
        public int Experience { get { return experience; } }
        private int expToLevel;
        public int ExpToLevel { get { return expToLevel; } }
        private int maxExp;
        public int MaxExp { get { return maxExp; } }
        private float expBonus;
        public float ExpBonus { get { return expBonus; } }

        public CharacterAbilities Abilities;

        public int MaxAccessories;

        public Pc()
        {
            Wealth = 0;
            Upkeep = new UpkeepData();
            Name = new FantasyName();
            Gender = Gender.None;
            Background = null;
            Status = PcStatus.Idle;

            RaceKey = "";
            ProfessionKey = "";
            Description = "";

            EncounterIndex = -1;
            WorldIndex = -1;
            PartyIndex = -1;
            Hair = -1;
            Beard = -1;
            MaxAccessories = 1;

            Level = 0;
            experience = 0;
            expToLevel = 0;
            maxExp = 0;
            expBonus = 0;

            attributeManager = new AttributeManager();

            for (int i = 0; i < (int)BaseAttribute.Number; i++)
                attributeManager.AddAttribute(AttributeListType.Base, new Attribute(AttributeType.Base, i, GameSettings.AttributeExpCost));

            for (int i = 0; i < (int)DerivedAttribute.Number; i++)
                attributeManager.AddAttribute(AttributeListType.Derived, new Attribute(AttributeType.Derived, i, 0));

            for (int i = 0; i < (int)DamageType.Number; i++)
                attributeManager.AddAttribute(AttributeListType.Resistance, new Attribute(AttributeType.Resistance, i, 0));

            Abilities = new CharacterAbilities();
            Inventory = new CharacterInventory();
        }

        public Pc(FantasyName name, Gender gender, Background background, string race, string profession, int hair, int beard, int index, int enc_index, int party_index,
            int power_slots, int spell_slots)
        {
            Wealth = 0;
            Upkeep = new UpkeepData();
            Name = name;
            Background = new Background(background);

            Gender = gender;
            RaceKey = race;
            ProfessionKey = profession;
            WorldIndex = index;
            EncounterIndex = enc_index;
            PartyIndex = party_index;
            Hair = hair;
            Beard = beard;

            MaxAccessories = Random.Range(1, 4);

            Level = 0;
            experience = 0;
            expToLevel = 0;
            maxExp = 0;
            expBonus = 0.0f;

            attributeManager = new AttributeManager();

            for (int i = 0; i < (int)BaseAttribute.Number; i++)
            {
                attributeManager.AddAttribute(AttributeListType.Base, new Attribute(AttributeType.Base, i, GameSettings.AttributeExpCost));
            }

            for (int i = 0; i < (int)DerivedAttribute.Number; i++)
            {
                attributeManager.AddAttribute(AttributeListType.Derived, new Attribute(AttributeType.Derived, i, 0));
            }

            for (int i = 0; i < (int)DamageType.Number; i++)
            {
                attributeManager.AddAttribute(AttributeListType.Resistance, new Attribute(AttributeType.Resistance, i, 0));
            }

            Abilities = new CharacterAbilities(this, power_slots, spell_slots);
            Inventory = new CharacterInventory();
        }

        public Pc(Pc pc)
        {
            Name = pc.Name;
            Gender = pc.Gender;

            Wealth = pc.Wealth;
            Upkeep = new UpkeepData(pc.Upkeep);
            Background = new Background(pc.Background);
            RaceKey = pc.RaceKey;
            ProfessionKey = pc.ProfessionKey;
            WorldIndex = pc.WorldIndex;
            EncounterIndex = pc.EncounterIndex;
            PartyIndex = pc.PartyIndex;
            PartySlot = pc.PartySlot;
            Hair = pc.Hair;
            Beard = pc.Beard;

            Description = pc.Description;

            Level = pc.Level;
            experience = pc.Experience;
            expToLevel = pc.ExpToLevel;
            maxExp = pc.MaxExp;
            expBonus = pc.ExpBonus;

            attributeManager = new AttributeManager();

            for (int i = 0; i < (int)BaseAttribute.Number; i++)
            {
                attributeManager.AddAttribute(AttributeListType.Base, new Attribute(pc.attributeManager.GetAttribute(AttributeListType.Base, i)));
            }

            for (int i = 0; i < (int)DerivedAttribute.Number; i++)
            {
                attributeManager.AddAttribute(AttributeListType.Derived, new Attribute(pc.attributeManager.GetAttribute(AttributeListType.Derived, i)));
            }

            for (int i = 0; i < (int)DamageType.Number; i++)
            {
                attributeManager.AddAttribute(AttributeListType.Resistance, new Attribute(pc.attributeManager.GetAttribute(AttributeListType.Resistance, i)));
            }

            foreach (KeyValuePair<Skill, Attribute> kvp in pc.GetSkills())
            {
                attributeManager.AddSkill(kvp.Key, new Attribute(kvp.Value));
            }

            Abilities = new CharacterAbilities(pc);
            Inventory = new CharacterInventory(pc.Inventory);
        }

        public override void CalculateStartSkills()
        {
            int numSkills = Random.Range(0, 6);
            int start = 0;
            for (int i = 0; i < numSkills; i++)
            {
                SkillDefinition skillDef = Database.Skills[Random.Range(0, Database.Skills.Count)];
                Attribute skill = new Attribute();

                start = Random.Range(1, 4);
                skill.Type = AttributeType.Skill;
                skill.SetStart(start, 0, 100);

                attributeManager.AddSkill(skillDef.key, skill);
            }
        }           

        public bool CanEquip(Item item, EquipmentSlot slot)
        {
            bool canEquip = false;

            if (item.Slot != slot)
                canEquip = true;

            return canEquip;
        }

        public void CalculateAttributeModifiers()
        {
            for (int slot = 0; slot < (int)EquipmentSlot.Number; slot++)
            {
                if (Inventory.EquippedItems[slot] != null)
                {
                    Item item = Inventory.EquippedItems[slot];

                    if (item.WeaponData != null)
                    {
                        //if (item.WeaponData.AttackType == AttackType.Might)
                        //    DerivedAttributes[(int)DerivedAttribute.Might_Attack].AddToModifier(item.WeaponData.Attributes[(int)WeaponAttributes.Attack].Value);
                        //else if (item.WeaponData.AttackType == AttackType.Finesse)
                        //    DerivedAttributes[(int)DerivedAttribute.Finesse_Attack].AddToModifier(item.WeaponData.Attributes[(int)WeaponAttributes.Attack].Value);
                        //else if (item.WeaponData.AttackType == AttackType.Spell)
                        //    DerivedAttributes[(int)DerivedAttribute.Spell_Attack].AddToModifier(item.WeaponData.Attributes[(int)WeaponAttributes.Attack].Value);

                        //DerivedAttributes[(int)DerivedAttribute.Parry].AddToModifier(item.WeaponData.Attributes[(int)WeaponAttributes.Parry].Value);
                    }

                    if (item.WearableData != null)
                    {
                        //DerivedAttributes[(int)DerivedAttribute.Armor].AddToModifier(item.WearableData.Attributes[(int)WearableAttributes.Armor].Value);
                        //DerivedAttributes[(int)DerivedAttribute.Block].AddToModifier(item.WearableData.Attributes[(int)WearableAttributes.Block].Value);
                        //DerivedAttributes[(int)DerivedAttribute.Dodge].AddToModifier(item.WearableData.Attributes[(int)WearableAttributes.Dodge].Value);

                        //for (int r = 0; r < item.WearableData.Resistances.Count; r++)
                        //{
                        //    Resistances[(int)item.WearableData.Resistances[r].DamageType].AddToModifier(item.WearableData.Resistances[r].Value);
                        //}
                    }

                    //for (int m = 0; m < item.Modifiers.Count; m++)
                    //{
                    //    for (int e = 0; e < item.Modifiers[m].Effects.Count; e++)
                    //    {
                    //        if (item.Modifiers[m].Effects[e].GetType() == typeof(AlterCharacteristicEffect))
                    //        {
                    //            AlterCharacteristicEffect effect = (AlterCharacteristicEffect)item.Modifiers[m].Effects[e];

                    //            if (effect.Type == CharacteristicType.Base_Attribute)
                    //                BaseAttributes[effect.Characteristic].AddToModifier(effect.MaxValue);
                    //            else if (effect.Type == CharacteristicType.Derived_Attribute)
                    //                DerivedAttributes[effect.Characteristic].AddToModifier(effect.MaxValue);
                    //            else if (effect.Type == CharacteristicType.Skill)
                    //                Skills[effect.Characteristic].AddToModifier(effect.MaxValue);
                    //            else if (effect.Type == CharacteristicType.Resistance)
                    //                Resistances[effect.Characteristic].AddToModifier(effect.MaxValue);
                    //        }
                    //    }
                    //}
                }
            }
        }

        public void CalculateExp()
        {
            expToLevel = Level * 1000;
            maxExp = Level * 10000;
        }

        public void CalculateExpCosts()
        {
            //for (int i = 0; i < BaseAttributes.Count; i++)
            //{
            //    BaseAttributes[i].CalculateExpCost();
            //}

            //for (int i = 0; i < Skills.Count; i++)
            //{
            //    Skills[i].CalculateExpCost();
            //}
        }

        public void AddExperience(int amount, bool adjusted)
        {
            if (amount == 0) return;

            int expToAdd = 0;

            if (adjusted == true)
            {
                expToAdd = (int)((float)amount * Database.GetRace(RaceKey).ExpModifier);
                expToAdd += (int)((float)amount * ExpBonus);
            }
            else
            {
                expToAdd = amount;
            }

            experience += expToAdd;

            if (experience >= expToLevel)
                LevelUp();

            onExperienceChange(experience, expToLevel);
        }

        public void SpendExperience(int amount)
        {
            experience -= amount;
        }

        public void LevelUp()
        {
            //Debug.Log(Name.FirstName + " has gained a level!");
            SpendExperience(expToLevel);
            Level++;
            CalculateExp();
            CalculateExpCosts();
            CalculateDerivedAttributes();

            onLevelUp();
        }

        public void CalculateUpkeep()
        {
            Upkeep = new UpkeepData();

            Race race = Database.GetRace(RaceKey);
            Upkeep.Coin = race.Upkeep.Coin;
            Upkeep.Essence = race.Upkeep.Essence;
            Upkeep.Materials = race.Upkeep.Materials;
            Upkeep.Rations = race.Upkeep.Rations;

            Profession profession = Database.GetProfession(ProfessionKey);
            Upkeep.Coin += profession.Upkeep.Coin;
            Upkeep.Essence += profession.Upkeep.Essence;
            Upkeep.Materials += profession.Upkeep.Materials;
            Upkeep.Rations += profession.Upkeep.Rations;

            Wealth = Database.Races[RaceKey].StartingWealth.Roll(false) + Database.Professions[ProfessionKey].StartingWealth.Roll(false);
        }

        public new void ModifyAttribute(AttributeType type, int attribute, int value)
        {
            if (value == 0) return;

            base.ModifyAttribute(type, attribute, value);

            int cur = attributeManager.GetAttribute(AttributeListType.Derived, attribute).Current;
            int max = attributeManager.GetAttribute(AttributeListType.Derived, attribute).Maximum;

            if (attribute == (int)DerivedAttribute.Armor)
                onArmorChange(cur, max);
            else if (attribute == (int)DerivedAttribute.Health)
                onHealthChange(cur, max);
            else if (attribute == (int)DerivedAttribute.Stamina)
                onStaminaChange(cur, max);
            else if (attribute == (int)DerivedAttribute.Essence)
                onEssenceChange(cur, max);
            else if (attribute == (int)DerivedAttribute.Morale)
                onMoraleChange(cur, max);

            CheckVitals();
        }

        public delegate void OnArmorChange(int current, int max);
        public event OnArmorChange onArmorChange;

        public delegate void OnHealthChange(int current, int max);
        public event OnHealthChange onHealthChange;

        public delegate void OnStaminaChange(int current, int max);
        public event OnStaminaChange onStaminaChange;

        public delegate void OnEssenceChange(int current, int max);
        public event OnEssenceChange onEssenceChange;

        public delegate void OnMoraleChange(int current, int max);
        public event OnMoraleChange onMoraleChange;

        public delegate void OnExperienceChange(int current, int max);
        public event OnExperienceChange onExperienceChange;

        public delegate void OnDeath();
        public event OnDeath onDeath;

        public delegate void OnRevive();
        public event OnRevive onRevive;

        public delegate void OnLevelUp();
        public event OnLevelUp onLevelUp;

        public delegate void OnInteract();
        public event OnInteract onInteract;

        public delegate void OnAttack();
        public event OnAttack onAttack;

        public void CheckVitals()
        {
            CheckHealth();
            CheckStamina();
            CheckEssence();
            CheckMorale();
        }

        public void CheckHealth()
        {
            if (isDead == false && attributeManager.GetAttribute(AttributeListType.Derived, (int)DerivedAttribute.Health).Current <= 0)
            {
                Death();
            }
            else if (isDead == true && attributeManager.GetAttribute(AttributeListType.Derived, (int)DerivedAttribute.Health).Current > 0)
            {
                Revive();
            }
        }

        public void CheckStamina()
        {
            if (isDead == false && isExhausted == false && attributeManager.GetAttribute(AttributeListType.Derived, (int)DerivedAttribute.Stamina).Current <= 0)
            {
                isExhausted = true;
                //Debug.Log(Name.FirstName + " is exhausted");
            }
        }

        public void CheckEssence()
        {
            if (isDead == false && isDrained == false && attributeManager.GetAttribute(AttributeListType.Derived, (int)DerivedAttribute.Essence).Current <= 0)
            {
                isDrained = true;
                //Debug.Log(Name.FirstName + " is out of essence");
            }
        }

        public void CheckMorale()
        {
            if (isDead == false && isBroken == false && attributeManager.GetAttribute(AttributeListType.Derived, (int)DerivedAttribute.Morale).Current <= 0)
            {
                isBroken = true;
                //Debug.Log(Name.FirstName + " is broken");
            }
        }

        public void Death()
        {
            isDead = true;
            onDeath();
            Debug.Log(Name.FirstName + " has died");
        }

        public void Revive()
        {
            isDead = false;
            onRevive();
            Debug.Log(Name.FirstName + " has revived");
        }

        public void Interact()
        {
            Debug.Log(Name.FirstName + " is interacting");
            onInteract();
        }

        public void Attack()
        {
            Debug.Log(Name.FirstName + " is attacking");
            onAttack();
        }
    }
}