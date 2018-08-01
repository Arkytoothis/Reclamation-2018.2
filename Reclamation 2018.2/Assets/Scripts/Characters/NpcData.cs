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

    public class NpcData : CharacterData
    {
        public string key;
        public int npcIndex;
        public int partyIndex;
        public int partySlot;

        public int level;
        public int expValue;

        public List<Ability> abilities;

        CombatStatus combatStatus;
        public CombatStatus CombatStatus { get { return combatStatus; } }


        public override void ModifyAttribute(AttributeType type, int attribute, int value)
        {
            if (value == 0) return;

            base.ModifyAttribute(type, attribute, value);

            int cur = attributeManager.GetAttribute(AttributeListType.Derived, attribute).Current;
            int max = attributeManager.GetAttribute(AttributeListType.Derived, attribute).Maximum;

            if (attribute == (int)DerivedAttribute.Armor && onArmorChange != null)
                onArmorChange(cur, max);
            else if (attribute == (int)DerivedAttribute.Health && onHealthChange != null)
                onHealthChange(cur, max);
            else if (attribute == (int)DerivedAttribute.Stamina && onStaminaChange != null)
                onStaminaChange(cur, max);
            else if (attribute == (int)DerivedAttribute.Essence && onEssenceChange != null)
                onEssenceChange(cur, max);
            else if (attribute == (int)DerivedAttribute.Morale && onMoraleChange != null)
                onMoraleChange(cur, max);

            CheckVitals();
        }

        public new void SetStart(AttributeType type, int attribute, int start, int min, int max)
        {
            if (start == 0) return;
            attributeManager.SetStart((AttributeListType)type, attribute, start, min, max);
        }

        public new delegate void OnArmorChange(int current, int max);
        public new event OnArmorChange onArmorChange;

        public new delegate void OnHealthChange(int current, int max);
        public new event OnHealthChange onHealthChange;

        public new delegate void OnStaminaChange(int current, int max);
        public new event OnStaminaChange onStaminaChange;

        public new delegate void OnEssenceChange(int current, int max);
        public new event OnEssenceChange onEssenceChange;

        public new delegate void OnMoraleChange(int current, int max);
        public new event OnMoraleChange onMoraleChange;

        public new delegate void OnDeath();
        public new event OnDeath onDeath;

        public new delegate void OnRevive();
        public new event OnRevive onRevive;

        public new delegate void OnInteract();
        public new event OnInteract onInteract;

        public new delegate void OnAttack();
        public new event OnAttack onAttack;

        public NpcData()
        {
            name = new FantasyName();
            gender = Gender.None;
            background = null;

            key = "";
            combatStatus = CombatStatus.None;
            raceKey = "";
            professionKey = "";
            npcIndex = -1;
            partyIndex = -1;
            hair = "";
            beard = "";

            level = 0;
            expValue = 0;

            description = "";

            attributeManager = new AttributeManager();

            for (int i = 0; i < (int)BaseAttribute.Number; i++)
                attributeManager.AddAttribute(AttributeListType.Base, new Attribute(AttributeType.Base, i, GameSettings.AttributeExpCost));

            for (int i = 0; i < (int)DerivedAttribute.Number; i++)
                attributeManager.AddAttribute(AttributeListType.Derived, new Attribute(AttributeType.Derived, i, 0));

            for (int i = 0; i < (int)DamageType.Number; i++)
                attributeManager.AddAttribute(AttributeListType.Resistance, new Attribute(AttributeType.Resistance, i, 0));

            abilities = new List<Ability>();
            inventory = new CharacterInventory();
        }

        public NpcData(FantasyName name, string key, Gender gender, string race, string profession, int index, int map_x, int map_y, int enc_x, int enc_y)
        {
            base.name = new FantasyName(name);
            background = new Background();

            this.key = key;
            base.gender = gender;
            raceKey = race;
            professionKey = profession;
            npcIndex = index;
            partyIndex = -1;

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

            abilities = new List<Ability>();
            inventory = new CharacterInventory();
        }

        public NpcData(NpcData npc)
        {
            name = new FantasyName(npc.name);
            background = new Background(npc.background);

            key = npc.key;
            raceKey = npc.raceKey;
            professionKey = npc.professionKey;
            npcIndex = npc.npcIndex;
            partyIndex = npc.partyIndex;
            partySlot = npc.partySlot;

            hair = npc.hair;
            beard = npc.beard;

            description = npc.description;
            background = npc.background;

            level = npc.level;
            expValue = npc.expValue;

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

            abilities = new List<Ability>();
            inventory = new CharacterInventory(npc.inventory);
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
        }

        public void Revive()
        {
            isDead = false;
            //onRevive();
            Debug.Log(name.FirstName + " has revived");
        }

        public void Interact()
        {
            Debug.Log(name.FirstName + " is interacting");
            //onInteract();
        }

        public void Attack()
        {
            Debug.Log(name.FirstName + " is attacking");
            onAttack();
        }
    }
}