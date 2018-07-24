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

    public void SpawnCharacter(Transform parent, PC pc)
    {
        //GameObject go = null;

        if (pc.Gender == Gender.Male)
        {
            Instantiate(malePrefab, parent);
        }
        else if (pc.Gender == Gender.Female)
        {
            Instantiate(femalePrefab, parent);
        }
    }

    public GameObject GetPrefab(PC pc)
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