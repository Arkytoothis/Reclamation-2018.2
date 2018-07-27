using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reclamation.Misc;
using Reclamation.Name;

namespace Reclamation.Characters
{
    [System.Serializable]
    public abstract class BaseCharacter
    {
        public FantasyName Name;
        public Gender Gender;
        public Background Background;
        public CharacterPersonality Personality;

        public CharacterType Type;
        public EntitySize Size;

        public string RaceKey;
        public string ProfessionKey;

        public List<Characteristic> BaseAttributes;
        public List<Characteristic> DerivedAttributes;
        public List<Characteristic> Resistances;
        public List<Characteristic> Skills;

        public int Hair;
        public int Beard;

        public string Description;

        public CharacterInventory Inventory;
        public Vector3 position;
    }
}