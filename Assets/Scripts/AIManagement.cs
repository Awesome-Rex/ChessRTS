﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RandomRangeElement
{
    public float min;
    public float max;

    public Object element;

    public RandomRangeElement (float lastMax, float max, Object element)
    {
        min = lastMax;
        this.max = lastMax + max;

        this.element = element;
    }
}

public class AIManagement : MonoBehaviour
{
    public void turn ()
    {
        while (Game.currentGame.findSide(GameplayControl.gameplayControl.currentTurn).currentOrbs > 0) {
            List<RandomRangeElement> AIUnits = new List<RandomRangeElement>();
            float highestMax = 0;

            List<Unit> determinedPriorityUnits = new List<Unit>();

            foreach (AIUnitExecution unit in GameObject.FindObjectsOfType<AIUnitExecution>())
            {
                if (unit.GetComponent<SideDefine>().side == GameplayControl.gameplayControl.currentTurn && !unit.GetComponent<Unit>().hasDeterminedPriority) {
                    AIUnits.Add(new RandomRangeElement(highestMax, unit.GetComponent<Unit>().priority, unit));
                    highestMax += unit.GetComponent<Unit>().priority;
                } else
                {
                    //add ai determined priority, percentage
                    determinedPriorityUnits.Add(unit.GetComponent<Unit>());
                }
            }

            if (AIUnits.Count + determinedPriorityUnits.Count <= 0)
            {
                GameplayControl.gameplayControl.nextTurn();
                return;
            }

            float savedHighestMax = highestMax;

            float totalPercentage = 0f;

            foreach (Unit unit in determinedPriorityUnits)
            {
                totalPercentage += unit.determinedPriority;
            }
            foreach (Unit unit in determinedPriorityUnits) {
                AIUnits.Add(new RandomRangeElement(highestMax, (savedHighestMax / (100f - totalPercentage)) * unit.determinedPriority/*unit.determinedPriority*/, unit.GetComponent<AIUnitExecution>()));
                highestMax += (savedHighestMax / (100f - totalPercentage)) * unit.determinedPriority;
            } ///////////////fix here



            float picker = Random.Range(0f, highestMax);

            foreach (RandomRangeElement unit in AIUnits)
            {
                /////////////////////////// fix
                if (picker >= unit.min && picker < unit.max)
                {
                    //make unit move/attack
                    (unit.element as AIUnitExecution).action();
                    Game.currentGame.findSide(GameplayControl.gameplayControl.currentTurn).currentModularDamage = null;
                }
            }
        }

        GameplayControl.gameplayControl.nextTurn();
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
