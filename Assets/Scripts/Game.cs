using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SideData
{
    public SideSettings sideSettings;

    public int addedOrbs;

    public int currentOrbs;
    public int turns;

    public SideData (SideSettings sideSettings)
    {
        this.sideSettings = sideSettings;

        addedOrbs = 0;

        currentOrbs = 0;
        turns = 0;
    }
}

[System.Serializable]
public class Game
{
    public static List<Game> games;
    public static Game currentGame;

    public List<SideData> sides;
    public int gems;

    //upgrades
    public int fogViewRange;

    public List<string> unlockedHeroes;
    //

    //upgrade shop
    public List<Upgrade> upgrades;


    //

    //temporary
    public int money;
    //

    public Game() {
        games.Add(this);


        foreach (Object sideSetting in Resources.LoadAll("ScriptableObjects/Sides"))
        {
            sides.Add(new SideData(sideSetting as SideSettings));
        }

        gems = 0;
        money = 0;

        fogViewRange = 2;
        unlockedHeroes = new List<string>();
    }

    public SideData findSide(Side targetSide)
    {
        foreach (SideData savedSide in sides)
        {
            if (savedSide.sideSettings.side == targetSide)
            {
                return savedSide;
            }
        }

        return null;
    }

    public static void Save() {

    }

    public static void Load() {

    }
}
