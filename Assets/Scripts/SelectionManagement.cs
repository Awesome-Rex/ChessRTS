using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManagement : MonoBehaviour
{
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

    public Selectable targetedObject;
    public bool objectSelected;

    public GameObject hoverPositionObject;
    public Vector3 hoverPosition
    {
        get
        {
            return hoverPositionObject.transform.position;
        }
        set
        {
            hoverPositionObject.transform.position = value;
        }
    }

    public Selectable hoveredObject;

    public bool hoverException;

    public void relocateSelected (Vector3 newPosition)
    {
        targetPosition = newPosition;
    }

    // Start is called before the first frame update
    void Start()
    {
        targetPositionObject.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0f);
        hoverException = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 inputPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            inputPosition = new Vector3(Mathf.Round(inputPosition.x), Mathf.Round(inputPosition.y), 0f);


            bool includedInArea = false;
            if (targetedObject != null && targetedObject.GetComponent<PlayerUnitExecution>() != null && targetedObject.selected && inputPosition != targetedObject.transform.position && GameplayControl.gameplayControl.visualUnitAbility != GameplayControl.VisualUnitAbility.Nothing && GameplayControl.gameplayControl.currentTurn == targetedObject.GetComponent<SideDefine>().side)
            {
                if (GameplayControl.gameplayControl.visualUnitAbility == GameplayControl.VisualUnitAbility.Movement)
                {
                    foreach (Vector3 spot in targetedObject.GetComponent<Unit>().movementAreaListed)
                    {
                        if (inputPosition == targetedObject.transform.position + spot)
                        {
                            includedInArea = true;
                        }
                    }
                } else if (GameplayControl.gameplayControl.visualUnitAbility == GameplayControl.VisualUnitAbility.Damage)
                {
                    foreach (Vector3 spot in targetedObject.GetComponent<Unit>().damageAreaListed)
                    {
                        if (inputPosition == targetedObject.transform.position + spot)
                        {
                            includedInArea = true;
                        }
                    }
                }
            }

            //if player unit not selected
            if (
                !(targetedObject != null && targetedObject.GetComponent<PlayerUnitExecution>() != null && targetedObject.selected && inputPosition != targetedObject.transform.position && GameplayControl.gameplayControl.visualUnitAbility != GameplayControl.VisualUnitAbility.Nothing && GameplayControl.gameplayControl.currentTurn == targetedObject.GetComponent<SideDefine>().side)
                || (!includedInArea && (targetedObject != null && targetedObject.GetComponent<PlayerUnitExecution>() != null && targetedObject.selected && inputPosition != targetedObject.transform.position && GameplayControl.gameplayControl.visualUnitAbility != GameplayControl.VisualUnitAbility.Nothing && GameplayControl.gameplayControl.currentTurn == targetedObject.GetComponent<SideDefine>().side))
                ) {
                //check if selection is outside of movement or damage area
                
                targetPosition = inputPosition;
                //////////////////////////error
                if (targetedObject != null && inputPosition != targetedObject.transform.position)
                {
                    targetedObject.selected = false;
                    objectSelected = false;

                }

                Selectable targetedObjectCast = Physics2D.Raycast(inputPosition, Vector3.zero, 0f, ~LayerMask.NameToLayer("Object")).collider != null ?
                    Physics2D.Raycast(inputPosition, Vector3.zero, 0f, ~LayerMask.NameToLayer("Object")).collider.GetComponent<Selectable>() :
                    null;

                if (targetedObjectCast != null) {
                    targetedObject = targetedObjectCast;

                    targetedObjectCast.selected = !targetedObjectCast.selected;
                    objectSelected = targetedObjectCast.selected;

                    if (objectSelected)
                    {
                        targetPositionObject.GetComponent<SpriteRenderer>().color = Color.white;
                    }
                    else if (!objectSelected)
                    {
                        targetPositionObject.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0.5f);
                    }
                } else
                {
                    targetedObject = null;

                    objectSelected = false;
                    targetPositionObject.GetComponent<SpriteRenderer>().color = Color.white;
                }
            }
        }

        if (!hoverException) {
            if (Input.GetAxis("Mouse X") != 0f || Input.GetAxis("Mouse Y") != 0f) {
                Vector3 inputPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                inputPosition = new Vector3(Mathf.Round(inputPosition.x), Mathf.Round(inputPosition.y), 0f);

                hoverPosition = inputPosition;

                //display selected hover data
            }
        }
    }
}
