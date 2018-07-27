using UnityEngine;
using System.Collections.Generic;

namespace Reclamation.World
{
    [System.Serializable]
    public class Stronghold
    {
        public string Name;
        public int MapX;
        public int MapY;

        public Stronghold()
        {
            Name = "Stronghold";
            MapX = 0;
            MapY = 0;
        }

        public Stronghold(string name, int x, int y)
        {
            Name = name;
            MapX = x;
            MapY = y;
        }

        public Stronghold(Stronghold stronghold)
        {
            Name = stronghold.Name;
            MapX = stronghold.MapX;
            MapY = stronghold.MapY;
        }
    }
}