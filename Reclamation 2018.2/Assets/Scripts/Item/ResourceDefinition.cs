using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResourceDefinition
{
    public string Name;
    public string Key;
    public string Icon;
    public string Description;
    public int Index;

    public ResourceDefinition()
    {
        Name = "";
        Key = "";
        Icon = "";
        Description = "";
        Index = -1;
    }

    public ResourceDefinition(string name, string key, string icon, string description, int index)
    {
        Name = name;
        Key = key;
        Icon = icon;
        Description = description;
        Index = index;
    }
}