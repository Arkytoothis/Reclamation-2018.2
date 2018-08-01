using UnityEngine;
using BehaviorDesigner.Runtime.Tactical;
using Reclamation.Misc;

namespace Reclamation.Characters
{
    /// <summary>
    /// Example component which adds health to an object.
    /// </summary>
    public class Damagable : MonoBehaviour, IDamageable
    {
        [SerializeField] CharacterData characterData;
        public CharacterData CharacterData { get { return characterData; } }

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
            //Debug.Log(characterData.name.FirstName + " has taken " + amount + " damage");
            characterData.ModifyAttribute(Misc.AttributeType.Derived, (int)DerivedAttribute.Health, -amount);

            if (characterData.GetDerived((int)DerivedAttribute.Health).Current <= 0)
            {
                //Debug.Log("Dead");
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