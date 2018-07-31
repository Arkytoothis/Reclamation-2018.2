using UnityEngine;
using System.Collections.Generic;

namespace Reclamation.Equipment
{
    [System.Serializable]
    public class ItemShort
    {
        public string ItemKey;
        public string MaterialKey;
        public string PlusKey;
        public string PreKey;
        public string PostKey;

        public int StackSize;

        public ItemShort()
        {
            ItemKey = "";
            MaterialKey = "";
            PlusKey = "";
            PreKey = "";
            PostKey = "";

            StackSize = 0;
        }

        public ItemShort(string item, string material, string plus, string pre, string post)
        {
            ItemKey = item;
            MaterialKey = material;
            PlusKey = plus;
            PreKey = pre;
            PostKey = post;

            StackSize = 0;
        }

        public ItemShort(ItemData item)
        {
            ItemKey = item.Key;
            StackSize = item.StackSize;

            if (item.Material != null)
                MaterialKey = item.Material.Key;

            if (item.Quality != null)
                PlusKey = item.Quality.Key;

            if (item.PreEnchant != null)
                PreKey = item.PreEnchant.Key;

            if (item.PostEnchant != null)
                PostKey = item.PostEnchant.Key;
        }

        public ItemShort(ItemShort item)
        {
            ItemKey = item.ItemKey;
            MaterialKey = item.MaterialKey;
            PlusKey = item.PlusKey;
            PreKey = item.PreKey;
            PostKey = item.PostKey;
        }
    }
}