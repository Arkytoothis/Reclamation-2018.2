﻿using UnityEngine;
using BehaviorDesigner.Runtime.Tactical;

namespace Reclamation.Characters
{
    [System.Serializable]
    /// <summary>
    /// Example component which will attack by firing a bullet.
    /// </summary>
    public class MeleeAttack : MonoBehaviour, IAttack
    {
        // The furthest distance that the agent is able to attack from
        public float attackDistance = 1f;
        // The amount of time it takes for the agent to be able to attack again
        public float repeatAttackDelay;
        // The maximum angle that the agent can attack from
        public float attackAngle;

        // The last time the agent attacked
        private float lastAttackTime;

        private CharacterData characterData;

        /// <summary>
        /// Initialize the default values.
        /// </summary>
        private void Awake()
        {
            lastAttackTime = -repeatAttackDelay;
        }

        public void SetCharacterData(CharacterData character)
        {
            characterData = character;
        }

        /// <summary>
        /// Returns the furthest distance that the agent is able to attack from.
        /// </summary>
        /// <returns>The distance that the agent can attack from.</returns>
        public float AttackDistance()
        {
            return attackDistance;
        }

        /// <summary>
        /// Can the agent attack?
        /// </summary>
        /// <returns>Returns true if the agent can attack.</returns>
        public bool CanAttack()
        {
            bool canAttack = lastAttackTime + repeatAttackDelay < Time.time;
            return canAttack;
        }

        /// <summary>
        /// Returns the maximum angle that the agent can attack from.
        /// </summary>
        /// <returns>The maximum angle that the agent can attack from.</returns>
        public float AttackAngle()
        {
            return attackAngle;
        }

        /// <summary>
        /// Does the actual attack. 
        /// </summary>
        /// <param name="targetPosition">The position to attack.</param>
        public void Attack(GameObject defender)
        {
            Damagable damagable = defender.GetComponent<Damagable>();

            if (damagable == null)
            {
                Debug.Log("damagable == null");
                return;
            }

            if (Random.Range(0, 100) > 65)
            {
                int damage = Random.Range(1, 10);
                //Debug.Log(characterData.name.FirstName + " hit " + damagable.CharacterData.name.FirstName + " for " + damage + " damage");
                lastAttackTime = Time.time;
                damagable.Damage(damage);
            }
        }
    }
}