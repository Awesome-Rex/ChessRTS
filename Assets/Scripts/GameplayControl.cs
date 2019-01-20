using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Side { Life, Nature, Death }

public class GameplayControl : MonoBehaviour
{
    public static GameplayControl gameplayControl;

    public int turnOrbs;
    
    public Side currentTurn;
    public int turns;

    public int turnsLife;
    public int turnsNature;
    public int turnsDeath;

    public GameObject targetPositionObject;
    public Vector3 targetPosition
    {
        get
        {
            return targetPositionObject.transform.position;
        }
        set
        {
            targetPositionObject.transform.position = value;
        }
    }

    public GameObject targetedObject;

    public enum VisualUnitAbility {Nothing, Movement, Damage}
    public VisualUnitAbility visualUnitAbility;

    public enum ModularVisualUnitsAbility {Nothing, AllyMovement, AllyDamage, EnemyMovement, EnemyDamage}
    public ModularVisualUnitsAbility modularVisualUnitsAbility;

    //temporary
    private UpgraderFunctions upgraderFunctions;
    //


    public static Vector3[] convert2DtoVector3 (bool[,] map) {

        return null;
    }
    public static Vector3[] convert2DtoVector3(int[,] map) {

        return null;
    }


    public void executeUpgrade(Upgrade upgrade) {
        if (upgrade.copiesLeft > 0) {
            upgraderFunctions.Invoke(upgrade.upgradeObjectData.upgradeFunction, 0f);
            upgrade.copiesLeft -= 1;
        }
    }

    private void Awake()
    {
        gameplayControl = this;
        upgraderFunctions = GetComponent<UpgraderFunctions>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
