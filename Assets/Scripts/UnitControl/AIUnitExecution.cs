using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections.Specialized;

public class AIUnitExecution : MonoBehaviour
{
    public bool canAttackJackpot;
    
    public int maxDangerDamage;
    public float avgDangerDamage;
    
    public Unit highestHealthEnemy;
    public Unit lowestHealthEnemy;
    
    public Unit highestAvgDamageEnemy;
    public Unit lowestAvgDamageEnemy;    
    
    public int movableSpotCount;
    public int safeMovableSpotCount;
    
    public int farSafeMovableSpotCount;
    public int closeSafeMovableSpotCount;
    
    
    
    private Unit Unit_Comp;
    private Health Health_Comp;
    private Matter Matter_Comp;
    private SideDefine SideDefine_Comp;

    public void action ()
    {
        //Debug.Log("I did an action!");

        float moveAttack_Picker = Random.Range(0f, 100f);

        if (moveAttack_Picker <= Unit_Comp.defensive + ((Health_Comp.maxHealth - Health_Comp.health) * Unit_Comp.defenseInfluence))
        {
            move();
        } else if (moveAttack_Picker > Unit_Comp.defensive + ((Health_Comp.maxHealth - Health_Comp.health) * Unit_Comp.defenseInfluence))
        {
            attack();
        }
    }

    public void move() {
        List<AbilitySpot> movableAreas = new List<AbilitySpot>();
        foreach (AbilitySpot spot in Unit_Comp.movementAreaListed)
        {
            if (Unit_Comp.checkMovable(transform.position + spot.location))
            {
                //movableAreas.Add(transform.position + spot);
                movableAreas.Add(new AbilitySpot(this, spot.location, transform.position + spot.location, Vector3.Distance(transform.position, Vector3.zero/** change later **/)));
            }
        }


        List<Side> enemySides = new List<Side>();
        foreach (SideData sideData in Game.currentGame.sides)
        {
            if (sideData.sideSettings.side != Side.Nothing && sideData.sideSettings.side != SideDefine_Comp.side) {
                enemySides.Add(sideData.sideSettings.side);
            }
        }
        List<AbilitySpot> unsafeAreas = GameplayControl.combineModularDamage(enemySides);


        List<AbilitySpot> safeMovableAreas = new List<AbilitySpot>();
        foreach (AbilitySpot spot in movableAreas)
        {
            if (!GameplayControl.containedInArea(Matter_Comp.matterAreaListed, spot.source.transform.position + spot.location, unsafeAreas, Vector3.zero))
            {
                safeMovableAreas.Add(spot);
            }
        }

        

        float retreativeDodgitive_Picker = Random.Range(0f, 100f);

        if (retreativeDodgitive_Picker <= Unit_Comp.retreative + ((Health_Comp.maxHealth - Health_Comp.health) * Unit_Comp.retreatInfluence))
        {
            //retreat
            if (safeMovableAreas.Count > 0)
            {
                safeMovableAreas = safeMovableAreas.OrderBy(x => x.distanceFromSide).ToList();

                Unit_Comp.move(safeMovableAreas.ElementAt(Random.Range(Tools.underZero(Mathf.FloorToInt(safeMovableAreas.Count() / 2f) - 1), safeMovableAreas.Count() - 1)).worldLocation);
            } else if (safeMovableAreas.Count <= 0 && movableAreas.Count > 0)
            {
                movableAreas = movableAreas.OrderBy(x => x.distanceFromSide).ToList();

                Unit_Comp.move(movableAreas.ElementAt(Random.Range(Tools.underZero(Mathf.FloorToInt(movableAreas.Count() / 2f) - 1), movableAreas.Count() - 1)).worldLocation);
            } else if (movableAreas.Count <= 0)
            {
                return;
            }
        } else if (retreativeDodgitive_Picker > Unit_Comp.retreative + ((Health_Comp.maxHealth - Health_Comp.health) * Unit_Comp.retreatInfluence))
        {
            //dodge / move forward

            if (safeMovableAreas.Count > 0)
            {
                safeMovableAreas = safeMovableAreas.OrderBy(x => x.distanceFromSide).ToList();

                Unit_Comp.move(safeMovableAreas.ElementAt(Random.Range(0, Mathf.CeilToInt(safeMovableAreas.Count() / 2f) - 1)).worldLocation);
            }
            else if (safeMovableAreas.Count <= 0 && movableAreas.Count > 0)
            {
                movableAreas = movableAreas.OrderBy(x => x.distanceFromSide).ToList();

                Unit_Comp.move(movableAreas.ElementAt(Random.Range(0, Mathf.CeilToInt(movableAreas.Count() / 2f) - 1)).worldLocation);
            } else if (movableAreas.Count <= 0)
            {
                return;
            }
        }
    }

