using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum Side { Life, Nature, Death, Nothing }

public class GameplayControl : MonoBehaviour
{
    public Game currentGame;
    public static GameplayControl gameplayControl;


    public List<SideSettings> sideTurnOrder;
    
    public Side currentTurn;


    public enum VisualUnitAbility {Nothing, Movement, Damage}
    public VisualUnitAbility visualUnitAbility;

    public enum ModularVisualUnitsAbility {Nothing, AllyMovement, AllyDamage, EnemyMovement, EnemyDamage}
    public ModularVisualUnitsAbility modularVisualUnitsAbility;

    public VisualUnitAbility visualUnitAbilityMovement;

    public bool actionException;

    //temporary
    //private UpgraderFunctions upgraderFunctions;
    //

    
    public void nextTurn ()
    {
        int sideIndex = 0;

        for(; sideIndex < sideTurnOrder.Count; sideIndex++)
        {
            if (sideTurnOrder[sideIndex].side == currentTurn)
            {
                break;
            }
        }

        int newSideIndex = sideIndex + 1;
        if (sideIndex > sideTurnOrder.Count - 1)
        {
            newSideIndex = 0;
        }

        currentTurn = sideTurnOrder[newSideIndex].side;

        List<Unit> sideUnits = new List<Unit>();
        foreach (SideDefine sideUnit in Object.FindObjectsOfType<SideDefine>())
        {
            if (sideUnit.GetComponent<Unit>() != null && sideUnit.GetComponent<SideDefine>().side == currentTurn)
            {
                sideUnits.Add(sideUnit.GetComponent<Unit>());
            }
        }

        Game.currentGame.findSide(currentTurn).currentOrbs = sideUnits.Count + Game.currentGame.findSide(currentTurn).addedOrbs;
        Game.currentGame.findSide(currentTurn).turns += 1;

        if (!Game.currentGame.findSide(currentTurn).playerControlled)
        {
            GetComponent<AIManagement>().turn();
        }
    }


