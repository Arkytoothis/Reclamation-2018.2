using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Race
{
    public string Name;
    public string Key;
    public string Description;

    public string MaleSprite;
    public string FemaleSprite;
    public string WingsSprite;
    public string HornsSprite;

    public int BaseSpeed;
    public float ExpModifier;
    public GameValue HealthPerLevel;
    public GameValue EnergyPerLevel;

    public bool Short;
    public int Hair;
    public int Beard;

    public bool HelmAllowed;
    public bool PantsAllowed;
    public bool ShoesAllowed;

    public List<GameValue> StartingAttributes;
    public List<SkillProficiency> SkillProficiencies;
    public List<ResistanceData> Resistances;

    public List<AbilityUnlock> Traits;
    public List<AbilityUnlock> Powers;
    public List<AbilityUnlock> Spells;

    public UpkeepData Upkeep;
    public GameValue StartingWealth;

    public Race()
    {
        Name = "";
        Key = "";
        Description = "empty";


        StartingAttributes = new List<GameValue>();
        SkillProficiencies = new List<SkillProficiency>();
        Resistances = new List<ResistanceData>();

        Traits = new List<AbilityUnlock>();
        Powers = new List<AbilityUnlock>();
        Spells = new List<AbilityUnlock>();

        ExpModifier = 1.0f;
        BaseSpeed = 10;

        Upkeep = new UpkeepData();
        StartingWealth = new GameValue();
    }

    public Race(string name, string key, string male, string female, bool small, string wings, string horns, int hair, int beard, bool helm, bool pants, bool shoes,
                GameValue health, GameValue energy, int movement, float exp_mod, UpkeepData upkeep, GameValue wealth)
    {
        Name = name;
        Key = key;
        Description = "empty";
        MaleSprite = male;
        FemaleSprite = female;

        Short = small;
        WingsSprite = wings;
        HornsSprite = horns;
        Hair = hair;
        Beard = beard;

        HelmAllowed = helm;
        PantsAllowed = pants;
        ShoesAllowed = shoes;

        StartingAttributes = new List<GameValue>();
        for (int i = 0; i < (int)BaseAttribute.Number; i++)
        {
            StartingAttributes.Add(new GameValue(5, 4));
        }

        SkillProficiencies = new List<SkillProficiency>();
        Resistances = new List<ResistanceData>();

        Traits = new List<AbilityUnlock>();
        Powers = new List<AbilityUnlock>();
        Spells = new List<AbilityUnlock>();

        HealthPerLevel = new GameValue(health.Number, health.Die);
        EnergyPerLevel = new GameValue(energy.Number, energy.Die);

        ExpModifier = exp_mod;
        BaseSpeed = movement;

        Upkeep = new UpkeepData(upkeep);
        StartingWealth = new GameValue(wealth);
    }

    public bool ArmorSlotAllowed(EquipmentSlot slot)
    {
        bool allowed = true;

        if (slot == EquipmentSlot.Head && HelmAllowed == false)
            allowed = false;

        if (slot == EquipmentSlot.Feet && ShoesAllowed == false)
            allowed = false;

        return allowed;
    }
}