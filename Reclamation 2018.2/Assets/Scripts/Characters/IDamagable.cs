﻿namespace Reclamation.Characters
{
    /// <summary>
    /// Interface for objects that can take damage.
    /// </summary>
    public interface IDamageable
    {
        /// <summary>
        /// Take damage by the specified amount.
        /// </summary>
        /// <param name="amount">The amount of damage to take.</param>
        void Damage(int amount);

        /// <summary>
        /// Is the object currently alive?
        /// </summary>
        /// <returns>True if the object is alive.</returns>
        bool IsAlive();

        void SetCharacterData(CharacterData character);
    }
}