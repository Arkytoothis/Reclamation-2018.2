using UnityEngine;
using Reclamation.Audio;
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
        public CharacterData data;
        public Transform textTransform;

        void Start()
        {
        }

        public void SetController(CharacterData data)
        {
            this.data = data;
        }

        /// <summary>
        /// Take damage. Deactivate if the amount of remaining health is 0.
        /// </summary>
        /// <param name="amount"></param>
        public void Damage(int amount)
        {
            if (amount > 0)
            {
                data.Attributes.ModifyAttribute(AttributeType.Derived, (int)DerivedAttribute.Health, -amount);

                EncounterManager.instance.textManagerWorld.Add(amount.ToString(), textTransform, "damage");
                AudioManager.instance.PlaySound("impact 01", true);
                ParticleManager.instance.SpawnEffect("hit", transform);
            }
        }

        public void Heal(int amount)
        {
            int current = data.GetDerived((int)DerivedAttribute.Health).Current;
            int max = data.GetDerived((int)DerivedAttribute.Health).Maximum;

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
                data.Attributes.ModifyAttribute(AttributeType.Derived, (int)DerivedAttribute.Health, amount);

                EncounterManager.instance.textManagerWorld.Add(amount.ToString(), textTransform, "heal");
                AudioManager.instance.PlaySound("heal 01", false);
                ParticleManager.instance.SpawnEffect("circular light wall", transform);
            }
        }

        // Is the object alive?
        public bool IsAlive()
        {
            return !data.IsDead;
        }

        /// <summary>
        /// Sets the current health to the starting health and enables the object.
        /// </summary>
        public void ResetHealth()
        {
            data.Attributes.ModifyAttribute(Misc.AttributeType.Derived, (int)DerivedAttribute.Health, data.GetDerived((int)DerivedAttribute.Health).Maximum);
            gameObject.SetActive(true);
        }
    }
}