    public void retreat ()
    {

    }

    public void dodge()
    {

    }

    public void attack() {
        Debug.Log("I'm attacking!");

        List<AbilitySpot> damagableSpots = new List<AbilitySpot>();
        foreach (AbilitySpot spot in Unit_Comp.damageAreaListed)
        {
            if (Unit_Comp.checkDamagable(transform.position + spot.location))
            {
                AbilitySpot damageSpot = new AbilitySpot(this, spot.location, spot.damageValues[0]);
                damageSpot.worldLocation = transform.position + spot.location;

                RaycastHit2D enemyCast = Physics2D.Raycast(transform.position + spot.location, Vector3.zero, 0f, ~LayerMask.NameToLayer("Object"));
                /*if (enemyCast.collider != null && enemyCast.collider.GetComponent<Health>() != null)
                {*/
                    damageSpot.damageTarget = enemyCast.collider.GetComponent<Health>();
                //}

                damagableSpots.Add(damageSpot);
            }
        }

        if (damagableSpots.Count <= 0)
        {
            Debug.Log("I cant attack?");
            return;
        }

        damagableSpots.OrderBy(spot => spot.damageTarget.health / spot.damageTarget.maxHealth).ToList();

        float lowHighAggresive_Picker = Random.Range(0f, 100f);
        if (lowHighAggresive_Picker <= Unit_Comp.lowAggressive) {
            //attack low health

            /*List<AbilitySpot> targetDamageSpots = new List<AbilitySpot>();

            foreach (AbilitySpot spot in damagableSpots)
            {
                RaycastHit2D enemyCast = Physics2D.Raycast(spot.worldLocation, Vector3.zero, 0f, ~LayerMask.NameToLayer("Object"));
                if (enemyCast.collider != null && enemyCast.collider.GetComponent<Health>() != null && enemyCast.collider.GetComponent<Health>().health <= Mathf.FloorToInt(enemyCast.collider.GetComponent<Health>().maxHealth / 2))
                {
                    targetDamageSpots.Add(spot);
                }
            }

            if (targetDamageSpots.Count <= 0)
            {
                return;
            }

            Unit_Comp.attack(targetDamageSpots[Random.Range(0, targetDamageSpots.Count - 1)].worldLocation);*/
            Unit_Comp.attack(damagableSpots[Random.Range(0, Tools.underZero(Mathf.FloorToInt((Unit_Comp.lowAggressive / 100f) * (damagableSpots.Count - 1))) + 1)].worldLocation);
            Debug.Log("I just attacked!");
            /////////add damage and health score
        } else if (lowHighAggresive_Picker > Unit_Comp.lowAggressive) {
            //attack high health

            /*List<AbilitySpot> targetDamageSpots = new List<AbilitySpot>();

            foreach (AbilitySpot spot in damagableSpots)
            {
                RaycastHit2D enemyCast = Physics2D.Raycast(spot.worldLocation, Vector3.zero, 0f, ~LayerMask.NameToLayer("Object"));
                if (enemyCast.collider != null && enemyCast.collider.GetComponent<Health>() != null && enemyCast.collider.GetComponent<Health>().health > Mathf.FloorToInt(enemyCast.collider.GetComponent<Health>().maxHealth / 2))
                {
                    targetDamageSpots.Add(spot);
                }
            }

            if (targetDamageSpots.Count <= 0)
            {
                return;
            }

            Unit_Comp.attack(targetDamageSpots[Random.Range(0, targetDamageSpots.Count - 1)].worldLocation);*/
            Unit_Comp.attack(damagableSpots[Random.Range(Mathf.CeilToInt((Unit_Comp.lowAggressive / 100f) * (damagableSpots.Count - 1)), damagableSpots.Count)].worldLocation);
            Debug.Log("I just attacked!");
            /////////add damage and health score
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        Unit_Comp = GetComponent<Unit>();
        Health_Comp = GetComponent<Health>();
        Matter_Comp = GetComponent<Matter>();
        SideDefine_Comp = GetComponent<SideDefine>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
