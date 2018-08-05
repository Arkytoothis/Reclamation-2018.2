using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reclamation.Party
{
    public class PartyController : MonoBehaviour
    {
        public T GetPartyComponent<T>()
        {
            T t = GetComponentInChildren<T>();

            if (t == null)
                Debug.LogWarning("Component " + typeof(T).ToString() + " not found");

            return t;
        }
    }
}