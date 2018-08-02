using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reclamation.Characters
{
    public abstract class CharacterController : MonoBehaviour
    {
        public CharacterData data;
        public abstract bool CheckIsAlive();


    }
}