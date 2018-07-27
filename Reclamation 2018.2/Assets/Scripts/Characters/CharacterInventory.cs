using UnityEngine;
using System.Collections.Generic;
using Reclamation.Equipment;
using Reclamation.Misc;

namespace Reclamation.Characters
{
    [System.Serializable]
    public class CharacterInventory
    {
        public Item[] EquippedItems;
        public List<Item> Accessories;
        public static int MaximumAccessories = 14;
        public int AccessoryLimit = 4;

        public Item GetItem(int slot)
        {
            return EquippedItems[slot];
        }

        public CharacterInventory()
        {
            EquippedItems = new Item[(int)EquipmentSlot.Number];
            Accessories = new List<Item>(MaximumAccessories);

            for (int i = 0; i < EquippedItems.Length; i++)
            {
                EquippedItems[i] = null;
            }

            for (int i = 0; i < MaximumAccessories; i++)
            {
                Accessories.Add(null);
            }
        }

        public CharacterInventory(CharacterInventory inventory)
        {
            AccessoryLimit = inventory.AccessoryLimit;
            EquippedItems = new Item[(int)EquipmentSlot.Number];
            Accessories = new List<Item>(MaximumAccessories);

            for (int i = 0; i < inventory.EquippedItems.Length; i++)
            {
                if (inventory.EquippedItems[i] != null)
                    EquippedItems[i] = new Item(inventory.EquippedItems[i]);
            }

            for (int i = 0; i < inventory.Accessories.Count; i++)
            {
                if (inventory.Accessories[i] != null)
                    Accessories.Add(new Item(inventory.Accessories[i]));
            }
        }

        public void EquipItem(Item item, EquipmentSlot slot)
        {
            if (item != null)
                EquippedItems[(int)slot] = new Item(item);
            else
                EquippedItems[(int)slot] = null;

        }

        public void EquipAccessory(Item item, int slot)
        {
            if (slot != -1)
            {
                Accessories[slot] = new Item(item);
            }
            else
            {
                for (int i = 0; i < Accessories.Count; i++)
                {
                    if (Accessories[i] == null)
                    {
                        Accessories[i] = new Item(item);
                        break;
                    }
                }
            }
        }

        public bool TryEquip(Item item, EquipmentSlot slot)
        {
            bool success = true;

            if (item.Slot == slot)
            {
                success = true;
                EquipItem(item, slot);
            }

            return success;
        }
    }
}