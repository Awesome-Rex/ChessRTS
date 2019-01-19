using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Game
{
    public List<Game> games;
    public static Game currentGame;

    public int gems;


    //upgrades
    public int addedTurnOrbs;
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

    }

    public static void Save() {

    }

    public static void Load() {

    }
}
