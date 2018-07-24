using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResistanceData
{
    public DamageType DamageType;
    public int Value;

    public ResistanceData()
    {
        DamageType = DamageType.None;
        Value = 0;
    }

    public ResistanceData(DamageType type, int value)
    {
        DamageType = type;
        Value = value;
    }

    public ResistanceData(ResistanceData data)
    {
        DamageType = data.DamageType;
        Value = data.Value;
    }
}