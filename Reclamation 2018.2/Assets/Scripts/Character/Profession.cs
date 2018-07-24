using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Profession
{
    public string Name;
    public string Key;
    public string Description;
    public GameValue ActionsPerLevel;
    public GameValue HealthPerLevel;
    public GameValue EnergyPerLevel;
    public GameValue ManaPerLevel;

    public List<int> MinimumAttributes;
    public List<SkillProficiency> SkillProficiencies;
    public List<BaseAttribute> AttributePriorities;
    public List<ItemShort> StartingItems;

    public List<AbilityUnlock> Traits;
    public List<AbilityUnlock> Powers;
    public List<AbilityUnlock> Spells;

    public int BaseHireCost;
    public UpkeepData Upkeep;
    public GameValue StartingWealth;

    public Profession()
    {
        Name = "";
        Key = "";
        Description = "empty";

        ActionsPerLevel = new GameValue();
        HealthPerLevel = new GameValue();
        EnergyPerLevel = new GameValue();
        ManaPerLevel = new GameValue();

        MinimumAttributes = new List<int>();
        AttributePriorities = new List<BaseAttribute>();
        SkillProficiencies = new List<SkillProficiency>();
        StartingItems = new List<ItemShort>();

        Traits = new List<AbilityUnlock>();
        Powers = new List<AbilityUnlock>();
        Spells = new List<AbilityUnlock>();

        BaseHireCost = 0;
        Upkeep = new UpkeepData();
        StartingWealth = new GameValue();
    }

    public Profession(string name, string key, string description, int hire_cost, UpkeepData upkeep, GameValue wealth)
    {
        Name = name;
        Key = key;
        Description = description;

        ActionsPerLevel = new GameValue();
        HealthPerLevel = new GameValue();
        EnergyPerLevel = new GameValue();
        ManaPerLevel = new GameValue();

        MinimumAttributes = new List<int>();

        int index = 0;
        for (int i = 0; i < (int)BaseAttribute.Number; i++)
        {
            MinimumAttributes.Add(0);
            index++;
        }

        AttributePriorities = new List<BaseAttribute>();

        SkillProficiencies = new List<SkillProficiency>();

        StartingItems = new List<ItemShort>();

        Traits = new List<AbilityUnlock>();
        Powers = new List<AbilityUnlock>();
        Spells = new List<AbilityUnlock>();

        BaseHireCost = hire_cost;
        Upkeep = new UpkeepData(upkeep);
        StartingWealth = new GameValue(wealth);
    }
}