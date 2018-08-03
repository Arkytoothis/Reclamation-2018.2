using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reclamation.Misc;

namespace Reclamation.Characters
{
    public abstract class CharacterController : MonoBehaviour
    {
        public AttributeManager Attributes;
        public abstract bool CheckIsAlive();

        public abstract void ModifyAttribute(AttributeType type, int attribute, int value);
        public delegate void OnDeath();
        public delegate void OnRevive();
        public delegate void OnInteract();
        public delegate void OnAttack();

        public abstract void Death();
        public abstract void Revive();
        public abstract void Interact();
        public abstract void Attack();
    }
}