using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections.Specialized;

public class AIUnitExecution : MonoBehaviour
{
    private Unit Unit_Comp;
    private Health Health_Comp;
    private Matter Matter_Comp;

    public void action ()
    {
        float moveAttack_Picker = Random.Range(0f, 100f);

        if (moveAttack_Picker <= Unit_Comp.defensive)
        {
            move();
        } else if (moveAttack_Picker > Unit_Comp.defensive)
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
                movableAreas.Add(new AbilitySpot(spot.location, transform.position + spot.location, Vector3.Distance(transform.position, Vector3.zero/** change later **/)));
            }
        }

        if (movableAreas.Count <= 0)
        {
            return;
        }


        List<AbilitySpot> safeMovableAreas = new List<AbilitySpot>();
        foreach (AbilitySpot spot in movableAreas)
        {
            /*if (GameplayControl.containedInArea(Matter_Comp.matterAreaListed, spot, **all damageable areas**, Vector3.zero))
            {
                safeMovableAreas.Add(spot);
            }*/
        }


        float retreativeDodgitive_Picker = Random.Range(0f, 100f);

        if (retreativeDodgitive_Picker <= Unit_Comp.retreative)
        {
            ////////////////retreat

            //Dictionary<Vector3, float> spotsOrdered = safeMovableAreas.Cast<DictionaryEntry>().OrderBy(x => x.Value).ToDictionary(pair => (Vector3)pair.Key, pair => (float)pair.Value);
            //Dictionary<Vector3, float> spotsOrdered = safeMovableAreas.OrderBy(x => x.Value).ToDictionary(pair => pair.Key, pair => pair.Value);

            safeMovableAreas = safeMovableAreas.OrderBy(x => x.distanceFromSide).ToList();

            Unit_Comp.move(safeMovableAreas.ElementAt(Random.Range(Mathf.CeilToInt(safeMovableAreas.Count() / 2f) - 1, safeMovableAreas.Count())).worldLocation);
            /*Dictionary<Vector3, float> spotsOrdered = new Dictionary<Vector3, float>();
            foreach (KeyValuePair<Vector3, float> spot in safeMovableAreas)
            {

            }*/
        } else if (retreativeDodgitive_Picker > Unit_Comp.retreative)
        {
            //dodge / move forward

            Unit_Comp.move(safeMovableAreas.ElementAt(Random.Range(0, safeMovableAreas.Count)).worldLocation);
        }
    }

    public void attack() {
        List<AbilitySpot> damagableSpots = new List<AbilitySpot>();
        foreach (AbilitySpot spot in Unit_Comp.damageAreaListed)
        {
            if (Unit_Comp.checkDamagable(transform.position + spot.location))
            {
                AbilitySpot damageSpot = new AbilitySpot(spot.location, spot.damageValues[0]);
                damageSpot.worldLocation = transform.position + spot.location;

                damagableSpots.Add(damageSpot);
            }
        }


        float lowHighAggresive_Picker = Random.Range(0f, 100f);

        if (lowHighAggresive_Picker <= Unit_Comp.lowAggressive)
        {
            //attack low health

            List<AbilitySpot> targetDamageSpots = new List<AbilitySpot>();

            foreach (AbilitySpot spot in damagableSpots)
            {
                RaycastHit2D enemyCast = Physics2D.Raycast(spot.worldLocation, Vector3.zero, 0f, ~LayerMask.NameToLayer("Object"));
                if (enemyCast.collider != null && enemyCast.collider.GetComponent<Health>() != null && enemyCast.collider.GetComponent<Health>().health <= Mathf.FloorToInt(enemyCast.collider.GetComponent<Health>().maxHealth / 2))
                {
                    targetDamageSpots.Add(spot);
                }
            }
            /////////add damage and health score
            Unit_Comp.attack(targetDamageSpots[Random.Range(0, targetDamageSpots.Count)].worldLocation);
        } else if (lowHighAggresive_Picker > Unit_Comp.lowAggressive)
        {
            //attack high health

            
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Unit_Comp = GetComponent<Unit>();
        Health_Comp = GetComponent<Health>();
        Matter_Comp = GetComponent<Matter>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
