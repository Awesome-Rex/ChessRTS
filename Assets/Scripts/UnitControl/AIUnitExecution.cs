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
        OrderedDictionary movableAreas = new OrderedDictionary();
        foreach (Vector3 spot in Unit_Comp.movementAreaListed)
        {
            if (Unit_Comp.checkMovable(transform.position + spot))
            {
                //movableAreas.Add(transform.position + spot);
                movableAreas.Add(transform.position + spot, Vector3.Distance(transform.position, Vector3.zero/** change later **/));
            }
        }

        if (movableAreas.Count <= 0)
        {
            return;
        }


        OrderedDictionary safeMovableAreas = new OrderedDictionary();
        foreach (KeyValuePair<Vector3, float> spot in movableAreas)
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

            Dictionary<Vector3, float> spotsOrdered = safeMovableAreas.Cast<DictionaryEntry>().OrderBy(x => x.Value).ToDictionary(pair => (Vector3)pair.Key, pair => (float)pair.Value);
            //Dictionary<Vector3, float> spotsOrdered = safeMovableAreas.OrderBy(x => x.Value).ToDictionary(pair => pair.Key, pair => pair.Value);

            Unit_Comp.move(spotsOrdered.ElementAt(Random.Range(Mathf.CeilToInt(spotsOrdered.Count() / 2f) - 1, spotsOrdered.Count() - 1)).Key);
            /*Dictionary<Vector3, float> spotsOrdered = new Dictionary<Vector3, float>();
            foreach (KeyValuePair<Vector3, float> spot in safeMovableAreas)
            {

            }*/
        } else if (retreativeDodgitive_Picker > Unit_Comp.retreative)
        {
            //dodge / move forward
        }
    }

    public void attack() {
        float lowHighAggresive_Picker = Random.Range(0f, 100f);

        if (lowHighAggresive_Picker <= Unit_Comp.lowAggressive)
        {
            //attack low health
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
