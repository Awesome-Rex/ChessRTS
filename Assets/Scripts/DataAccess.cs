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