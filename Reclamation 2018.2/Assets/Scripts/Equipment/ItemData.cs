using UnityEngine;
using System.Collections.Generic;
using Reclamation.Characters;
using Reclamation.Misc;
using Reclamation.World;

namespace Reclamation.Equipment
{
    [System.Serializable]
    public class ItemData
    {
        public string Name;
        public string Key;
        public string IconPath;
        public string MeshPath;
        public string TexturePath;

        public ItemNameFormat NameFormat;
        public ItemType Type;
        public EquipmentSlot Slot;
        public ItemHardnessAllowed Hardness;
        public List<SkillRequirement> SkillRequirements;

        public int StackSize;
        public float RecoveryTime;
        public float ActionSpeed;
        public int Power;
        public int DurabilityCur;
        public int DurabilityMax;
        public Rarity Rarity;

        public WeaponData WeaponData;
        public AmmoData AmmoData;
        public WearableData WearableData;
        public AccessoryData AccessoryData;
        public ItemModifier Material;
        public ItemModifier Quality;
        public ItemModifier PreEnchant;
        public ItemModifier PostEnchant;

        public ArtifactData ArtifactData;
        public ItemSetData SetData;

        public UsableData UsableData;

        public int HoursToBuild;
        public List<ResourceData> ResourcesRequired;

        public string TooltipText;

        public ItemData()
        {
            Name = "";
            Key = "";
            Type = ItemType.None;
            Slot = EquipmentSlot.None;
            Hardness = ItemHardnessAllowed.None;
            NameFormat = ItemNameFormat.None;

            StackSize = 0;
            Power = 0;
            RecoveryTime = 0;
            ActionSpeed = 0;
            HoursToBuild = 0;

            WeaponData = null;
            AmmoData = null;
            WearableData = null;
            AccessoryData = null;
            Material = null;
            Quality = null;
            PreEnchant = null;
            PostEnchant = null;

            ArtifactData = null;
            SetData = null;

            UsableData = null;

            SkillRequirements = new List<SkillRequirement>();
            ResourcesRequired = new List<ResourceData>();
            TooltipText = "";
        }

        public ItemData(ItemData item)
        {
            Name = item.Name;
            Key = item.Key;
            Type = item.Type;
            NameFormat = item.NameFormat;

            IconPath = item.IconPath;
            MeshPath = item.MeshPath;
            TexturePath = item.TexturePath;

            Slot = item.Slot;
            DurabilityCur = item.DurabilityCur;
            DurabilityMax = item.DurabilityMax;
            StackSize = item.StackSize;

            Hardness = item.Hardness;
            Power = item.Power;
            RecoveryTime = item.RecoveryTime;
            ActionSpeed = item.ActionSpeed;
            HoursToBuild = item.HoursToBuild;
            Rarity = item.Rarity;
            TooltipText = item.TooltipText;

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

            if (item.Material != null)
            {
                Material = new ItemModifier(item.Material);
            }
            else
                Material = null;

            if (item.Quality != null)
            {
                Quality = new ItemModifier(item.Quality);
            }
            else
                Quality = null;

            if (item.PreEnchant != null)
            {
                PreEnchant = new ItemModifier(item.PreEnchant);
            }
            else
                PreEnchant = null;

            if (item.PostEnchant != null)
            {
                PostEnchant = new ItemModifier(item.PostEnchant);
            }
            else
                PostEnchant = null;

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

            if (item.ArtifactData != null)
                ArtifactData = new ArtifactData(item.ArtifactData);
            else
                ArtifactData = null;

            if (item.SetData != null)
                SetData = new ItemSetData(item.SetData);
            else
                SetData = null;

            if (item.UsableData != null)
                UsableData = new UsableData(item.UsableData);
            else
                UsableData = null;
        }

