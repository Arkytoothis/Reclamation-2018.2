﻿using UnityEngine;
using System.Collections;
using System.Runtime.Serialization;

public enum BaseAttribute
{
    Strength, Endurance, Agility, Dexterity, Senses, Intellect, Wisdom, Willpower, Charisma, Memory,
    Number, None
}

public enum DerivedAttribute
{
    Actions, Armor, Health, Energy, Mana, Morale, 
    Might_Attack, Might_Damage, Finesse_Attack, Finesse_Damage, Spell_Attack, Spell_Damage, Spell_Modifier,
    Block, Dodge, Parry, Resistance, Speed, Detection_Range, 
    Action_Modifier, Duration_Modifier, Range_Modifier, Magic_Find,
    Fumble, Graze, Critical_Strike, Perfect_Strike, Critical_Damage,
    Health_Regen, Energy_Regen, Mana_Regen,
    Number, None
}

public enum DamageType
{
    Physical, Fire, Shock, Cold, Poison, Acid, Unholy, Holy, Psychic, Arcane,
    Number, None
}

public enum Skill
{
    One_Hand_Melee, Two_hand_Melee, Polearms, Unarmed, Thrown, Archery, Firearms, Explosives,
    Light_Armor, Medium_Armor, Heavy_Armor, Bucklers, Shields, Leadership, Tactics,
    Fire_Magic, Air_Magic, Water_Magic, Earth_Magic, Death_Magic, Life_Magic, Shadow_Magic, Arcane_Magic,
    Alchemy, Enchanting, Lore, Research, Channeling, 
    Stealth, Perception, Tricks, Evasion, Precision, Devices, Persuasion, Poison_Craft, 
    Mining, Gathering, Crafting, Engineering, Logistics, Steamcraft, Survival, Navigation, Training, Medicine,
    Number, None
}

public enum PartyAttribute
{
    March_Speed, Scout_Range, Supplies, Supplies_Used, Max_Supplies, Rations, Rations_Used, Max_Rations, 
    Number, None
}

public enum WeaponAttributes
{
    Actions, Attack, Parry, Range, 
    Number, None
}

public enum AmmoAttributes
{
    Actions, Attack, Range, 
    Number, None
}

public enum WearableAttributes
{
    Actions, Armor, Block, Dodge, 
    Number, None
}

public enum AccessoryAttributes
{
    Actions, Cooldown,
    Number, None
}

[System.Serializable]
public class Characteristic
{
    public CharacteristicType Type;
    public int Index;
    private int start;
    private int current;
    private int modifier;
    private int minimum;
    private int maximum;
    private int spent;
    private int expCost;

    public int Start { get { return start; } }
    public int Current { set { current = value; }  get { return current; } }
    public int Modifier { get { return modifier; } }
    public int Minimum { get { return minimum; } }
    public int Maximum { get { return maximum; } }
    public int Spent { get { return spent; } }
    public int ExpCost { get { return expCost; } }

    public int ExpModifier;

    public Characteristic()
    {
        Type = CharacteristicType.None;
        Index = 0;
        start = 0;
        current = 0;
        modifier = 0;
        minimum = 0;
        maximum = 0;
        spent = 0;
        expCost = 0;
        ExpModifier = 0;
    }

    public Characteristic(CharacteristicType type, int index, int exp_mod)
    {
        Type = type;
        Index = index;
        start = 0;
        current = 0;
        modifier = 0;
        minimum = 0;
        maximum = 0;
        spent = 0;
        ExpModifier = exp_mod;
    }
    public Characteristic(Characteristic c)
    {
        Type = c.Type;
        Index = c.Index;
        start = c.start;
        current = c.current;
        modifier = c.modifier;
        minimum = c.minimum;
        maximum = c.maximum;
        spent = c.spent;
        expCost = c.expCost;
        ExpModifier = c.ExpModifier;
    }

    public Characteristic(CharacteristicType type, int index, int start, int current, int modifier, int minimum, int maximum, int exp_mod)
    {
        Type = type;
        Index = index;
        this.start = start;
        this.current = current;
        this.modifier = modifier;
        this.minimum = minimum;
        this.maximum = maximum;
        spent = 0;
        ExpModifier = exp_mod;
    }

    public void Reset()
    {
        current = maximum;
    }

    public static Characteristic Max { get { return new Characteristic(CharacteristicType.Base_Attribute, 0, 999999, 999999, 999999, 0, 999999, 999999); } }

    public void SetStart(int start, int min, int max)
    {
        this.start = start;
        this.minimum = min;
        this.maximum = max;
        this.current = start;
        this.spent = 0;
        Check();
    }

    public void SetStart(int start)
    {
        this.start = start;
        this.minimum = 0;
        this.maximum = start;
        this.current = start;
        this.spent = 0;
        Check();
    }

    public void SetMax(int max, bool set_cur)
    {
        maximum = max;

        if (set_cur == true)
            current = maximum;
    }

    public void ModifyStart(int value)
    {
        start += value;
        maximum = start;
        current = start;
        Check();
    }

    public void SetModifier(int mod)
    {
        modifier = mod;
    }

    public void AddToModifier(int mod)
    {
        modifier += mod;
    }

    public void AddSpent()
    {
        spent++;
        current = start + spent;
    }

    public void SubtractSpent()
    {
        spent--;
        current = start + spent;
    }

    void Check()
    {
        if (this.current < this.minimum)
            this.current = this.minimum;

        if (this.start < this.minimum)
            this.start = this.minimum;
    }

    public void SetExpModifier(int mod)
    {
        ExpModifier = mod;
    }

    public void CalculateExpCost()
    {
        expCost = current * ExpModifier;
    }
}