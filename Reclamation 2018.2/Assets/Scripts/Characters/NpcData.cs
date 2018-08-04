using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Reclamation.Abilities;
using Reclamation.Name;
using Reclamation.Gui;
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

    [System.Serializable]
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
        

        public new void SetStart(AttributeType type, int attribute, int start, int min, int max)
        {
            if (start == 0) return;
            attributes.SetStart((AttributeListType)type, attribute, start, min, max);
        }

        //public event OnArmorChange onArmorChange;
        //public event OnHealthChange onHealthChange;
        //public event OnStaminaChange onStaminaChange;
        //public event OnEssenceChange onEssenceChange;
        //public event OnMoraleChange onMoraleChange;
        //public event OnDeath onDeath;
        //public event OnRevive onRevive;
        //public event OnInteract onInteract;
        //public event OnAttack onAttack;

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
            faction = "Neutral";

            level = 0;
            expValue = 0;

            description = "";

            attributes = new AttributeManager();

            for (int i = 0; i < (int)BaseAttribute.Number; i++)
                attributes.AddAttribute(AttributeListType.Base, new Attribute(AttributeType.Base, i, GameSettings.AttributeExpCost));

            for (int i = 0; i < (int)DerivedAttribute.Number; i++)
                attributes.AddAttribute(AttributeListType.Derived, new Attribute(AttributeType.Derived, i, 0));

            for (int i = 0; i < (int)DamageType.Number; i++)
                attributes.AddAttribute(AttributeListType.Resistance, new Attribute(AttributeType.Resistance, i, 0));

            abilities = new List<Ability>();
            inventory = new CharacterInventory();
        }

        public NpcData(FantasyName name, string key, Gender gender, string race, string profession, int index, int map_x, int map_y, int enc_x, int enc_y,
            string faction)
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

            attributes = new AttributeManager();

            for (int i = 0; i < (int)BaseAttribute.Number; i++)
            {
                attributes.AddAttribute(AttributeListType.Base, new Attribute(AttributeType.Base, i, GameSettings.AttributeExpCost));
            }

            for (int i = 0; i < (int)DerivedAttribute.Number; i++)
            {
                attributes.AddAttribute(AttributeListType.Derived, new Attribute(AttributeType.Derived, i, 0));
            }

            for (int i = 0; i < (int)DamageType.Number; i++)
            {
                attributes.AddAttribute(AttributeListType.Resistance, new Attribute(AttributeType.Resistance, i, 0));
            }

            abilities = new List<Ability>();
            inventory = new CharacterInventory();
            this.faction = faction;
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

            attributes = new AttributeManager();

            for (int i = 0; i < (int)BaseAttribute.Number; i++)
            {
                attributes.AddAttribute(AttributeListType.Base, new Attribute(npc.attributes.GetAttribute(AttributeListType.Base, i)));
            }

            for (int i = 0; i < (int)DerivedAttribute.Number; i++)
            {
                attributes.AddAttribute(AttributeListType.Derived, new Attribute(npc.attributes.GetAttribute(AttributeListType.Derived, i)));
            }

            for (int i = 0; i < (int)DamageType.Number; i++)
            {
                attributes.AddAttribute(AttributeListType.Resistance, new Attribute(npc.attributes.GetAttribute(AttributeListType.Resistance, i)));
            }

            abilities = new List<Ability>();
            inventory = new CharacterInventory(npc.inventory);
            faction = npc.faction;
        }

        public void SetupController(PcController controller)
        {
            attributes.controller = controller;
        }
    }
}