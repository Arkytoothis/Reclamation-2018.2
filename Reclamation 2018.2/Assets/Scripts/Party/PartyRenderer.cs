using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reclamation.Party
{
    public class PartyRenderer : MonoBehaviour
    {
        [SerializeField] GameObject model;

        public void SetModel(GameObject model)
        {
            this.model = model;
        }
    }
}