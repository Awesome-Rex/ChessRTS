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

    public bool playerControlled;

    public void startReset ()
    {
        List<Unit> sideUnits = new List<Unit>();
        foreach (SideDefine sideUnit in Object.FindObjectsOfType<SideDefine>())
        {
            if (sideUnit.GetComponent<Unit>() != null && sideUnit.GetComponent<SideDefine>().side == sideSettings.side)
            {
                sideUnits.Add(sideUnit.GetComponent<Unit>());
            }
        }

        currentOrbs = sideUnits.Count + addedOrbs;
        
        turns = 0;
    }

    public SideData (SideSettings sideSettings)
    {
        this.sideSettings = sideSettings;

        addedOrbs = 0;

        currentOrbs = 0;
        turns = 0;

        playerControlled = false;
    }
}

/*public class Global
{
    public List<Game> games;
    public Game currentGame;

    public Global ()
    {
        games = new List<Game>();
        
    }
}*/

[System.Serializable]
public class Game
{
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

    public Game(Object[] sideAssets, Side playerSide) {
        sides = new List<SideData>();
        foreach (Object sideSetting in sideAssets)
        {
            SideData definedSideData = new SideData(sideSetting as SideSettings);

            sides.Add(definedSideData);

            if ((sideSetting as SideSettings).side == playerSide) {
                definedSideData.playerControlled = true;
            }
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

    public void Save() {

    }

    public void Load() {

    }
}