        public ItemData(ItemDefinition def, int stack_size)
        {
            Name = def.Name;
            Key = def.Key;
            Type = def.Type;
            NameFormat = def.NameFormat;

            IconPath = def.IconPath;
            MeshPath = def.MeshPath;
            TexturePath = def.TexturePath;

            StackSize = stack_size;
            Slot = def.Slot;
            DurabilityCur = def.DurabilityMax;
            DurabilityMax = def.DurabilityMax;
            Hardness = def.Hardness;

            if (def.WeaponData != null && def.WeaponData.Type != WeaponType.None)
            {
                WeaponData = new WeaponData(def.WeaponData);
                RecoveryTime = WeaponData.Attributes[(int)WeaponAttributes.Action_Speed].Value;
            }
            else
                WeaponData = null;

            if (def.AmmoData != null && def.AmmoData.Type != AmmoType.None)
            {
                AmmoData = new AmmoData(def.AmmoData);
                RecoveryTime += AmmoData.Attributes[(int)AmmoAttributes.Action_Speed].Value;
            }
            else
                AmmoData = null;

            if (def.WearableData != null && def.WearableData.Type != WearableType.None)
            {
                WearableData = new WearableData(def.WearableData);
                RecoveryTime += WearableData.Attributes[(int)WearableAttributes.Action_Speed].Value;
            }
            else
                WearableData = null;

            if (def.AccessoryData != null && def.AccessoryData.Type != AccessoryType.None)
            {
                AccessoryData = new AccessoryData(def.AccessoryData);
                RecoveryTime += AccessoryData.Attributes[(int)AccessoryAttributes.Action_Speed].Value;
            }
            else
                AccessoryData = null;

            if (UsableData != null)
            {
                UsableData = new UsableData(def.UsableData);
            }
            else
            {
                UsableData = null;
            }

            ResourcesRequired = new List<ResourceData>();
            for (int i = 0; i < def.ResourcesRequired.Count; i++)
            {
                ResourcesRequired.Add(new ResourceData(def.ResourcesRequired[i]));
            }

            SkillRequirements = new List<SkillRequirement>();
            for (int i = 0; i < def.SkillRequirements.Count; i++)
            {
                SkillRequirements.Add(new SkillRequirement(def.SkillRequirements[i]));
            }

            RecoveryTime = def.RecoveryTime;
            ActionSpeed = def.ActionSpeed;
            HoursToBuild = def.BaseBuildTime;
            Power = def.BasePower;
        }

        public void SetMaterial(ItemModifier material)
        {
            if (material == null)
                return;

            Material = new ItemModifier(material);
        }

        public void SetName()
        {
            if (NameFormat != ItemNameFormat.Artifact)
            {
                Name = "";

                if (Quality != null)
                {
                    Name = Quality.Name + " ";
                }

                if (PreEnchant != null)
                {
                    Name += PreEnchant.Name + " ";
                }

                if (NameFormat == ItemNameFormat.Material_First)
                {
                    if (Material != null)
                        Name += Database.GetItemModifier(Material.Key).Name + " " + Database.GetItem(Key, false).Name;
                }
                else if (NameFormat == ItemNameFormat.Material_Middle)
                {
                    string[] strings = Database.GetItem(Key, false).Name.Split(' ');

                    if (strings.Length > 1)
                    {
                        Name += strings[0];
                        if (Material != null)
                            Name += " " + Database.GetItemModifier(Material.Key).Name;

                        Name = Database.GetItemModifier(Material.Key).Name + " " + Name;
                    }
                    else
                    {
                        Name = Database.GetItemModifier(Material.Key).Name + " " + Database.GetItem(Key, false).Name;
                    }
                }

                if (PostEnchant != null)
                {
                    Name += " " + PostEnchant.Name;
                }
            }
        }

        public void CalculatePower()
        {
            if (Database.Items.ContainsKey(Key) == true)
            {
                if (Material != null && Material.Key != "")
                    Power += Database.GetItemModifier(Material.Key).Power;

                if (Quality != null && Quality.Key != null && Quality.Key != "" && Database.ItemModifiers.ContainsKey(Quality.Key))
                {
                    Power += Database.GetItemModifier(Quality.Key).Power;
                }

                if (PreEnchant != null && PreEnchant.Key != null && PreEnchant.Key != "" && Database.ItemModifiers.ContainsKey(PreEnchant.Key))
                {
                    Power += Database.GetItemModifier(PreEnchant.Key).Power;
                }

                if (PostEnchant != null && PostEnchant.Key != null && PostEnchant.Key != "" && Database.ItemModifiers.ContainsKey(PostEnchant.Key))
                {
                    Power += Database.GetItemModifier(PostEnchant.Key).Power;
                }

                if (Power < 1)
                    Power = 1;
            }
        }

        public void CalculateHoursToBuild()
        {
            if (Material != null && Database.ItemModifiers.ContainsKey(Material.Key))
                HoursToBuild += Database.ItemModifiers[Material.Key].HoursToBuild;

            if (Quality != null && Quality.Key != null && Quality.Key != "" && Database.ItemModifiers.ContainsKey(Quality.Key))
                HoursToBuild += Database.ItemModifiers[Quality.Key].HoursToBuild;

            if (PreEnchant != null && PreEnchant.Key != null && PreEnchant.Key != "" && Database.ItemModifiers.ContainsKey(PreEnchant.Key))
                HoursToBuild += Database.ItemModifiers[PreEnchant.Key].HoursToBuild;

            if (PostEnchant != null && PostEnchant.Key != null && PostEnchant.Key != "" && Database.ItemModifiers.ContainsKey(PostEnchant.Key))
                HoursToBuild += Database.ItemModifiers[PostEnchant.Key].HoursToBuild;
        }

