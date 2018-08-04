using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Linq;

namespace Reclamation.Misc
{
    public class SpriteManager : Singleton<SpriteManager>
    {
        bool initialized = false;

        private Dictionary<string, Sprite> guiIcons = new Dictionary<string, Sprite>();
        private Dictionary<string, Sprite> itemIcons = new Dictionary<string, Sprite>();
        private Dictionary<string, Sprite> raceIcons = new Dictionary<string, Sprite>();
        private Dictionary<string, Sprite> abilityIcons = new Dictionary<string, Sprite>();

        public TMP_FontAsset NormalFont;

        public Dictionary<string, Sprite> SmallItemSprites { get { return itemIcons; } }
        public Dictionary<string, Sprite> AbilitySprites { get { return abilityIcons; } }
        public Dictionary<string, Sprite> RaceSprites { get { return raceIcons; } }

        private void Awake()
        {
            Reload();
        }

        public void Initialize()
        {
            if (initialized == false)
            {
                initialized = true;
                DontDestroyOnLoad(transform.gameObject);

                //NormalFont = Resources.Load<TMP_FontAsset>("Fonts/normal");

                //LoadSpritesFromFolder("Sprites/Gui Icons", guiIcons, false);

                Sprite[] sprites;

                //sprites = Resources.LoadAll<Sprite>("Sprites/items");

                //foreach (Sprite s in sprites)
                //{
                //    itemIcons[s.name] = s;
                //}

                //sprites = Resources.LoadAll<Sprite>("Sprites/races");

                //foreach (Sprite s in sprites)
                //{
                //    raceIcons[s.name] = s;
                //}

                sprites = Resources.LoadAll<Sprite>("Abilities");

                foreach (Sprite s in sprites)
                {
                    abilityIcons[s.name] = s;
                }
            }
        }

        void LoadSpritesFromFolder(string path, Dictionary<string, Sprite> dict, bool log)
        {
            Texture2D[] textures = (Texture2D[])Resources.LoadAll<Texture2D>(path);

            for (int i = 0; i < textures.Length; i++)
            {
                Sprite sprite = Sprite.Create(textures[i], new Rect(0f, 0f, textures[i].width, textures[i].height), new Vector2(0f, 0f), 32f);
                sprite.texture.filterMode = FilterMode.Point;

                dict.Add(textures[i].name, sprite);

                if (log == true)
                {
                    Debug.Log(textures[i].name);
                }
            }
        }

        public Sprite GetItemSprite(string key)
        {
            if (itemIcons.ContainsKey(key) == false)
            {
                Debug.LogWarning("itemSprites key: " + key + " does not exist");
                return itemIcons["blank"];
            }
            else
            {
                return itemIcons[key];
            }
        }

        public Sprite GetRaceSprite(string key)
        {
            if (raceIcons.ContainsKey(key) == false)
            {
                Debug.LogWarning("raceSprites key: " + key + " does not exist");
                return raceIcons["blank"];
            }
            else
            {
                return raceIcons[key];
            }
        }

        public Sprite GetAbilitySprite(string key)
        {
            if (abilityIcons.ContainsKey(key) == false)
            {
                Debug.LogWarning("abilitySprites key: " + key + " does not exist");
                return abilityIcons["blank"];
            }
            else
            {
                return abilityIcons[key];
            }
        }

        public Sprite GetIconSprite(string key)
        {
            if (guiIcons.ContainsKey(key) == false)
            {
                Debug.LogWarning("guiIcons key: " + key + " does not exist");
                return raceIcons["blank"];
            }
            else
            {
                return guiIcons[key];
            }
        }
    }
}