using UnityEngine;
using System.Collections;

namespace Reclamation.Equipment
{
    public class IngredientData
    {
        public string Name;
        public string Key;

        public IngredientData()
        {
            Name = "";
            Key = "";
        }

        public IngredientData(string name, string key)
        {
            Name = name;
            Key = key;
        }

        public IngredientData(IngredientData data)
        {
            Name = data.Name;
            Key = data.Key;
        }
    }
}