using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reclamation.Misc;

namespace Reclamation.Characters
{
    [System.Serializable]
    public class CharacteristicModifier
    {
        public CharacteristicType Type;
        public int Attribute;

        public CharacteristicModifier()
        {
            Type = CharacteristicType.None;
            Attribute = 0;
        }

        public CharacteristicModifier(CharacteristicType type, int attribute)
        {
            Type = type;
            Attribute = attribute;
        }

        public CharacteristicModifier(CharacteristicModifier mod)
        {
            Type = mod.Type;
            Attribute = mod.Attribute;
        }
    }
}