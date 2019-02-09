using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Upgrade", menuName = "Game/Upgrade", order = 1)]
public class UpgradeObjectData : ScriptableObject
{
    public string upgradeName;
    
    public string upgradeFunction = "Assign a function please!";
    public UnityEvent otherFunction;

    public Texture2D sampleImage;

    public int gemCost;
    public int moneyCost;

    public bool limitedCopies = true;
    public int copies = 1;

    public bool availableAtFirst = true;

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