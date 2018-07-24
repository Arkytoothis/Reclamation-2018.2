using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingStatus { Building, Enabled, Disabled, Damaged, Destroyed, Number, None}

public class Building
{
    public string Key;
    public string Description;
    public string Icon;

    public BuildingStatus Status;

    public int DurabilityCur;
    public int DurabilityMax;
    public List<ResourceData> ResourcesToBuild;
    public List<ResourceData> ResourcesUsed;
    public List<ResourceData> ResourcesGenerated;

    public Building()
    {
        Key = "";
        Description = "empty";
        Icon = "";
        Status = BuildingStatus.None;
        ResourcesToBuild = new List<ResourceData>();
        ResourcesUsed = new List<ResourceData>();
        ResourcesGenerated = new List<ResourceData>();
    }

    public Building(BuildingDefinition def, BuildingStatus status, int durability)
    {
        if (Database.Buildings.ContainsKey(def.Key) == false)
        {
            Debug.LogWarning("Key " + def.Key + " does not exist");
            return;
        }

        Key = def.Key;
        Description = def.Description;
        Icon = def.Icon;
        Status = status;
        DurabilityMax = def.DurabilityMax;
        DurabilityCur = durability;

        ResourcesToBuild = new List<ResourceData>();
        for (int i = 0; i < def.ResourcesToBuild.Count; i++)
        {
            ResourcesToBuild.Add(new ResourceData(def.ResourcesToBuild[i]));
        }

        ResourcesUsed = new List<ResourceData>();
        for (int i = 0; i < def.ResourcesUsed.Count; i++)
        {
            ResourcesUsed.Add(new ResourceData(def.ResourcesUsed[i]));
        }

        ResourcesGenerated = new List<ResourceData>();
        for (int i = 0; i < def.ResourcesGenerated.Count; i++)
        {
            ResourcesGenerated.Add(new ResourceData(def.ResourcesGenerated[i]));
        }
    }

    //public Building(string key, string description, string icon, BuildingStatus status, int cur_durabilty, int max_durability)
    //{
    //    if(Database.Buildings.ContainsKey(key) == false)
    //    {
    //        Debug.LogWarning("Key " + key + " does not exist");
    //        return;
    //    }

    //    Key = key;
    //    Description = description;
    //    Icon = icon;
    //    Status = status;
    //    DurabilityMax = max_durability;
    //    DurabilityCur = cur_durabilty;

    //    ResourcesToBuild = new List<ResourceData>();
    //    for (int i = 0; i < Database.Buildings[key].ResourcesToBuild.Count; i++)
    //    {
    //        ResourcesToBuild.Add(new ResourceData(Database.Buildings[key].ResourcesToBuild[i]));
    //    }

    //    ResourcesUsed = new List<ResourceData>();
    //    for (int i = 0; i < Database.Buildings[key].ResourcesUsed.Count; i++)
    //    {
    //        ResourcesUsed.Add(new ResourceData(Database.Buildings[key].ResourcesUsed[i]));
    //    }

    //    ResourcesGenerated = new List<ResourceData>();
    //    for (int i = 0; i < Database.Buildings[key].ResourcesGenerated.Count; i++)
    //    {
    //        ResourcesGenerated.Add(new ResourceData(Database.Buildings[key].ResourcesGenerated[i]));
    //    }
    //}

    public Building(Building building)
    {
        if(building == null)
        {
            Debug.LogWarning("building == null");
            return;
        }

        if (Database.Buildings.ContainsKey(building.Key) == false)
        {
            Debug.LogWarning("building.Key " + building.Key + " does not exist");
            return;
        }

        Key = building.Key;
        Status = building.Status;
        DurabilityMax = building.DurabilityMax;
        DurabilityCur = building.DurabilityCur;

        ResourcesToBuild = new List<ResourceData>();
        for (int i = 0; i < building.ResourcesToBuild.Count; i++)
        {
            ResourcesToBuild.Add(new ResourceData(building.ResourcesToBuild[i]));
        }

        ResourcesUsed = new List<ResourceData>();
        for (int i = 0; i < building.ResourcesUsed.Count; i++)
        {
            ResourcesUsed.Add(new ResourceData(building.ResourcesUsed[i]));
        }

        ResourcesGenerated = new List<ResourceData>();
        for (int i = 0; i < building.ResourcesGenerated.Count; i++)
        {
            ResourcesGenerated.Add(new ResourceData(building.ResourcesGenerated[i]));
        }
    }

    public string GetTooltipText()
    {
        string details = "Status " + Status.ToString();
        details += "\nDurability " + DurabilityCur + "/" + DurabilityMax;

        details += "\nMaintance ";
        foreach(ResourceData resource in ResourcesUsed)
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