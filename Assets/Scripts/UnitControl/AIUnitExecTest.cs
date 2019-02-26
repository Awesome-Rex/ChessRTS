using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AIUnitExecTest : MonoBehaviour
{
    public int possibleActionsChosen = 1;

    //movemement
    public int sideDistanceInfluence = 1;
    public int damagableSpotsNextInfluence = 1;
    public int dangerDamageInfluence = 1;

    //damage
    
    

    
    
    private Unit Unit_Comp;
    private Health Health_Comp;
    private Matter Matter_Comp;
    private SideDefine SideDefine_Comp;

    public void doAction ()
    {
        List<AbilitySpot> actions = new List<AbilitySpot>();

        foreach (AbilitySpot movementSpot in Unit_Comp.movementAreaListed)
        {
            if (Unit_Comp.checkMovable(transform.position + movementSpot.location))
            {
                ///////// score can be negative!
                //form movement score (based on distance, damagable spots next, danger damage)


            }
        }

        foreach (AbilitySpot damageSpot in Unit_Comp.damageAreaListed)
        {
            if (Unit_Comp.checkDamagable(transform.position + damageSpot.location))
            {
                //form damage score (based on target health, damage done, will kill unit)

                RaycastHit2D healthCast = Physics2D.Raycast(transform.position + damageSpot.location, Vector3.zero, 0f, ~LayerMask.NameToLayer("Object"));

                if (healthCast.collider != null && healthCast.collider.GetComponent<Health>() != null)
                {
                    AbilitySpot newScoredAction = damageSpot.Clone();

                    float damageDoneEffectiviness = ((healthCast.collider.GetComponent<Health>().health - (healthCast.collider.GetComponent<Health>().health - damageSpot.damageValues[0])) / healthCast.collider.GetComponent<Health>().health) * 1;
                    float targetKillEffectiviness = (healthCast.collider.GetComponent<Health>().health - damageSpot.damageValues[0]) <= 0 ? 1 : 0;
                    float targetStrengthEffectiviness

                    newScoredAction.effectivityScore = 
                }
            }
        }

        if (actions.Count <= 0)
        {
            return;
        }

        /* might not work due to always danger damage
        
        int badActionCount = 0;
        foreach (AbilitySpot action in actions)
        {
            if (action.effectivityScore <= 0)
            {
                badActionCount++;
            }
        }
        if (badActionCount == actions.Count)
        {
            return;
        }*/

        // do good actions
        actions = actions.OrderBy(spot => spot.effectivityScore).ToList();
        AbilitySpot chosenAction = actions[Random.Range((actions.Count - (possibleActionsChosen - 1)) - 1, actions.Count)];

        if (chosenAction.damageValues.Count <= 0)
        {
            Unit_Comp.move(transform.position + chosenAction.location);
        } else if (chosenAction.damageValues.Count > 0)
        {
            Unit_Comp.attack(transform.position + chosenAction.location);
        }
    }

    void Awake()
    {
        Unit_Comp = GetComponent<Unit>();
        Health_Comp = GetComponent<Health>();
        Matter_Comp = GetComponent<Matter>();
        SideDefine_Comp = GetComponent<SideDefine>();
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
