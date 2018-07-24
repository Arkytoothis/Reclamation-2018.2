using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResearchEntryUnlock
{
    public EntryUnlockType Type;
    public string Key;
    public int Value;

    public ResearchEntryUnlock()
    {
        Type = EntryUnlockType.None;
        Key = "";
        Value = 0;
    }

    public ResearchEntryUnlock(EntryUnlockType type, string key, int value = 0)
    {
        Type = type;
        Key = key;
        Value = value;
    }

    public ResearchEntryUnlock(ResearchEntryUnlock unlock)
    {
        Type = unlock.Type;
        Key = unlock.Key;
        Value = unlock.Value;
    }
}
