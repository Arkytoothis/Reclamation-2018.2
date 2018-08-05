using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reclamation.Audio;
using Reclamation.Characters;
using Reclamation.Gui;
using Reclamation.Equipment;
using Reclamation.Misc;

namespace Reclamation.World
{
    public class WorldManager : Singleton<WorldManager>
    {
        [SerializeField] Camera mainCamera;

        void Awake()
        {
            Invoke(nameof(Initialize), 0.1f);
            mainCamera = Camera.main;
        }

        public void Initialize()
        {
            //Debug.Log("GameManager.Initialize");

            Database.Initialize();
            ItemGenerator.Initialize();
            PcGenerator.Initialize();
            NpcGenerator.Initialize();

            SpriteManager.instance.Initialize();
            AudioManager.instance.Initialize();
            ModelManager.instance.Initialize();
            ParticleManager.instance.Initialize();
            MessageSystem.instance.Initialize();
            MapManager.instance.Initialize();
            ScreenManager.instance.Initialize();
            PlayerManager.instance.Initialize();

            StartGame();
        }

        public void StartGame()
        {
            //Debug.Log("GameManager.StartGame");
            mainCamera.transform.SetParent(MapManager.instance.Stronghold.transform);
            CameraController cam = mainCamera.GetComponent<CameraController>();
            cam.target = MapManager.instance.Stronghold.transform.transform;

            AudioManager.instance.WorldPlaylist.StartPlaying(0);
        }
    }
}