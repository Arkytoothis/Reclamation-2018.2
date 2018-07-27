using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reclamation.Misc
{
    public static class GameSettings
    {
        public static float EffectsVolume = 0.5f;
        public static float MusicVolume = 0.5f;
        public static float GuiVolume = 0.5f;
        public static float AmbientVolume = 0.5f;

        public static bool LoadGame = false;
        public static string GameToLoad = "save_01";

        public static int WorldWidth = 64;
        public static int WorldHeight = 64;

        public static bool RandomizeWorldSeed = false;
        public static int WorldSeed = 0;

        public static int AttributeExpCost = 150;
        public static int SkillExpCost = 100;
    }
}