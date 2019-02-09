using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgraderFunctions", menuName = "Game/UpgraderFunctions", order = 1)]
public class UpgraderFunctions : ScriptableObject
{
    //gem upgrades
    public void moreOrbs () {
        Game.currentGame.findSide(GameplayControl.gameplayControl.currentTurn).addedOrbs += 1;
        //if (GameplayControl.gameplayControl.currentTurn == Side.Life) {
            Game.currentGame.findSide(Side.Life).currentOrbs += 1;
        //}
    }

    public void moreUnits() {
        List<UpgradeObjectData> availableUpgrades = new List<UpgradeObjectData>();

        foreach (Object upgradeFile in Resources.LoadAll("ScriptableObjects/Upgrades/AddedUnitUpgrades")) {
            bool available = true;

            foreach (Upgrade boughtUpgrade in Game.currentGame.upgrades) {
                if (((UpgradeObjectData)upgradeFile) == boughtUpgrade.upgradeObjectData) {
                    available = false;
                }
            }

            if (available) {
                availableUpgrades.Add((UpgradeObjectData)upgradeFile);
            }
        }

        Game.currentGame.upgrades.Add(new Upgrade(availableUpgrades[Random.Range(0, availableUpgrades.Count)]));

        //reload item shop
    }

    public void moreViewRange() {
        Game.currentGame.fogViewRange += 1;
    }
    //

    //money upgrades
    public void addHealth () {
        if (SelectionManagement.selectionManagement.targetedObject.GetComponent<Health>() != null)
        {
            SelectionManagement.selectionManagement.targetedObject.GetComponent<Health>().health += SelectionManagement.selectionManagement.targetedObject.GetComponent<Health>().maxHealth / 2;
        } else
        {
            //alert no health
        }
    }

    public void addUnit (Unit addedUnit) {

    }
    //
}
