using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Reclamation.Equipment;
using Reclamation.Misc;
using UnityEngine.Animations;

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

            LoadItem((int)EquipmentSlot.Right_Hand, (int)EquipmentRenderSlot.Right_Hand);
            LoadItem((int)EquipmentSlot.Left_Hand, (int)EquipmentRenderSlot.Left_Hand);

            if (pcData.Hair != "")
                SetHair(ModelManager.instance.GetHairModel(pcData.Hair));
            if (pcData.Beard != "")
                SetBeard(ModelManager.instance.GetBeardModel(pcData.Beard));
        }

        public void LoadItem(int equipmentSlot, int renderSlot)
        {
            ItemData item = this.pcData.Inventory.EquippedItems[equipmentSlot];

            if (item != null)
            {
                GameObject go = Instantiate(ModelManager.instance.GetItemPrefab(item.Key));

                if (go == null)
                {
                    Debug.Log("Loading " + item.Name + " to Right Hand failed");
                }
                else
                {
                    ItemDefinition def = Database.GetItem(item.Key, false);
                    go.transform.SetParent(mounts[renderSlot], true);
                    go.transform.localPosition = def.offset;
                    go.transform.localEulerAngles = def.rotation;
                    go.transform.localScale = Vector3.one;

                }
            }
        }

        public void SetHair(GameObject hair)
        {
            GameObject go = Instantiate(hair);

            ModelAdjustment adjustment = go.GetComponent<ModelAdjustment>();
            if (adjustment != null)
            {
                go.transform.localPosition = adjustment.position;
                go.transform.localEulerAngles = adjustment.rotation;
                go.transform.localScale = adjustment.scale;
            }

            go.transform.SetParent(mounts[(int)EquipmentRenderSlot.Hair], false);

            go.name = hair.name;
        }

        public void SetBeard(GameObject beard)
        {
            GameObject go = Instantiate(beard);

            ModelAdjustment adjustment = go.GetComponent<ModelAdjustment>();
            if (adjustment != null)
            {
                go.transform.localPosition = adjustment.position;
                go.transform.localEulerAngles = adjustment.rotation;
                go.transform.localScale = adjustment.scale;
            }

            go.transform.SetParent(mounts[(int)EquipmentRenderSlot.Beard], false);

            go.name = beard.name;
        }
    }
}