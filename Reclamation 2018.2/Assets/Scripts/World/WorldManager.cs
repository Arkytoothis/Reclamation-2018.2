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
        [SerializeField] new Camera camera;

        void Awake()
        {
            //Debug.Log("WorldManager.Awake");
            Invoke(nameof(Initialize), 0.1f);
            camera = Camera.main;
        }

        public void Initialize()
        {
            //Debug.Log("WorldManager.Initialize");
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
            MessageSystem.instance.AddMessage("Welcome to Reclamation");

            //Debug.Log("GameManager.StartGame");
            camera.transform.SetParent(PlayerManager.instance.Pcs[0].transform);
            CameraController cameraController = camera.GetComponent<CameraController>();
            cameraController.target = PlayerManager.instance.Pcs[0].transform;

            AudioManager.instance.WorldPlaylist.StartPlaying(0);
        }
    }
}