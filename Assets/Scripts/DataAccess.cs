using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataAccess", menuName = "Game/DataAccess", order = 1)]
public class DataAccess : ScriptableObject
{
    public UpgraderFunctions upgraderFunctions;

    public List<UpgradeObjectData> upgrades;

    public List<SideSettings> sideSettings;
}

public class Tools
{
    public static int underZero(int number)
    {
        if (number < 0)
        {
            return 0;
        }
        return number;
    }
}