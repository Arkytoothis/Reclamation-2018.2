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

        public string useAnimation;
        public string useParticle;
        public string useSound;

        public List<ItemAttribute> Attributes;

        public AccessoryData()
        {
            Type = AccessoryType.None;
            Attributes = new List<ItemAttribute>();
        }

        public AccessoryData(AccessoryType type, int actions, string useAnimation, string useParticle, string useSound)
        {
            Type = type;

            this.useAnimation = useAnimation;
            this.useParticle = useParticle;
            this.useSound = useSound;

            Attributes = new List<ItemAttribute>();
            Attributes.Add(new ItemAttribute((int)AccessoryAttributes.Action_Speed, actions));
            Attributes.Add(new ItemAttribute((int)AccessoryAttributes.Cooldown, actions));
        }

        public AccessoryData(AccessoryData data)
        {
            Type = data.Type;

            this.useAnimation = data.useAnimation;
            this.useParticle = data.useParticle;
            this.useSound = data.useSound;

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

            if (Attributes[(int)AccessoryAttributes.Action_Speed].Value != 1)
                s += "\n" + Attributes[(int)AccessoryAttributes.Action_Speed].Value + " Actions";
            else
                s += "\n" + Attributes[(int)AccessoryAttributes.Action_Speed].Value + " Action";

            s += "\nCooldown " + Attributes[(int)AccessoryAttributes.Cooldown].Value + " turns";
            return s;
        }
    }
}