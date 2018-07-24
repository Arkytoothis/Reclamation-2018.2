using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : Singleton<MainMenuManager>
{
    void Awake()
    {
        Database.Initialize();
    }

    void Start()
    {
    }

    public void ContinueGame()
    {
    }

    public void NewGame()
    {
    }

    public void Options()
    {
    }

    public void Exit()
    {
    }
}