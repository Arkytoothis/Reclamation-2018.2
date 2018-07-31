using UnityEngine;
using System.Collections.Generic;
using Reclamation.Characters;
using Reclamation.Misc;
using Reclamation.World;

namespace Reclamation.Equipment
{
    [System.Serializable]
    public class ItemDefinition
    {
        public string Name;
        public string Key;
        public string Description;

        public string IconPath;
        public string MeshPath;
        public string TexturePath;

        public Vector3 offset;
        public Vector3 rotation;

        public ItemNameFormat NameFormat;
        public ItemType Type;
        public EquipmentSlot Slot;
        public ItemHardnessAllowed Hardness;

        public float RecoveryTime;
        public float ActionSpeed;

        public int BasePower;
        public int BaseBuildTime;
        public int DurabilityMax;

        public List<SkillRequirement> SkillRequirements;
        public List<ResourceData> ResourcesRequired;

        public WeaponData WeaponData;
        public AmmoData AmmoData;
        public WearableData WearableData;
        public AccessoryData AccessoryData;
        public UsableData UsableData;

        public string TooltipText;

        public ItemDefinition()
        {
            Name = "";
            Key = "";
            Description = "empty";
            IconPath = "";
            MeshPath = "";
            TexturePath = "";

            Type = ItemType.None;
            Slot = EquipmentSlot.None;
            Hardness = ItemHardnessAllowed.None;
            NameFormat = ItemNameFormat.None;

            RecoveryTime = 0f;
            ActionSpeed = 0f;

            BasePower = 0;
            BaseBuildTime = 0;
            DurabilityMax = 0;

            WeaponData = null;
            AmmoData = null;
            WearableData = null;
            AccessoryData = null;
            UsableData = null;

            BaseBuildTime = 0;
            SkillRequirements = new List<SkillRequirement>();
            ResourcesRequired = new List<ResourceData>();
            TooltipText = "";
        }

        public ItemDefinition(string name, string key, string iconKey, string meshKey, string textureKey, EquipmentSlot slot, int durability, int power, int hours,
            float recoveryTime, float actionSpeed, ItemType type, ItemHardnessAllowed hardness, ItemNameFormat format,
            WeaponData weapon, AmmoData ammo, WearableData armor, AccessoryData accessory, UsableData usable,
            Vector3 rotation, Vector3 offset)
        {
            Name = name;
            Key = key;
            Description = "empty";

            Type = type;
            NameFormat = format;
            IconPath = iconKey;
            MeshPath = meshKey;
            TexturePath = textureKey;

            RecoveryTime = recoveryTime;
            ActionSpeed = actionSpeed;

            BasePower = power;
            BaseBuildTime = hours;
            Slot = slot;
            DurabilityMax = durability;
            Hardness = hardness;

            if (weapon != null)
            {
                WeaponData = new WeaponData(weapon);
            }
            else
                WeaponData = null;

            if (ammo != null)
            {
                AmmoData = new AmmoData(ammo);
            }
            else
                AmmoData = null;

            if (armor != null)
            {
                WearableData = new WearableData(armor);
            }
            else
                WearableData = null;

            if (accessory != null)
            {
                AccessoryData = new AccessoryData(accessory);
            }
            else
                AccessoryData = null;

            if (usable != null)
                UsableData = new UsableData(usable);
            else
                UsableData = null;

            ResourcesRequired = new List<ResourceData>();
            SkillRequirements = new List<SkillRequirement>();
            SetTooltipText();

            this.rotation = new Vector3(rotation.x, rotation.y, rotation.z);
            this.offset = offset;
        }

