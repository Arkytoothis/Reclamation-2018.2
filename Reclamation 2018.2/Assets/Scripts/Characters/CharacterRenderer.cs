using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Reclamation.Equipment;
using Reclamation.Misc;

namespace Reclamation.Characters
{
    public class CharacterRenderer : MonoBehaviour
    {
        public Transform pivot;

        public List<Transform> mounts;
        public List<SkinnedMeshRenderer> meshes;

        public int defaultMesh = 0;

        private PcData pcData;

        void Start()
        {
            //for (int i = 0; i < meshes.Count; i++)
            //{
            //    meshes[i].enabled = false;
            //}

            //meshes[defaultMesh].enabled = true;
        }

        public void LoadEquipment(PcData pcData)
        {
            if (pcData == null) return;
            this.pcData = pcData;

            ItemData item = this.pcData.Inventory.EquippedItems[(int)EquipmentSlot.Right_Hand];

            if (item != null)
            {
                GameObject go = Instantiate( ModelManager.instance.GetItemPrefab(item.Key));

                if (go == null)
                {
                    Debug.Log("Loading " + item.Name + " to Right Hand failed");
                }
                else
                {
                    ItemDefinition def = Database.GetItem(item.Key, false);
                    //Debug.Log("Loading " + item.Name + " to Right Hand success");
                    go.transform.SetParent(mounts[(int)EquipmentRenderSlot.Right_Hand], false);
                    go.transform.localPosition += def.offset;
                    go.transform.localRotation = new Quaternion(def.rotation.x, def.rotation.y, def.rotation.z, 0f);
                }
            }
        }

        public void SetHair(GameObject hair)
        {
            GameObject go = Instantiate(hair, mounts[(int)EquipmentRenderSlot.Hair]);
        }

        public void SetBeard(GameObject beard)
        {
            GameObject go = Instantiate(beard, mounts[(int)EquipmentRenderSlot.Beard]);
        }
    }
}