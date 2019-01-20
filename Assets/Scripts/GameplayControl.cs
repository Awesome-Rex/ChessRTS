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


    public static List<Vector3> convert2DtoVector3 (bool[,] map) {
        List<Vector3> spots = new List<Vector3>();

        for (int x = 0; x < map.GetLength(0); x++) {
            for (int y = 0; y < map.GetLength(1); y++) {
                if (map[x, y] == true) {
                    spots.Add(new Vector3((x - Mathf.Ceil(map.GetLength(0) / 2)), -(y - Mathf.Ceil(map.GetLength(1) / 2)), 0));
                }
            }
        }

        return spots;
    } public static List<Vector3> convert2DtoVector3(int[,] map) {
        List<Vector3> spots = new List<Vector3>();

        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                if (map[x, y] > 0)
                {
                    spots.Add(new Vector3((x - Mathf.Ceil(map.GetLength(0) / 2)), -(y - Mathf.Ceil(map.GetLength(1) / 2)), 0));
                }
            }
        }

        return spots;
    }

    public static bool[,] listTo2DArray (List<Vector3> list, Vector2 dimensions)
    {
        bool[,] array = new bool[(int)dimensions.x, (int)dimensions.y];

        for (int i = 0; i < list.Count; i++) {
            array[Mathf.CeilToInt(dimensions.x / 2) + ((int)(list[i].x)), Mathf.CeilToInt(dimensions.y / 2) + -((int)(list[i].y))] = true;
        }

        return array;
    }
    public static int[,] listTo2DArray(List<Vector3> list, List<int> intList, Vector2 dimensions) {
        int[,] array = new int[(int)dimensions.x, (int)dimensions.y];

        for (int i = 0; i < ((list.Count + intList.Count) / 2); i++)
        {
            array[Mathf.CeilToInt(dimensions.x / 2) + ((int)(list[i].x)), Mathf.CeilToInt(dimensions.y / 2) + -((int)(list[i].y))] = intList[i];
        }

        return array;
    }

    public static List<int> damageAreaToDamageList(int[,] map) {
        List<int> damageList = new List<int>();

        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                if (map[x, y] > 0)
                {
                    damageList.Add(map[x, y]);
                }
            }
        }

        return damageList;
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
