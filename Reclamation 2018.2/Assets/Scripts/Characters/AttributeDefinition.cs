using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Reclamation.Misc;

namespace Reclamation.Characters
{
    [System.Serializable]
    public class AttributeList
    {
        public List<AttributeDefinition> Attributes;

        public AttributeList()
        {
            Attributes = new List<AttributeDefinition>();
        }
    }

    [System.Serializable]
    public class AttributeDefinition
    {
        public AttributeType Type;
        public string Name;
        public string ShortName;
        public string Abbreviated;
        public string Description;
        public int Minimum;
        public int Maximum;

        public AttributeCalculation Calculation;

        public AttributeDefinition()
        {
            Type = AttributeType.None;
            Name = "";
            ShortName = "";
            Abbreviated = "";
            Description = "";
            Minimum = 0;
            Maximum = 0;
            Calculation = new AttributeCalculation();
        }

        public AttributeDefinition(string name, string short_name, string abbreviated, string description, int minimum, int maximum, AttributeType type,
            AttributeCalculation calc)
        {
            Name = name;
            ShortName = short_name;
            Abbreviated = abbreviated;
            Description = description;
            Type = type;
            Minimum = minimum;
            Maximum = maximum;
            Calculation = new AttributeCalculation(calc);
        }

        public AttributeDefinition(AttributeDefinition def)
        {
            Name = def.Name;
            ShortName = def.ShortName;
            Abbreviated = def.Abbreviated;
            Description = def.Description;
            Type = def.Type;
            Minimum = def.Minimum;
            Maximum = def.Maximum;
            Calculation = new AttributeCalculation(def.Calculation);
        }
    }
}