        public ItemDefinition(ItemDefinition item)
        {
            Name = item.Name;
            Key = item.Key;
            Description = item.Description;

            Type = item.Type;
            NameFormat = item.NameFormat;
            IconPath = item.IconPath;
            MeshPath = item.MeshPath;
            TexturePath = item.TexturePath;

            RecoveryTime = item.RecoveryTime;
            ActionSpeed = item.ActionSpeed;

            BaseBuildTime = item.BaseBuildTime;
            BasePower = item.BasePower;
            Slot = item.Slot;
            DurabilityMax = item.DurabilityMax;

            Hardness = item.Hardness;

            if (item.WeaponData != null)
            {
                WeaponData = new WeaponData(item.WeaponData);
            }
            else
                WeaponData = null;

            if (item.AmmoData != null)
            {
                AmmoData = new AmmoData(item.AmmoData);
            }
            else
                AmmoData = null;

            if (item.WearableData != null)
            {
                WearableData = new WearableData(item.WearableData);
            }
            else
                WearableData = null;

            if (item.AccessoryData != null)
            {
                AccessoryData = new AccessoryData(item.AccessoryData);
            }
            else
                AccessoryData = null;

            if (item.UsableData != null)
                UsableData = new UsableData(item.UsableData);
            else
                UsableData = null;

            ResourcesRequired = new List<ResourceData>();
            for (int i = 0; i < item.ResourcesRequired.Count; i++)
            {
                ResourcesRequired.Add(new ResourceData(item.ResourcesRequired[i]));
            }

            SkillRequirements = new List<SkillRequirement>();
            for (int i = 0; i < item.SkillRequirements.Count; i++)
            {
                SkillRequirements.Add(new SkillRequirement(item.SkillRequirements[i]));
            }

            SetTooltipText();

            rotation = item.rotation;
            offset = item.offset;
        }

        public string GetResourcesText()
        {
            string text = "";

            for (int i = 0; i < ResourcesRequired.Count; i++)
            {
                text += "\n" + ResourcesRequired[i].Amount + " " + ResourcesRequired[i].Resource;
            }

            return text;
        }

        public void SetTooltipText()
        {
            string details = "";

            details += "\nDurability " + DurabilityMax;

            if (SkillRequirements.Count > 0)
            {
                details += "\nSkill Requirements";

                for (int i = 0; i < SkillRequirements.Count; i++)
                {
                    details += "\n<pos=10%>" + Database.Skills[SkillRequirements[i].DefinitionIndex].Name + " " + SkillRequirements[i].Value;
                }
            }

            details += "\n" + Type;

            if (WeaponData != null)
            {
                details += " - " + WeaponData.Type;
                details += "\n" + WeaponData.Attributes[(int)WeaponAttributes.Attack].Value + " " + WeaponData.AttackType + " Attack";

                if (WeaponData.Attributes[(int)WeaponAttributes.Action_Speed].Value != 1)
                    details += "\n" + WeaponData.Attributes[(int)WeaponAttributes.Action_Speed].Value + " Actions";
                else
                    details += "\n" + WeaponData.Attributes[(int)WeaponAttributes.Action_Speed].Value + " Action";

                details += "<pos=50%>" + WeaponData.Attributes[(int)WeaponAttributes.Range].Value + " tile Range";

                if (WeaponData.Damage != null)
                {
                    for (int i = 0; i < WeaponData.Damage.Count; i++)
                    {
                        details += "\n" + WeaponData.Damage[i].ToString();
                    }
                }

                details += "\n" + WeaponData.Attributes[(int)WeaponAttributes.Parry].Value + "% Parry";
            }

            if (AmmoData != null)
            {
                details += " - " + AmmoData.Type;
                details += "\n" + AmmoData.Attributes[(int)AmmoAttributes.Attack].Value + " Attack";
                details += "<pos=50%>" + AmmoData.Attributes[(int)AmmoAttributes.Range].Value + " tile Range";

                if (AmmoData.Attributes[(int)AmmoAttributes.Action_Speed].Value != 1)
                    details += "\n" + AmmoData.Attributes[(int)AmmoAttributes.Action_Speed].Value + " Actions";
                else
                    details += "\n" + AmmoData.Attributes[(int)AmmoAttributes.Action_Speed].Value + " Action";


                if (AmmoData.Damage != null)
                {
                    for (int i = 0; i < AmmoData.Damage.Count; i++)
                    {
                        details += "\n" + AmmoData.Damage[i].ToString();
                    }
                }
            }

            if (WearableData != null)
            {
                details += " - " + WearableData.Type;
                details += "\n" + WearableData.Attributes[(int)WearableAttributes.Dodge].Value + "% Dodge";
                details += "<pos=50%>" + WearableData.Attributes[(int)WearableAttributes.Block].Value + "% Block";
                details += "\n" + WearableData.Attributes[(int)WearableAttributes.Armor].Value + " Armor";
                details += "\n";

                if (WearableData.Resistances != null)
                {
                    for (int i = 0; i < WearableData.Resistances.Count; i++)
                    {
                        details += WearableData.Resistances[i].Value + "% " + WearableData.Resistances[i].ToString() + " ";
                    }
                }
            }

            if (AccessoryData != null)
            {
                details += " - " + AccessoryData.Type;
            }

            details += "\n\nPower: " + BasePower;

            TooltipText = details;
        }
    }
}