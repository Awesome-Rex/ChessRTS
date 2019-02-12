using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class SideData
{
    public SideSettings sideSettings;

    public int addedOrbs;

    public int currentOrbs;
    public int money;
    public int turns;

    public List<AbilitySpot> currentModularDamage;
    public List<AbilitySpot> currentModularMovement;


    public bool playerControlled;

    public List<AbilitySpot> getModularDamage ()
    {
        List<AbilitySpot> combinedSpots = new List<AbilitySpot>();

        foreach (Unit unit in Object.FindObjectsOfType<Unit>())
        {
            if (unit.GetComponent<SideDefine>() != null && unit.GetComponent<SideDefine>().side == sideSettings.side)
            {
                combinedSpots.AddRange(unit.damageAreaListed);
            }
        }

        List<AbilitySpot> collapsedCombinedSpots = new List<AbilitySpot>();

        combinedSpots.GroupBy(spot => spot.source.transform.position + spot.location).ToList().ForEach(group => 
        {
            List<int> damageValuesList = new List<int>();
            foreach (AbilitySpot spot in group)
            {
                damageValuesList.Add(spot.damageValues[0]);
                //group.ToList().Remove(spot)
            }

            collapsedCombinedSpots.Add(new AbilitySpot(null, group.Key, damageValuesList));
        });

        return collapsedCombinedSpots;
    }

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

        money = 0;
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

[System.Serializable]
public class Settings
{
    //Gameplay
    public bool automaticTurnSwitching;
    public bool speedUpEnemyTurns;
    public float turnSpeed;

    //audio
    public float musicVolume;
    public float soundEffectsVolume;

    //visual
    public bool showHealthBars;
    public bool showHealthValue;

    public Settings ()
    {

    }

    public void setToDefault ()
    {

    }
}

[System.Serializable]
public class Game
{
    public static Game currentGame;

    public Settings settings;

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
    
    //

    public Game(Object[] sideAssets, Object[] upgradeFiles, Side playerSide) {
        settings = new Settings();

        sides = new List<SideData>();
        foreach (Object sideSetting in sideAssets)
        {
            SideData definedSideData = new SideData(sideSetting as SideSettings);

            sides.Add(definedSideData);

            if ((sideSetting as SideSettings).side == playerSide) {
                definedSideData.playerControlled = true;
            }
        }

        upgrades = new List<Upgrade>();
        foreach (Object upgradeFile in upgradeFiles)
        {
            if ((upgradeFile as UpgradeObjectData).availableAtFirst)
            {
                upgrades.Add(new Upgrade(upgradeFile as UpgradeObjectData));
            }
        }

        gems = 0;

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
