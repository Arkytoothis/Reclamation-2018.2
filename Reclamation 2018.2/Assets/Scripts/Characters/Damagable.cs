using UnityEngine;
using Reclamation.Encounter;
using Reclamation.Misc;
using Reclamation.Gui;

namespace Reclamation.Characters
{
    /// <summary>
    /// Example component which adds health to an object.
    /// </summary>
    public class Damagable : MonoBehaviour, IDamageable
    {
        [SerializeField] CharacterData characterData;
        public CharacterData CharacterData { get { return characterData; } }
        public Transform textTransform;

        void Start()
        {
            InvokeRepeating("Regen", 0.1f, 5f);
        }

        public void Regen()
        {
            if (characterData.GetDerived((int)DerivedAttribute.Health).Current < characterData.GetDerived((int)DerivedAttribute.Health).Maximum)
            {
                //Heal(1);
            }
        }

        public void SetCharacterData(CharacterData character)
        {
            characterData = character;
        }

        /// <summary>
        /// Take damage. Deactivate if the amount of remaining health is 0.
        /// </summary>
        /// <param name="amount"></param>
        public void Damage(int amount)
        {
            if (amount > 0)
            {
                EncounterManager.instance.textManagerWorld.Add(amount.ToString(), textTransform, "damage");
                characterData.ModifyAttribute(AttributeType.Derived, (int)DerivedAttribute.Health, -amount);
            }
        }

        public void Heal(int amount)
        {
            int current = characterData.GetDerived((int)DerivedAttribute.Health).Current;
            int max = characterData.GetDerived((int)DerivedAttribute.Health).Maximum;

            if (current + amount > max)
            {
                amount = max - current;
            }

            if (current >= max)
            {
                amount = 0;
            }

            if (amount > 0)
            {
                characterData.ModifyAttribute(AttributeType.Derived, (int)DerivedAttribute.Health, amount);
                EncounterManager.instance.textManagerWorld.Add(amount.ToString(), textTransform, "heal");
            }
        }

        // Is the object alive?
        public bool IsAlive()
        {
            return characterData.GetDerived((int)DerivedAttribute.Health).Current > 0;
        }

        /// <summary>
        /// Sets the current health to the starting health and enables the object.
        /// </summary>
        public void ResetHealth()
        {
            characterData.ModifyAttribute(Misc.AttributeType.Derived, (int)DerivedAttribute.Health, characterData.GetDerived((int)DerivedAttribute.Health).Maximum);
            gameObject.SetActive(true);
        }
    }
}