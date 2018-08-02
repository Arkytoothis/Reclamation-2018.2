using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Reclamation.Gui;
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
    public class PcData : CharacterData
    {
        public UpkeepData upkeep;
        public int wealth;
        public PcStatus status;
        public int encounterIndex;
        public int worldIndex;
        public int partyIndex;
        public int partySlot;

        public int level;

        private int experience;
        public int Experience { get { return experience; } }
        private int expToLevel;
        public int ExpToLevel { get { return expToLevel; } }
        private int maxExp;
        public int MaxExp { get { return maxExp; } }
        private float expBonus;
        public float ExpBonus { get { return expBonus; } }

        public CharacterAbilities abilities;

        public int maxAccessories;

        public PcData()
        {
            wealth = 0;
            upkeep = new UpkeepData();
            name = new FantasyName();
            gender = Gender.None;
            background = null;
            status = PcStatus.Idle;

            raceKey = "";
            professionKey = "";
            description = "";

            encounterIndex = -1;
            worldIndex = -1;
            partyIndex = -1;
            hair = "Hair 01";
            beard = "";
            maxAccessories = 1;

            level = 0;
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

            abilities = new CharacterAbilities();
            inventory = new CharacterInventory();
        }

        public PcData(FantasyName name, Gender gender, Background background, string race, string profession, string hair, string beard, int index, int enc_index, int party_index,
            int power_slots, int spell_slots)
        {
            wealth = 0;
            upkeep = new UpkeepData();
            base.name = name;
            base.background = new Background(background);

            base.gender = gender;
            raceKey = race;
            professionKey = profession;
            worldIndex = index;
            encounterIndex = enc_index;
            partyIndex = party_index;

            this.hair = hair;
            this.beard = beard;

            maxAccessories = Random.Range(1, 4);

            level = 0;
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

            abilities = new CharacterAbilities(this, power_slots, spell_slots);
            inventory = new CharacterInventory();
        }

        public PcData(PcData pc)
        {
            name = pc.name;
            gender = pc.gender;

            wealth = pc.wealth;
            upkeep = new UpkeepData(pc.upkeep);
            background = new Background(pc.background);
            raceKey = pc.raceKey;
            professionKey = pc.professionKey;
            worldIndex = pc.worldIndex;
            encounterIndex = pc.encounterIndex;
            partyIndex = pc.partyIndex;
            partySlot = pc.partySlot;
            hair = pc.hair;
            beard= pc.beard;

            description = pc.description;

            level = pc.level;
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

            abilities = new CharacterAbilities(pc);
            inventory = new CharacterInventory(pc.inventory);
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

        public bool CanEquip(ItemData item, EquipmentSlot slot)
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
                if (inventory.EquippedItems[slot] != null)
                {
                    ItemData item = inventory.EquippedItems[slot];

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
            expToLevel = level * 1000;
            maxExp = level * 10000;
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
                expToAdd = (int)((float)amount * Database.GetRace(raceKey).ExpModifier);
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
            level++;
            CalculateExp();
            CalculateExpCosts();
            CalculateDerivedAttributes();

            onLevelUp();
        }

        public void CalculateUpkeep()
        {
            upkeep = new UpkeepData();

            Race race = Database.GetRace(raceKey);
            upkeep.Coin = race.Upkeep.Coin;
            upkeep.Essence = race.Upkeep.Essence;
            upkeep.Materials = race.Upkeep.Materials;
            upkeep.Rations = race.Upkeep.Rations;

            Profession profession = Database.GetProfession(professionKey);
            upkeep.Coin += profession.Upkeep.Coin;
            upkeep.Essence += profession.Upkeep.Essence;
            upkeep.Materials += profession.Upkeep.Materials;
            upkeep.Rations += profession.Upkeep.Rations;

            wealth = Database.Races[raceKey].StartingWealth.Roll(false) + Database.Professions[professionKey].StartingWealth.Roll(false);
        }

        public override void ModifyAttribute(AttributeType type, int attribute, int value)
        {
            if (value == 0) return;

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

            base.ModifyAttribute(type, attribute, value);
            CheckVitals();
        }

        public event OnArmorChange onArmorChange;
        public event OnHealthChange onHealthChange;
        public event OnStaminaChange onStaminaChange;
        public event OnEssenceChange onEssenceChange;
        public event OnMoraleChange onMoraleChange;
        public event OnDeath onDeath;
        public event OnRevive onRevive;
        public event OnInteract onInteract;
        public event OnAttack onAttack;


        public delegate void OnExperienceChange(int current, int max);
        public event OnExperienceChange onExperienceChange;

        public delegate void OnLevelUp();
        public event OnLevelUp onLevelUp;

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
            MessageSystem.instance.AddMessage(name.FirstName + " has died");
        }

        public void Revive()
        {
            isDead = false;
            onRevive();
            MessageSystem.instance.AddMessage(name.FirstName + " has revived");
        }

        public void Interact()
        {
            MessageSystem.instance.AddMessage(name.FirstName + " has interacting");
            onInteract();
        }

        public void Attack()
        {
            onAttack();
            MessageSystem.instance.AddMessage(name.FirstName + " has attacking");
        }
    }
}