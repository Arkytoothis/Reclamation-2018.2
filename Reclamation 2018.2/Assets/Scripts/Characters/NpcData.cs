using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Reclamation.Abilities;
using Reclamation.Name;
using Reclamation.Characters;
using Reclamation.Misc;

namespace Reclamation.Characters
{
    public enum CombatStatus { Awake, Unconcisous, Dead, Number, None }

    public enum NpcType
    {
        Boss, Mini_Boss, Boss_Guard, Objective_Enemy, Powerful_Enenmy, Enemy, Weak_Enemy,
        Citizen, Rescue_Target, Survivor, Neutral, Hireling, Story, Trader,
        Number, None
    }

    public class NpcData : Character
    {
        public string Key;
        public int NPCIndex;
        public int PartyIndex;
        public int PartySlot;

        public int Level;
        public int ExpValue;

        public List<Ability> Abilities;

        CombatStatus combatStatus;
        public CombatStatus CombatStatus { get { return combatStatus; } }


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

        public delegate void OnDeath();
        public event OnDeath onDeath;

        public delegate void OnRevive();
        public event OnRevive onRevive;

        public delegate void OnInteract();
        public event OnInteract onInteract;

        public delegate void OnAttack();
        public event OnAttack onAttack;

        public NpcData()
        {
            Name = new FantasyName();
            Gender = Gender.None;
            Background = null;

            Key = "";
            combatStatus = CombatStatus.None;
            RaceKey = "";
            ProfessionKey = "";
            NPCIndex = -1;
            PartyIndex = -1;
            Hair = -1;
            Beard = -1;

            Level = 0;
            ExpValue = 0;

            Description = "";

            attributeManager = new AttributeManager();

            for (int i = 0; i < (int)BaseAttribute.Number; i++)
                attributeManager.AddAttribute(AttributeListType.Base, new Attribute(AttributeType.Base, i, GameSettings.AttributeExpCost));

            for (int i = 0; i < (int)DerivedAttribute.Number; i++)
                attributeManager.AddAttribute(AttributeListType.Derived, new Attribute(AttributeType.Derived, i, 0));

            for (int i = 0; i < (int)DamageType.Number; i++)
                attributeManager.AddAttribute(AttributeListType.Resistance, new Attribute(AttributeType.Resistance, i, 0));

            Abilities = new List<Ability>();
            Inventory = new CharacterInventory();
        }

        public NpcData(FantasyName name, string key, Gender gender, string race, string profession, int hair, int beard, int index, int map_x, int map_y, int enc_x, int enc_y)
        {
            Name = new FantasyName(name);
            Background = new Background();

            Key = key;
            Gender = gender;
            RaceKey = race;
            ProfessionKey = profession;
            NPCIndex = index;
            PartyIndex = -1;
            Hair = hair;
            Beard = beard;
            combatStatus = CombatStatus.Awake;

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

            Abilities = new List<Ability>();
            Inventory = new CharacterInventory();
        }

        public NpcData(NpcData npc)
        {
            Name = new FantasyName(npc.Name);
            Background = new Background(npc.Background);

            Key = npc.Key;
            RaceKey = npc.RaceKey;
            ProfessionKey = npc.ProfessionKey;
            NPCIndex = npc.NPCIndex;
            PartyIndex = npc.PartyIndex;
            PartySlot = npc.PartySlot;

            Hair = npc.Hair;
            Beard = npc.Beard;

            Description = npc.Description;
            Background = npc.Background;

            Level = npc.Level;
            ExpValue = npc.ExpValue;

            combatStatus = npc.combatStatus;

            attributeManager = new AttributeManager();

            for (int i = 0; i < (int)BaseAttribute.Number; i++)
            {
                attributeManager.AddAttribute(AttributeListType.Base, new Attribute(npc.attributeManager.GetAttribute(AttributeListType.Base, i)));
            }

            for (int i = 0; i < (int)DerivedAttribute.Number; i++)
            {
                attributeManager.AddAttribute(AttributeListType.Derived, new Attribute(npc.attributeManager.GetAttribute(AttributeListType.Derived, i)));
            }

            for (int i = 0; i < (int)DamageType.Number; i++)
            {
                attributeManager.AddAttribute(AttributeListType.Resistance, new Attribute(npc.attributeManager.GetAttribute(AttributeListType.Resistance, i)));
            }

            Abilities = new List<Ability>();
            Inventory = new CharacterInventory(npc.Inventory);
        }

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