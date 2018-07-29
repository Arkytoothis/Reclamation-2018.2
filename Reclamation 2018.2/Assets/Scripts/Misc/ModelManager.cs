using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reclamation.Characters;
using Reclamation.Encounter;

namespace Reclamation.Misc
{
    public class ModelManager : Singleton<ModelManager>
    {
        public GameObject malePrefab;
        public GameObject femalePrefab;

        public void Initialize()
        {

        }

        public GameObject SpawnPc(Transform parent, Pc pc)
        {
            GameObject go = null;

            if (pc.Gender == Gender.Male)
            {
                go = Instantiate(malePrefab, parent);
            }
            else if (pc.Gender == Gender.Female)
            {
                go = Instantiate(femalePrefab, parent);
            }

            go.GetComponent<EncounterPcController>().SetPcData(pc);

            return go;
        }

        public GameObject GetPrefab(Pc pc)
        {
            if (pc.Gender == Gender.Male)
            {
                return malePrefab;
            }
            else if (pc.Gender == Gender.Female)
            {
                return femalePrefab;
            }
            else
            {
                return malePrefab;
            }
        }
    }
}