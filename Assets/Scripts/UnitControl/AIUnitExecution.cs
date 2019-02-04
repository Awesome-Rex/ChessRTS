using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIUnitExecution : MonoBehaviour
{
    private Unit Unit_Comp;
    private Health Health_Comp;

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
        float retreativeDodgitive_Picker = Random.Range(0f, 100f);

        if (retreativeDodgitive_Picker <= Unit_Comp.retreative)
        {
            //retreat
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
