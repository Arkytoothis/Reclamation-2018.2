using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        ScreenManager.instance.Initialize();
        PlayerManager.instance.Initialize();

        StartGame();
    }

    public void StartGame()
    {
        //Debug.Log("GameManager.StartGame");
    }
}