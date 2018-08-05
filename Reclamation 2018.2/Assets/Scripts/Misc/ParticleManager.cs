using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reclamation.Misc
{
    public class ParticleManager : Singleton<ParticleManager>
    {
        Dictionary<string, GameObject> particleEffects;

        public void Initialize()
        {
            particleEffects = new Dictionary<string, GameObject>();
            LoadEffects();
        }

        void LoadEffects()
        {
            Object[] effects = Resources.LoadAll("Particle Effects");

            foreach (GameObject go in effects)
            {
                particleEffects.Add(go.name, go);
            }
        }

        GameObject GetEffect(string key)
        {
            if (particleEffects.ContainsKey(key) == false)
            {
                return null;
            }
            else
            {
                return particleEffects[key];
            }
        }

        public void SpawnEffect(string key, Transform target)
        {
            GameObject go = CFX_SpawnSystem.GetNextObject(GetEffect(key), true);
            go.transform.position = target.position;
        }
    }
}