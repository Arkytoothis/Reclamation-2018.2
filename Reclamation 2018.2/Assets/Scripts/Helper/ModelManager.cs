using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ModelManager : Singleton<ModelManager>
{
    public GameObject malePrefab;
    public GameObject femalePrefab;

    public void Initialize()
    {

    }

    public GameObject SpawnCharacter(Transform parent, Pc pc)
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