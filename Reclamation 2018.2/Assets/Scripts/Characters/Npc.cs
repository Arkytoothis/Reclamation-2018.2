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

    public class NPC : Character
    {
        public NPCImageType ImageType;
        public string Sprite;
        public string Key;
        public int NPCIndex;
        public int PartyIndex;
        public int PartySlot;

        public int Level;
        public int ExpValue;

        public List<Ability> Abilities;

        //StatBarWidgetWorld healthBar;
        //StatBarWidgetWorld actionsBar;

        CombatStatus combatStatus;
        public CombatStatus CombatStatus { get { return combatStatus; } }

        SpriteRenderer statusSprite;
        public SpriteRenderer StatusSprite { get { return statusSprite; } }

        public NPC()
        {
            Name = new FantasyName();
            Gender = Gender.None;
            Background = null;

            Key = "";
            ImageType = NPCImageType.None;
            Sprite = "";
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

            //healthBar = null;
            //actionsBar = null;
            statusSprite = null;
        }

        public NPC(FantasyName name, string key, Gender gender, NPCImageType image_type, string sprite, string race, string profession, int hair, int beard, int index, int map_x, int map_y, int enc_x, int enc_y)
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
            ImageType = image_type;
            Sprite = sprite;

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

            //healthBar = null;
            //actionsBar = null;
            statusSprite = null;
        }

        public NPC(NPC npc)
        {
            Name = new FantasyName(npc.Name);
            Background = new Background(npc.Background);

            Key = npc.Key;
            RaceKey = npc.RaceKey;
            ProfessionKey = npc.ProfessionKey;
            NPCIndex = npc.NPCIndex;
            PartyIndex = npc.PartyIndex;
            PartySlot = npc.PartySlot;

            ImageType = npc.ImageType;
            Sprite = npc.Sprite;
            Hair = npc.Hair;
            Beard = npc.Beard;

            Description = npc.Description;
            Background = npc.Background;

            Level = npc.Level;
            ExpValue = npc.ExpValue;

            combatStatus = npc.combatStatus;
            statusSprite = npc.statusSprite;

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

            //healthBar = npc.healthBar;
            //actionsBar = npc.actionsBar;
        }


        public override void ModifyAttribute(AttributeType type, int attribute, int value)
        {
            base.ModifyAttribute(type, attribute, value);
        }
    }
}