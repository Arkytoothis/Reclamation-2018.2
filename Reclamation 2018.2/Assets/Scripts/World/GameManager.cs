using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reclamation.Characters;
using Reclamation.Gui;
using Reclamation.Equipment;
using Reclamation.Misc;

namespace Reclamation.World
{
    public class GameManager : Singleton<GameManager>
    {
        void Awake()
        {
            Invoke("Initialize", 0.1f);
        }

        public void Initialize()
        {
            //Debug.Log("GameManager.Initialize");

            Database.Initialize();
            ItemGenerator.Initialize();
            PcGenerator.Initialize();

            WorldManager.instance.Initialize();
            ScreenManager.instance.Initialize();
            PlayerManager.instance.Initialize();

            StartGame();
        }

        public void StartGame()
        {
            //Debug.Log("GameManager.StartGame");
        }
    }
}