    //area to list
    public static List<Vector3> convert2DtoVector3 (bool[,] map) {
        List<Vector3> spots = new List<Vector3>();

        for (int x = 0; x < map.GetLength(0); x++) {
            for (int y = 0; y < map.GetLength(1); y++) {
                if (map[x, y] == true) {
                    if (map.GetLength(0) % 2 != 0 && map.GetLength(1) % 2 != 0) {
                        spots.Add(new Vector3((x - Mathf.Ceil(map.GetLength(0) / 2)), -(y - Mathf.Ceil(map.GetLength(1) / 2)), 0));
                    } else if (map.GetLength(0) % 2 == 0 || map.GetLength(1) % 2 == 0)
                    {
                        if (map.GetLength(0) % 2 == 0 && map.GetLength(1) % 2 == 0) {
                            spots.Add(new Vector3((x - (((map.GetLength(0) / 2) - 1) + 0.5f)), -(y - (((map.GetLength(1) / 2) - 1) + 0.5f)), 0));
                        } else if (map.GetLength(0) % 2 != 0 && map.GetLength(1) % 2 == 0)
                        {
                            spots.Add(new Vector3((x - Mathf.Ceil(map.GetLength(0) / 2)), -(y - (((map.GetLength(1) / 2) - 1) + 0.5f)), 0));
                        } else if (map.GetLength(0) % 2 == 0 && map.GetLength(1) % 2 != 0)
                        {
                            spots.Add(new Vector3((x - (((map.GetLength(0) / 2) - 1) + 0.5f)), -(y - Mathf.Ceil(map.GetLength(1) / 2)), 0));
                        }
                    }
                }
            }
        }

        return spots;
    }
    public static List<Vector3> convert2DtoVector3(int[,] map) {
        List<Vector3> spots = new List<Vector3>();

        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                if (map[x, y] > 0)
                {
                    if (map.GetLength(0) % 2 != 0 && map.GetLength(1) % 2 != 0)
                    {
                        spots.Add(new Vector3((x - Mathf.Ceil(map.GetLength(0) / 2)), -(y - Mathf.Ceil(map.GetLength(1) / 2)), 0));
                    }
                    else if (map.GetLength(0) % 2 == 0 || map.GetLength(1) % 2 == 0)
                    {
                        if (map.GetLength(0) % 2 == 0 && map.GetLength(1) % 2 == 0)
                        {
                            spots.Add(new Vector3((x - (((map.GetLength(0) / 2) - 1) + 0.5f)), -(y - (((map.GetLength(1) / 2) - 1) + 0.5f)), 0));
                        }
                        else if (map.GetLength(0) % 2 != 0 && map.GetLength(1) % 2 == 0)
                        {
                            spots.Add(new Vector3((x - Mathf.Ceil(map.GetLength(0) / 2)), -(y - (((map.GetLength(1) / 2) - 1) + 0.5f)), 0));
                        }
                        else if (map.GetLength(0) % 2 == 0 && map.GetLength(1) % 2 != 0)
                        {
                            spots.Add(new Vector3((x - (((map.GetLength(0) / 2) - 1) + 0.5f)), -(y - Mathf.Ceil(map.GetLength(1) / 2)), 0));
                        }
                    }
                }
            }
        }

        return spots;
    }
    public static List<int> damageAreaToDamageList(int[,] map)
    {
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
    //list to area
    public static bool[,] listTo2DArray (List<Vector3> list, Vector2 dimensions)
    {
        bool[,] array = new bool[(int)dimensions.x, (int)dimensions.y];

        for (int i = 0; i < list.Count; i++) {
            if (dimensions.x % 2 != 0 && dimensions.y % 2 != 0)
            {
                array[(Mathf.CeilToInt(dimensions.x / 2) - 1) + ((int)(list[i].x)), (Mathf.CeilToInt(dimensions.y / 2) - 1) + -((int)(list[i].y))] = true;
            }
            else if (dimensions.x % 2 == 0 || dimensions.y % 2 == 0)
            {
                if (dimensions.x % 2 == 0 && dimensions.y % 2 == 0) {
                    array[(int)((((dimensions.x / 2) - 1) + 0.5f) + (list[i].x)), (int)((((dimensions.x / 2) - 1) + 0.5f) + -(list[i].y))] = true;
                } else if (dimensions.x % 2 != 0 && dimensions.y % 2 == 0)
                {
                    array[(Mathf.CeilToInt(dimensions.x / 2) - 1) + ((int)(list[i].x)), (int)((((dimensions.x / 2) - 1) + 0.5f) + -(list[i].y))] = true;
                } else if (dimensions.x % 2 == 0 && dimensions.y % 2 != 0)
                {
                    array[(int)((((dimensions.x / 2) - 1) + 0.5f) + (list[i].x)), (Mathf.CeilToInt(dimensions.y / 2) - 1) + -((int)(list[i].y))] = true;
                }
            }
        }

        return array;
    }
    public static int[,] listTo2DArray(List<Vector3> list, List<int> intList, Vector2 dimensions) {
        int[,] array = new int[(int)dimensions.x, (int)dimensions.y];

        for (int i = 0; i < ((list.Count + intList.Count) / 2); i++)
        {
            //array[(Mathf.CeilToInt(dimensions.x / 2) - 1) + ((int)(list[i].x)), (Mathf.CeilToInt(dimensions.y / 2) - 1) + -((int)(list[i].y))] = intList[i];
            if (dimensions.x % 2 != 0 && dimensions.y % 2 != 0)
            {
                array[(Mathf.CeilToInt(dimensions.x / 2) - 1) + ((int)(list[i].x)), (Mathf.CeilToInt(dimensions.y / 2) - 1) + -((int)(list[i].y))] = intList[i];
            }
            else if (dimensions.x % 2 == 0 || dimensions.y % 2 == 0)
            {
                //array[(int)((((dimensions.x / 2) - 1) + 0.5f) + (list[i].x)), (int)((((dimensions.x / 2) - 1) + 0.5f) + -(list[i].y))] = intList[i];
                if (dimensions.x % 2 == 0 && dimensions.y % 2 == 0)
                {
                    array[(int)((((dimensions.x / 2) - 1) + 0.5f) + (list[i].x)), (int)((((dimensions.x / 2) - 1) + 0.5f) + -(list[i].y))] = intList[i];
                }
                else if (dimensions.x % 2 != 0 && dimensions.y % 2 == 0)
                {
                    array[(Mathf.CeilToInt(dimensions.x / 2) - 1) + ((int)(list[i].x)), (int)((((dimensions.x / 2) - 1) + 0.5f) + -(list[i].y))] = intList[i];
                }
                else if (dimensions.x % 2 == 0 && dimensions.y % 2 != 0)
                {
                    array[(int)((((dimensions.x / 2) - 1) + 0.5f) + (list[i].x)), (Mathf.CeilToInt(dimensions.y / 2) - 1) + -((int)(list[i].y))] = intList[i];
                }
            }
        }

        return array;
    }



    public static bool objectInSpot (Vector3 spot)
    {
        RaycastHit2D objectCast = Physics2D.Raycast(spot, Vector3.zero, 0f, ~LayerMask.NameToLayer("Object"));

        if (objectCast.collider != null) {
            return true;
        } else {
            return false;
        }
    }
    public static bool objectInSpot(Vector3 spot, GameObject ignoredObject)
    {
        RaycastHit2D objectCast = Physics2D.Raycast(spot, Vector3.zero, 0f, ~LayerMask.NameToLayer("Object"));

        if (objectCast.collider != null && objectCast.collider.gameObject != ignoredObject)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool objectInSpot (List<Vector3> matter, Vector3 spot)
    {
        foreach (Vector3 matterSpot in matter) {
            RaycastHit2D objectCast = Physics2D.Raycast(spot + matterSpot, Vector3.zero, 0f, ~LayerMask.NameToLayer("Object"));

            if (objectCast.collider != null)
            {
                return true;
            }
        }

        return false;
    }

    public static bool containedInArea (Vector3 location, List<Vector3> area, Vector3 areaLocation)
    {
        foreach (Vector3 areaSpot in area)
        {
            if (location == areaLocation + areaSpot)
            {
                return true;
            }
        }

        return false;
    }
    public static bool containedInArea (List<Vector3> matter, Vector3 location, List<Vector3> area, Vector3 areaLocation)
    {
        foreach (Vector3 matterSpot in matter)
        {
            foreach (Vector3 areaSpot in area) {
                if (!(location + matterSpot == areaLocation + areaSpot))
                {
                    return false;
                }
            }
        }

        return true;
    } 

    public void executeUpgrade(Upgrade upgrade) {
        if ((upgrade.upgradeObjectData.limitedCopies && upgrade.copiesLeft > 0) || (!upgrade.upgradeObjectData.limitedCopies)) {
            upgrade.upgradeObjectData.upgradeFunction.Invoke();
            if (upgrade.upgradeObjectData.limitedCopies) {
                upgrade.copiesLeft -= 1;
            }
        }
    }

    void Awake() {
        if (true/*file doesnt exist*/) {
            List<Object> upgradeFiles = new List<Object>(Resources.LoadAll("ScriptableObjects/Upgrades"));
            upgradeFiles.AddRange(Resources.LoadAll("ScriptableObjects/Upgrades/AddedUnitUpgrades"));

            Game.currentGame = new Game(Resources.LoadAll("ScriptableObjects/Sides"), upgradeFiles.ToArray(), Side.Life);
        } else
        {
            //load game | Game.currentGame = 

            foreach (SideData sideData in Game.currentGame.sides)
            {
                sideData.startReset();
            }
        }
        /////////////

        gameplayControl = this;
        currentGame = Game.currentGame;


        //sets turn variables

        List<Unit> sideUnits = new List<Unit>();
        foreach (SideDefine sideUnit in Object.FindObjectsOfType<SideDefine>())
        {
            if (sideUnit.GetComponent<Unit>() != null && sideUnit.GetComponent<SideDefine>().side == currentTurn)
            {
                sideUnits.Add(sideUnit.GetComponent<Unit>());
            }
        }

        Game.currentGame.findSide(currentTurn).currentOrbs = sideUnits.Count + Game.currentGame.findSide(currentTurn).addedOrbs;
        Game.currentGame.findSide(currentTurn).turns += 1;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!Game.currentGame.findSide(currentTurn).playerControlled)
        {
            GetComponent<AIManagement>().turn();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
