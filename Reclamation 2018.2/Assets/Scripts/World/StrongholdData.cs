using UnityEngine;
using System.Collections.Generic;

namespace Reclamation.World
{
    [System.Serializable]
    public class StrongholdData : MonoBehaviour
    {
        [SerializeField] new string name;

        public string Name { get { return name; } } 
    }
}