        public void CalculateAttributes()
        {
            CalculatePower();
            CalculateHoursToBuild();
            CalculateTotalResources();
            CalculateDurability();
            CalculateTotalResources();
            CalculateRarity();
        }

        public void CalculateTotalResources()
        {
            if (Database.Items.ContainsKey(Key) == false)
                return;

            if (Material != null)
                CalcResources(Material.ResourcesRequired);

            if (Quality != null)
                CalcResources(Quality.ResourcesRequired);

            if (PreEnchant != null)
                CalcResources(PreEnchant.ResourcesRequired);

            if (PostEnchant != null)
                CalcResources(PostEnchant.ResourcesRequired);
        }

        public void CalcResources(List<ResourceData> source_resources)
        {
            if (source_resources == null)
            {
                Debug.Log("source_resources == null");
                return;
            }

            for (int i = 0; i < source_resources.Count; i++)
            {
                bool found = false;
                for (int j = 0; j < ResourcesRequired.Count; j++)
                {
                    if (ResourcesRequired[j].Resource == source_resources[i].Resource)
                    {
                        found = true;
                        ResourcesRequired[j].Amount += source_resources[i].Amount;
                        break;
                    }
                }

                if (found == false)
                {
                    ResourcesRequired.Add(new ResourceData(source_resources[i]));
                }
            }
        }

        public void CalculateDurability()
        {
            if (Database.Items.ContainsKey(Key) == false)
                return;

            if (Material != null)
                DurabilityMax += Material.DurabilityModifier;

            if (Quality != null)
                DurabilityMax += Quality.DurabilityModifier;

            if (PreEnchant != null)
                DurabilityMax += PreEnchant.DurabilityModifier;

            if (PostEnchant != null)
                DurabilityMax += PostEnchant.DurabilityModifier;

            if (DurabilityMax < 1)
                DurabilityMax = 1;

            DurabilityCur = DurabilityMax;
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

        public void SetText()
        {
            SetName();

            string details = "";

            details += "\nDurability " + DurabilityCur + "/" + DurabilityMax;

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

            if (UsableData != null)
            {
                details += "Usable once every ";
                details += UsableData.Cooldown.Type;
            }

            if (AccessoryData != null)
            {
                details += " - " + AccessoryData.Type;
            }

            if (Quality != null)
            {
                details += "\n";
                details += Quality.GetText();
            }

            if (PreEnchant != null)
            {
                details += "\n";
                details += PreEnchant.GetText();
            }

            if (PostEnchant != null)
            {
                details += "\n";
                details += PostEnchant.GetText();
            }

            details += "\n\nPower: " + Power;

            if (ArtifactData != null)
            {
                details += ArtifactData.ToString();
            }

            if (SetData != null)
            {
                details += SetData.ToString();
            }

            TooltipText = details;
        }

        //public CharacterAction GetAction()
        //{
        //    CharacterAction action = new CharacterAction();

        //    if (WeaponData != null)
        //    {
        //        action.Type = ActionType.Weapon_Attack;
        //        action.DamageList = new List<DamageData>();

        //        for (int i = 0; i < WeaponData.Damage.Count; i++)
        //        {
        //            action.DamageList.Add(new DamageData(WeaponData.Damage[i]));
        //        }
        //    }

        //    return action;
        //}

        public void CalculateRarity()
        {
            if (ArtifactData == null)
            {
                Rarity = Rarity.Common;

                if (Material != null)
                {
                    Rarity = Material.Rarity;
                }

                if (Quality != null)
                {
                    if (Quality.Rarity > Rarity)
                        Rarity = Quality.Rarity;
                }

                if (PreEnchant != null)
                {
                    if (PreEnchant.Rarity > Rarity)
                        Rarity = PreEnchant.Rarity;
                }

                if (PostEnchant != null)
                {
                    if (PostEnchant.Rarity > Rarity)
                        Rarity = PostEnchant.Rarity;
                }

                if (SetData != null)
                {
                    Rarity = Rarity.Set;
                }
            }
            else Rarity = Rarity.Artifact;
        }
    }
}