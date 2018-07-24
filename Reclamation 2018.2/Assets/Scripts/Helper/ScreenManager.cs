using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ScreenType
{
    Armory, Barracks, Library, Stronghold, Codex, Menu
}

public class ScreenManager : Singleton <ScreenManager>
{
    public List<GameScreen> Screens;

    public void Initialize()
    {
        if (Screens == null)
        {
            return;
        }

        for (int i = 0; i < Screens.Count; i++)
        {
            Screens[i].Initialize();
            Screens[i].Close();
        }
    }

    public void OpenScreen(int index)
    {
        for (int i = 0; i < Screens.Count; i++)
        {
            if (i == index)
                Screens[i].Toggle();
            else
                Screens[i].Close();
        }
    }

    public void CloseScreens()
    {
        for (int i = 0; i < Screens.Count; i++)
            Screens[i].Close();
    }
}