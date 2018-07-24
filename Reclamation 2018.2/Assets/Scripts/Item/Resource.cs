using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Resource
{
    public string Key;
    public int Stored;
    public int Income;

    public Resource()
    {
        Key = "";
        Stored = 0;
        Income = 0;
    }

    public Resource(string key, int stored, int income)
    {
        Key = key;
        Stored = stored;
        Income = income;
    }

    public Resource(Resource resource)
    {
        Key = resource.Key;
        Stored = resource.Stored;
        Income = resource.Income;
    }
}