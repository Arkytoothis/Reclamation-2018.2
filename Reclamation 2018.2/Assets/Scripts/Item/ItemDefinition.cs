﻿using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ItemDefinition
{
    public string Name;
    public string Key;
    public string Description;

    public string LargeSpriteKey;
    public string SmallSpriteKey;

    public ItemNameFormat NameFormat;
    public ItemType Type;
    public EquipmentSlot Slot;
    public ItemHardnessAllowed Hardness;

    public int BaseActions;
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

        Type = ItemType.None;
        Slot = EquipmentSlot.None;
        Hardness = ItemHardnessAllowed.None;
        NameFormat = ItemNameFormat.None;

        BaseActions = 0;
        BasePower = 0;

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

    public ItemDefinition(string name, string key, string large_sprite_key, string small_sprite_key, EquipmentSlot slot, int durability, int power, int hours, int actions,
        ItemType type, ItemHardnessAllowed hardness, ItemNameFormat format,
        WeaponData weapon, AmmoData ammo, WearableData armor, AccessoryData accessory, UsableData usable)
    {
        Name = name;
        Key = key;
        Description = "empty";

        Type = type;
        NameFormat = format;
        LargeSpriteKey = large_sprite_key;
        SmallSpriteKey = small_sprite_key;
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

        BaseActions = actions;
        BasePower = power;
        BaseBuildTime = hours;
        ResourcesRequired = new List<ResourceData>();
        SkillRequirements = new List<SkillRequirement>();
        SetTooltipText();
    }

    public ItemDefinition(ItemDefinition item)
    {
        Name = item.Name;
        Key = item.Key;
        Description = item.Description;

        Type = item.Type;
        NameFormat = item.NameFormat;
        LargeSpriteKey = item.LargeSpriteKey;
        SmallSpriteKey = item.SmallSpriteKey;

        BaseBuildTime = item.BaseBuildTime;
        BaseActions = item.BaseActions;
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

            if (WeaponData.Attributes[(int)WeaponAttributes.Actions].Value != 1)
                details += "\n" + WeaponData.Attributes[(int)WeaponAttributes.Actions].Value + " Actions";
            else
                details += "\n" + WeaponData.Attributes[(int)WeaponAttributes.Actions].Value + " Action";

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

            if (AmmoData.Attributes[(int)AmmoAttributes.Actions].Value != 1)
                details += "\n" + AmmoData.Attributes[(int)AmmoAttributes.Actions].Value + " Actions";
            else
                details += "\n" + AmmoData.Attributes[(int)AmmoAttributes.Actions].Value + " Action";


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