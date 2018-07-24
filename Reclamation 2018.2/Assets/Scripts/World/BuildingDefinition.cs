using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuildingDefinition
{
    public string Name;
    public string Key;
    public string Description;
    public string Icon;
    public bool Unlocked;

    public int DurabilityMax;
    public int HoursToBuild;
    public List<ResourceData> ResourcesToBuild;
    public List<ResourceData> ResourcesUsed;
    public List<ResourceData> ResourcesGenerated;

    public BuildingDefinition()
    {
        Name = "";
        Key = "";
        Icon = "";
        Unlocked = false;

        DurabilityMax = 0;
        HoursToBuild = 0;
        Description = "";
    }

    public BuildingDefinition(string name, string key, string icon, int durability, int hours, bool unlocked, string description)
    {
        Name = name;
        Key = key;
        Icon = icon;
        DurabilityMax = durability;
        HoursToBuild = hours;
        Unlocked = unlocked;
        Description = description;
    }

    public string GetTooltipText()
    {
        string details = "Max Durability " + DurabilityMax;
        details += "\nHours to build " + HoursToBuild;

        details += "\nCost ";
        foreach (ResourceData resource in ResourcesToBuild)
        {
            details += " " + resource.Amount + " " + resource.Resource;
        }

        details += "\nMaintance ";
        foreach (ResourceData resource in ResourcesUsed)
        {
            details += " " + resource.Resource + " -" + resource.Amount;
        }

        details += "\nIncome ";
        foreach (ResourceData resource in ResourcesGenerated)
        {
            details += " " + resource.Resource + " +" + resource.Amount;
        }

        return details;
    }
}