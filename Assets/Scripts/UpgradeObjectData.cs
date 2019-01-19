using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Upgrade", menuName = "Game/Upgrade", order = 1)]
public class UpgradeObjectData : ScriptableObject
{
    public string upgradeName;
    
    public string upgradeFunction = "Assign a function please!";

    public Texture2D sampleImage;

    public int gemCost;
    public int moneyCost;

    public int copies = 1;

    [TextArea]
    public string description;
}

[System.Serializable]
public class Upgrade
{
    public UpgradeObjectData upgradeObjectData;
    public int copiesLeft;

    public Upgrade(UpgradeObjectData upgradeObjectData) {
        this.upgradeObjectData = upgradeObjectData;
        this.copiesLeft = upgradeObjectData.copies;
    }
}