using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reclamation.Misc;

namespace Reclamation.World
{
    [System.Serializable]
    public class ResearchEntry
    {
        public string Name;
        public string Key;
        public string Description;

        public ResearchCategory Category;
        public string Branch;

        public int ResearchHours;
        public int Tier;
        public string PrerequisiteEntry;

        public List<ResourceData> ResourcesRequired;
        public List<string> EntriesEnabled;
        public List<ResearchEntryUnlock> Unlocks;

        public ResearchEntry()
        {
            Name = "";
            Key = "";
            Description = "empty";
            Branch = "";
            ResearchHours = 0;
            Category = ResearchCategory.None;
            PrerequisiteEntry = "";
            Tier = 1;
            ResourcesRequired = new List<ResourceData>();
            EntriesEnabled = new List<string>();
            Unlocks = new List<ResearchEntryUnlock>();
        }

        public ResearchEntry(string name, string key, string branch, int tier, int hours, ResearchCategory category,
            string pq, List<ResourceData> resources, List<string> entries_enabled = null, List<ResearchEntryUnlock> unlocks = null)
        {
            Name = name;
            Key = key;
            Description = "empty";
            Branch = branch;
            Tier = tier;
            ResearchHours = hours;
            Category = category;
            PrerequisiteEntry = pq;
            ResourcesRequired = new List<ResourceData>();

            for (int i = 0; i < resources.Count; i++)
            {
                if (resources[i] != null)
                    ResourcesRequired.Add(new ResourceData(resources[i]));
                else
                    ResourcesRequired.Add(null);
            }

            EntriesEnabled = new List<string>();
            if (entries_enabled != null)
            {
                for (int i = 0; i < entries_enabled.Count; i++)
                {
                    EntriesEnabled.Add(entries_enabled[i]);
                }
            }

            Unlocks = new List<ResearchEntryUnlock>();
            if (unlocks != null)
            {
                for (int i = 0; i < unlocks.Count; i++)
                {
                    Unlocks.Add(new ResearchEntryUnlock(unlocks[i]));
                }
            }
        }

        public ResearchEntry(ResearchEntry entry)
        {
            Name = entry.Name;
            Key = entry.Key;
            Description = entry.Description;

            Branch = entry.Branch;
            Tier = entry.Tier;
            ResearchHours = entry.ResearchHours;
            Category = entry.Category;
            PrerequisiteEntry = entry.PrerequisiteEntry;
            ResourcesRequired = new List<ResourceData>();

            for (int i = 0; i < entry.ResourcesRequired.Count; i++)
            {
                if (entry.ResourcesRequired[i] != null)
                    ResourcesRequired.Add(new ResourceData(entry.ResourcesRequired[i]));
                else
                    ResourcesRequired.Add(null);
            }

            EntriesEnabled = new List<string>();
            if (entry.EntriesEnabled != null)
            {
                for (int i = 0; i < entry.EntriesEnabled.Count; i++)
                {
                    EntriesEnabled.Add(entry.EntriesEnabled[i]);
                }
            }

            Unlocks = new List<ResearchEntryUnlock>();
            if (entry.Unlocks != null)
            {
                for (int i = 0; i < entry.Unlocks.Count; i++)
                {
                    Unlocks.Add(new ResearchEntryUnlock(entry.Unlocks[i]));
                }
            }
        }

        public string GetUnlocksString()
        {
            string s = "";

            for (int i = 0; i < Unlocks.Count; i++)
            {
                s += Unlocks[i].Type + ": " + Unlocks[i].Key + "\n";
            }

            return s;
        }
    }
}