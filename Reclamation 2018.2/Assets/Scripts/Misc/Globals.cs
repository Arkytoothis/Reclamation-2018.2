using UnityEngine;
using System.Collections.Generic;

namespace Reclamation.Misc
{
    public static class Globals
    {
        public static string EmptyString = "empty";
        public static int AttributeMax = 999999;

        public static List<Color> RarityColors = new List<Color>
    {
        new Color(0.4f, 0.4f, 0.4f, 1f),    //Common
        new Color(1.0f, 1.0f, 1.0f, 1f),    //Uncommon
        new Color(0.1f, 0.1f, 1.0f, 1f),    //Rare
        new Color(0.0f, 1.0f, 0.0f, 1f),    //Fabled        
        new Color(0.6f, 0.0f, 0.6f, 1f),    //Mythical
        new Color(0.6f, 0.4f, 0.0f, 1f),    //Legendary
        new Color(0.0f, 0.9f, 1.0f, 1f),    //Artifact
        new Color(1.0f, 0.9f, 0.1f, 1f)     //Set
    };
    }

    public enum Rarity
    {
        Common, Uncommon, Rare, Fabled, Mythical, Legendary, Set, Artifact,
        Number, None
    };

    public enum AttributeDefinitionType
    {
        Base, Derived_Points, Derived_Score, Derived_Percent, Resistance, Skill, Party, Weapon, Ammo, Wearable, Accessory,
        Number, None
    }

    public enum AttributeComponentType
    {
        Current, Start, Minimum, Maximum, Modifier, Spent, Exp_Cost,
        Number, None
    }

    public enum AttributeType
    {
        Base, Derived, Resistance, Skill, Party, Weapon, Ammo, Wearable, Accessory,
        Number, None
    }

    public enum DerivedAttributeType
    {
        Derived_Points, Derived_Percent, Derived_Score,
        Number, None
    }

    public enum AttributeModifierType
    {
        Base_Attribute, Derived_Attribute, Resistance, Skill, Party_Attribute, Value, Race,
        Number, None
    }

    public enum AttributeField
    { Start, Current, Minimum, Maximum, Modifier }

    public enum Gender
    {
        Male, Female, Both, Either, Other, None, Number,
    };

    public enum SkillCategory
    {
        Combat, Magic, Misc
    };

    public enum EquipmentSlot
    {
        Right_Hand, Left_Hand, Body, Head, Arms, Hands, Legs, Feet, Back, Neck, Waist, Left_Finger, Right_Finger,
        Number, None
    };

    public enum CharacterRendererSlots
    {
        Back, Wings, Body, Horns, Main_Hand, Ammo, Torso, Legs, Beard, Hair, Head, Hands, Feet, Neck, Left_Finger, Right_Finger, Waist, Wrists, Off_Hand,
        Number, None
    };

    public enum ItemType
    {
        Weapon, Ammo, Wearable, Accessory, Quest,
        Number, None
    };

    public enum WeaponType
    {
        Unarmed, One_Handed_Melee, Two_Handed_Melee, Polearm, Bow, Crossbow, Thrown, Firearm,
        None, Number
    }

    public enum AmmoType
    {
        Arrow, Bolt, Bullet, Sling_Stone,
        None, Number
    }

    public enum WearableType
    {
        Clothing, Armor, Shield, Jewelry,
        None, Number
    }

    public enum AccessoryType
    {
        Consumable, Scroll, Throwable, Usable, Tool,
        None, Number
    }

    public enum ItemModifierType
    {
        Material, Quality, Plus_Enchant, Pre_Enchant, Post_Enchant,
        Number, None
    }

    public enum ItemTypeAllowed
    {
        Any,
        Weapon, One_Handed_Melee, Two_Handed_Melee, Polearm, Bow, Crossbow, Thrown, Firearm,
        Ammo, Arrow, Bolt, Bullet, Sling_Stone,
        Wearable, Clothing, Armor, Shield, Jewelry,
        Accessory, Consumable, Scroll, Throwable, Usable, Tool,
        Number, None
    }

    public enum MaterialHardness
    {
        Cloth, Leather, Soft, Stone, Metal,
        Liquid, Paper, Food,
        Number, Any, None
    };

    public enum ItemHardnessAllowed
    {
        Soft, Hard, Soft_or_Hard, Organic, Cloth, Leather, Metal, Stone,
        Potion, Scroll, Food,
        Number, Any, None
    };

    public enum MapDirection
    {
        North, North_East, East, South_East, South, South_West, West, North_West, Center
    };

    public enum GameEventType
    {
        Story, Lore, Quest, Battle, Conquest, Defense, Siege, Rescue, Rumor, Merchant, Puzzle, Tutorial,
        Number, Blank
    };

    public enum EventDifficulty
    {
        Very_Easy, Easy, Average, Hard, Very_Hard, Impossible, Number, None
    };

    public enum EventState
    {
        Active, Inactive, Deactivate
    };

    public enum ItemNameFormat
    {
        Material_First, Material_Middle, Material_Last, Artifact,
        Number, None
    };

    public enum ItemEffectType
    {
        Attribute, Skill, Damage, Resistance, Weapon_Attribute, Armor_Attribute, Trait, Power, Spell,
        Number, None
    };

    public enum AbilityClass
    {
        World, Encounter, Either, Number, None
    };

    public enum AbilityType
    {
        Power, Spell, Trait, Positive_Quirk, Neutral_Quirk, Negative_Quirk, Wound, Defect, Number, None
    };

    public enum SpellSchoolType
    {
        Fire, Air, Water, Earth, Death, Life, Shadow, Arcane, Number, None
    }

    public enum TraitType
    { Race, Profession, Wound, Background, Misc, Number, None }

    public enum AreaType
    { Single, Sphere, Rectangle, Cone, Beam, Number, None }

    public enum DurationType
    { Instant, Permanent, Duration, Number, None }

    public enum RangeType
    { Self, Distance, Touch, Weapon, Number, None }

    public enum TargetType
    { Self, Any, Friend, Enemy, Number, None }

    public enum TimeType
    { Minute, Hour, Day, Month, Year, Turn, Number, None }

    public enum TriggerType
    {
        Always_On, Use, Cast, Channel, On_Attack, On_Damage, On_Miss, On_Defense, On_Dodge, On_Block, On_Damaged,
        Number, None
    }
    public enum ResearchCategory
    {
        Stronghold, Library, Stockpile, Armory, Barracks, Codex, Combat, Magic,
        Number, None
    }

    public enum EntryUnlockType
    {
        Power, Spell, Trait, Building, Item_Material, Item, Item_Enchantment, Profession, Race,
        None
    }

    public enum ResearchStatus { Active, Deactive, Researched, Researching };

    public enum RewardType
    {
        Resource, Item, Exp, Unit, Unlock,
        Number, None
    };

    public enum RollType
    {
        Percentile, Attribute, Skill, Attack, Defense, Resistance,
        Number, None
    }
    public enum CheckType
    {
        Party_Highest, Party_Combined, Party_Individual, Character, Leader, Number, None
    }

    public enum ActionType
    {
        Movement, Weapon_Attack, Unarmed_Attack, Power, Spell, Item,
        Number, None
    }
}