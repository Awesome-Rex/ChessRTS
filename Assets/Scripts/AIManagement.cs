using System.Collections;
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
        List<RandomRangeElement> AIUnits = new List<RandomRangeElement>();

        float highestMax = 0;

        foreach (AIUnitExecution unit in GameObject.FindObjectsOfType<AIUnitExecution>())
        {
            AIUnits.Add(new RandomRangeElement(highestMax, unit.GetComponent<Unit>().priority, unit));
            highestMax += unit.GetComponent<Unit>().priority;
        }


        float picker = Random.Range(0f, highestMax);

        foreach (RandomRangeElement unit in AIUnits)
        {
            if (picker >= unit.min && picker < unit.max)
            {
                //make unit move/attack
                (unit.element as AIUnitExecution).action();
            }
        }
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
