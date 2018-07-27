using UnityEngine;
using System.Collections.Generic;
using Reclamation.Misc;
using Reclamation.Characters;

namespace Reclamation.Equipment
{
    [System.Serializable]
    public class AccessoryData
    {
        public AccessoryType Type;
        public List<ItemAttribute> Attributes;

        public AccessoryData()
        {
            Type = AccessoryType.None;
            Attributes = new List<ItemAttribute>();
        }

        public AccessoryData(AccessoryType type, int actions)
        {
            Type = type;
            Attributes = new List<ItemAttribute>();
            Attributes.Add(new ItemAttribute((int)AccessoryAttributes.Actions, actions));
            Attributes.Add(new ItemAttribute((int)AccessoryAttributes.Cooldown, actions));
        }

        public AccessoryData(AccessoryData data)
        {
            Type = data.Type;

            Attributes = new List<ItemAttribute>();
            for (int i = 0; i < data.Attributes.Count; i++)
            {
                Attributes.Add(data.Attributes[i]);
            }
        }

        public string GetText()
        {
            string s = "";

            s += " - " + Type;

            if (Attributes[(int)AccessoryAttributes.Actions].Value != 1)
                s += "\n" + Attributes[(int)AccessoryAttributes.Actions].Value + " Actions";
            else
                s += "\n" + Attributes[(int)AccessoryAttributes.Actions].Value + " Action";

            s += "\nCooldown " + Attributes[(int)AccessoryAttributes.Cooldown].Value + " turns";
            return s;
        }
    }
}