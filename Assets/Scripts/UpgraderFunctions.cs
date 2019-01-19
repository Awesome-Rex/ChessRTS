using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgraderFunctions : MonoBehaviour
{
    //gem upgrades
    public void moreOrbs () {
        Game.currentGame.addedTurnOrbs += 1;
        if (GameplayControl.gameplayControl.currentTurn == Side.Life) {
            GameplayControl.gameplayControl.turnOrbs += 1;
        }
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

    }

    public void addUnit () {

    }
    //
}
