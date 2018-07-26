using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : Singleton<WorldManager>
{
    public GameObject[] worldSitePrefabs;
    public List<GameObject> worldSites;
    public float seaLevel = 1f;

    private List<Vector3> worldSitePositions;

    public void Initialize()
    {
        worldSitePositions = new List<Vector3>();
        FindWorldSiteSpawns();
        SpawnWorldSite(10);
    }

    public void FindWorldSiteSpawns()
    {
        for (int i = 0; i < 250; i++)
        {
            float x = Random.Range(0f, 250f);
            float z = Random.Range(0f, 250f);
            float y = Terrain.activeTerrain.SampleHeight(new Vector3(x, 0, z));

            if (y > seaLevel)
            {
                worldSitePositions.Add(new Vector3(x, y, z));
            }
        }
    }

    public void SpawnWorldSite(int numToSpawn)
    {
        for (int i = 0; i < numToSpawn; i++)
        {
            int prefabIndexToSpawn = Random.Range(0, worldSitePrefabs.Length);

            GameObject go = Instantiate(worldSitePrefabs[prefabIndexToSpawn], transform);

            int positionIndex = Random.Range(0, worldSitePositions.Count);
            Vector3 positionToSpawn = worldSitePositions[positionIndex];

            go.transform.position = positionToSpawn;

            worldSites.Add(go);
        }
    }
}