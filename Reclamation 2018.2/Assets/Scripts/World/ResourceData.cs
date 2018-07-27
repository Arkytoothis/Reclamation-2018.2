using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reclamation.World
{
    [System.Serializable]
    public class ResourceData
    {
        public string Resource;
        public int Amount;

        public ResourceData()
        {
            Resource = "";
            Amount = 0;
        }

        public ResourceData(string resource, int amount)
        {
            Resource = resource;
            Amount = amount;
        }

        public ResourceData(ResourceData data)
        {
            Resource = data.Resource;
            Amount = data.Amount;
        }
    }
}