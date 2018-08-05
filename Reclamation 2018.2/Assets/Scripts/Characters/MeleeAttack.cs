using UnityEngine;
using Reclamation.Audio;
using Reclamation.Gui;
using Reclamation.Misc;

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

        public CharacterData data;

        /// <summary>
        /// Initialize the default values.
        /// </summary>
        private void Awake()
        {
            lastAttackTime = -repeatAttackDelay;
        }

        public void SetController(CharacterData data)
        {
            this.data = data;

            if (this.data == null) Debug.Log("this.data == null");
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
            if (data.IsDead == true) return;

            Damagable damagable = defender.GetComponent<Damagable>();

            if (damagable == null)
            {
                Debug.Log("damagable == null");
                return;
            }

            //AudioManager.instance.PlaySound("male attack 01");
            AudioManager.instance.PlaySound("sword 01", true);

            if (Random.Range(0, 100) > 65)
            {
                int dmg = Random.Range(1, 10);
                if(data != null && data.Inventory != null && data.Inventory.EquippedItems[(int)EquipmentSlot.Right_Hand] != null)
                    dmg = data.Inventory.EquippedItems[(int)EquipmentSlot.Right_Hand].WeaponData.Damage[0].DamageDice.Roll(false);

                lastAttackTime = Time.time;
                damagable.Damage(dmg);
                string red = "<color=#00ff26>";
                string green = "<color=#ff0000>";

                string attColor = "";
                string defColor = "";
                string dmgColor = "<color=#ff9000>";

                if (damagable.data.Faction.Equals("Player") == true)
                {
                    attColor = green;
                    defColor = red;
                }
                else if (damagable.data.Faction.Equals("Enemy") == true)
                {
                    attColor = red;
                    defColor = green;
                }
                else if (damagable.data.Faction.Equals("Neutral") == true)
                {
                    attColor = green;
                    defColor = red;
                }

                string message = "";
                message += attColor + data.Name.FirstName + "</color> hit ";
                message += defColor + damagable.data.Name.FirstName + "</color> for ";
                message += dmgColor + dmg + "</color> damage"; 

                MessageSystem.instance.AddMessage(message);

            }
        }
    }
}