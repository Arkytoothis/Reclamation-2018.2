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

            Vector3 raceScale = Database.GetRace(pcData.raceKey).scale;

            LoadItem((int)EquipmentSlot.Right_Hand, (int)EquipmentRenderSlot.Right_Hand);
            LoadItem((int)EquipmentSlot.Left_Hand, (int)EquipmentRenderSlot.Left_Hand);

            if (pcData.hair != "")
                SetHair(ModelManager.instance.GetHairModel(pcData.hair));
            if (pcData.beard != "")
                SetBeard(ModelManager.instance.GetBeardModel(pcData.beard));
        }

        public void LoadItem(int equipmentSlot, int renderSlot)
        {
            ItemData item = this.pcData.inventory.EquippedItems[equipmentSlot];

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

                    //Debug.Log(gameObject.transform.localScale);
                    go.transform.SetParent(mounts[renderSlot], true);
                    go.transform.localPosition = def.offset;
                    go.transform.localEulerAngles = def.rotation;
                    go.transform.localScale = Vector3.one;// new Vector3(2- gameObject.transform.localScale.x, 2- gameObject.transform.localScale.y, 2- gameObject.transform.localScale.z);

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