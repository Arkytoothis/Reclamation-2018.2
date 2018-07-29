using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Reclamation.Characters;
using Reclamation.Misc;

namespace Reclamation.Equipment
{
    [System.Serializable]
    public class WearableData
    {
        public WearableType Type;
        public List<ItemAttribute> Attributes;
        public List<ResistanceData> Resistances;

        public WearableData()
        {
            Type = WearableType.None;
            Attributes = new List<ItemAttribute>();
            Resistances = new List<ResistanceData>();
        }

        public WearableData(WearableType type, int armor, int dodge, int block, int actions, List<ResistanceData> resistances)
        {
            Type = type;

            Attributes = new List<ItemAttribute>();
            Attributes.Add(new ItemAttribute((int)WearableAttributes.Action_Speed, actions));
            Attributes.Add(new ItemAttribute((int)WearableAttributes.Armor, armor));
            Attributes.Add(new ItemAttribute((int)WearableAttributes.Block, block));
            Attributes.Add(new ItemAttribute((int)WearableAttributes.Dodge, dodge));

            Resistances = new List<ResistanceData>();
            if (resistances != null)
            {
                for (int i = 0; i < resistances.Count; i++)
                {
                    Resistances.Add(new ResistanceData(resistances[i]));
                }
            }
        }

        public WearableData(WearableData data)
        {
            Type = data.Type;

            Attributes = new List<ItemAttribute>();
            for (int i = 0; i < data.Attributes.Count; i++)
            {
                Attributes.Add(data.Attributes[i]);

            }

            Resistances = new List<ResistanceData>();
            if (data.Resistances != null)
            {
                for (int i = 0; i < data.Resistances.Count; i++)
                {
                    Resistances.Add(new ResistanceData(data.Resistances[i]));
                }
            }
        }

        public string GetText()
        {
            string s = "";

            s += " - " + Type;
            s += "\nActions +" + Attributes[(int)WearableAttributes.Action_Speed].Value;
            s += "\n" + Attributes[(int)WearableAttributes.Dodge].Value + "% Dodge";
            s += "<pos=50%>" + Attributes[(int)WearableAttributes.Block].Value + "% Block";
            s += "\n" + Attributes[(int)WearableAttributes.Armor].Value + " Armor";
            s += "\n";

            if (Resistances != null)
            {
                for (int i = 0; i < Resistances.Count; i++)
                {
                    s += Resistances[i].Value + "% " + Resistances[i].ToString() + " ";
                }
            }

            return s;
        }
    }
}