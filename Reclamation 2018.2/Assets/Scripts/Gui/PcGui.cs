using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reclamation.Characters;

namespace Reclamation.Gui
{
    public class PcGui : MonoBehaviour
    {
        public GameObject vitalBarPrefab;
        public WorldSpaceAttributeBar barInstance;

        Camera cameraToLookAt;

        void Awake()
        {
            cameraToLookAt = Camera.main;
        }

        public void SpawnHealthBar()
        {
            GameObject go = Instantiate(vitalBarPrefab, transform.position, Quaternion.identity, transform);
            barInstance = go.GetComponent<WorldSpaceAttributeBar>();
        }

        public void SetData(ref NpcData npc)
        {
            barInstance.SetData(ref npc);
        }

        void LateUpdate()
        {
            transform.LookAt(cameraToLookAt.transform);
            transform.rotation = Quaternion.LookRotation(-cameraToLookAt.transform.forward);
        }
    }
}