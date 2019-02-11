using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManagement : MonoBehaviour
{
    public static SelectionManagement selectionManagement;

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

    public bool selectionException;


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


    public IEnumerator selectionExceptionFrame ()
    {
        selectionException = true;

        yield return null;

        selectionException = false;
    }


    // Start is called before the first frame update
    void Start()
    {
        selectionManagement = this;

        targetPositionObject.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0f);

        selectionException = false;
        hoverException = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!selectionException) {
            if (Input.GetMouseButtonDown(0)) {
                Vector3 inputPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                inputPosition = new Vector3(Mathf.Round(inputPosition.x), Mathf.Round(inputPosition.y), 0f);


                bool inputOnTargetObject = false;

                Selectable targetedObjectCast = Physics2D.Raycast(inputPosition, Vector3.zero, 0f, ~LayerMask.NameToLayer("Object")).collider != null ?
                        Physics2D.Raycast(inputPosition, Vector3.zero, 0f, ~LayerMask.NameToLayer("Object")).collider.GetComponent<Selectable>() :
                        null;

                if (targetedObjectCast != null && targetedObject != null && targetedObjectCast == targetedObject)
                {
                    inputOnTargetObject = true;
                }


                bool includedInArea = false;
                if (/*!inputOnTargetObject && */targetedObject != null && targetedObject.GetComponent<PlayerUnitExecution>() != null && targetedObject.selected && GameplayControl.gameplayControl.visualUnitAbility != GameplayControl.VisualUnitAbility.Nothing && GameplayControl.gameplayControl.currentTurn == targetedObject.GetComponent<SideDefine>().side)
                {
                    if (GameplayControl.gameplayControl.visualUnitAbility == GameplayControl.VisualUnitAbility.Movement)
                    {
                        if (targetedObject.GetComponent<Matter>().savedMatterDimensions.x % 2 == 0 || targetedObject.GetComponent<Matter>().savedMatterDimensions.y % 2 == 0)
                        {
                            inputPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                            //inputPosition = new Vector3(Mathf.Sign(inputPosition.x) * (Mathf.Abs((int)inputPosition.x) + 0.5f), Mathf.Sign(inputPosition.y) * (Mathf.Abs((int)inputPosition.y) + 0.5f), 0f);
                            if (targetedObject.GetComponent<Matter>().savedMatterDimensions.x % 2 == 0 && targetedObject.GetComponent<Matter>().savedMatterDimensions.y % 2 == 0)
                            {
                                inputPosition = new Vector3(Mathf.Sign(inputPosition.x) * (Mathf.Abs((int)inputPosition.x) + 0.5f), Mathf.Sign(inputPosition.y) * (Mathf.Abs((int)inputPosition.y) + 0.5f), 0f);
                            }
                            else if (targetedObject.GetComponent<Matter>().savedMatterDimensions.x % 2 != 0 && targetedObject.GetComponent<Matter>().savedMatterDimensions.y % 2 == 0)
                            {
                                inputPosition = new Vector3(Mathf.Round(inputPosition.x), Mathf.Sign(inputPosition.y) * (Mathf.Abs((int)inputPosition.y) + 0.5f), 0f);
                            }
                            else if (targetedObject.GetComponent<Matter>().savedMatterDimensions.x % 2 == 0 && targetedObject.GetComponent<Matter>().savedMatterDimensions.y % 2 != 0)
                            {
                                inputPosition = new Vector3(Mathf.Sign(inputPosition.x) * (Mathf.Abs((int)inputPosition.x) + 0.5f), Mathf.Round(inputPosition.y), 0f);
                            }
                        }

                        if (GameplayControl.containedInArea(inputPosition, targetedObject.GetComponent<Unit>().movementAreaListed_deprecated, targetedObject.transform.position))
                        {
                            includedInArea = true;
                        }
                    } else if (GameplayControl.gameplayControl.visualUnitAbility == GameplayControl.VisualUnitAbility.Damage)
                    {
                        if (GameplayControl.containedInArea(inputPosition, targetedObject.GetComponent<Unit>().damageAreaListed_deprecated, targetedObject.transform.position))
                        {
                            includedInArea = true;
                        }
                    }
                }
                
                //if player unit not selected
                if (
                    !(/*!inputOnTargetObject && */targetedObject != null && targetedObject.GetComponent<PlayerUnitExecution>() != null && targetedObject.selected /*inputPosition != targetedObject.transform.position*/ && GameplayControl.gameplayControl.visualUnitAbility != GameplayControl.VisualUnitAbility.Nothing && GameplayControl.gameplayControl.currentTurn == targetedObject.GetComponent<SideDefine>().side)
                    || (!includedInArea && (/*!inputOnTargetObject && */targetedObject != null && targetedObject.GetComponent<PlayerUnitExecution>() != null && targetedObject.selected /*inputPosition != targetedObject.transform.position*/ && GameplayControl.gameplayControl.visualUnitAbility != GameplayControl.VisualUnitAbility.Nothing && GameplayControl.gameplayControl.currentTurn == targetedObject.GetComponent<SideDefine>().side))
                    ) {
                    //check if selection is outside of movement or damage area
                    inputPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    inputPosition = new Vector3(Mathf.Round(inputPosition.x), Mathf.Round(inputPosition.y), 0f);


                    targetPosition = inputPosition;
                    // fixed ////////////////////////error
                    if (targetedObject != null && /*inputPosition != targetedObject.transform.position*/ !inputOnTargetObject)
                    {
                        targetedObject.selected = false;
                        objectSelected = false;

                    }

                    if (targetedObjectCast != null) {
                        targetedObject = targetedObjectCast;
                        targetPosition = targetedObjectCast.transform.position;

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

                        if (targetedObjectCast.GetComponent<PlayerUnitExecution>() != null)
                        {
                            StartCoroutine(targetedObjectCast.GetComponent<PlayerUnitExecution>().abilityExceptionFrame());
                        }
                    } else
                    {
                        targetedObject = null;

                        objectSelected = false;
                        targetPositionObject.GetComponent<SpriteRenderer>().color = Color.white;
                    }
                }
            }
        }

        if (!hoverException) {
            if (Input.GetAxis("Mouse X") != 0f || Input.GetAxis("Mouse Y") != 0f) {
                
                Vector3 inputPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                if (targetedObject != null && targetedObject.GetComponent<Unit>() != null && GameplayControl.gameplayControl.visualUnitAbility == GameplayControl.VisualUnitAbility.Movement && (targetedObject.GetComponent<Matter>().savedMatterDimensions.x % 2 == 0 || targetedObject.GetComponent<Matter>().savedMatterDimensions.y % 2 == 0)) {
                    //if even

                    bool inArea = false;

                    if (targetedObject.GetComponent<Matter>().savedMatterDimensions.x % 2 == 0 && targetedObject.GetComponent<Matter>().savedMatterDimensions.y % 2 == 0)
                    {
                        if (GameplayControl.containedInArea(new Vector3(Mathf.Sign(inputPosition.x) * (Mathf.Abs((int)inputPosition.x) + 0.5f), Mathf.Sign(inputPosition.y) * (Mathf.Abs((int)inputPosition.y) + 0.5f), 0f), targetedObject.GetComponent<Unit>().movementAreaListed_deprecated, targetedObject.transform.position)) {
                            inputPosition = new Vector3(Mathf.Sign(inputPosition.x) * (Mathf.Abs((int)inputPosition.x) + 0.5f), Mathf.Sign(inputPosition.y) * (Mathf.Abs((int)inputPosition.y) + 0.5f), 0f);
                            inArea = true;
                        }
                    } else if (targetedObject.GetComponent<Matter>().savedMatterDimensions.x % 2 != 0 && targetedObject.GetComponent<Matter>().savedMatterDimensions.y % 2 == 0)
                    {
                        if (GameplayControl.containedInArea(new Vector3(Mathf.Round(inputPosition.x), Mathf.Sign(inputPosition.y) * (Mathf.Abs((int)inputPosition.y) + 0.5f), 0f), targetedObject.GetComponent<Unit>().movementAreaListed_deprecated, targetedObject.transform.position))
                        {
                            inputPosition = new Vector3(Mathf.Round(inputPosition.x), Mathf.Sign(inputPosition.y) * (Mathf.Abs((int)inputPosition.y) + 0.5f), 0f);
                            inArea = true;
                        }
                    } else if (targetedObject.GetComponent<Matter>().savedMatterDimensions.x % 2 == 0 && targetedObject.GetComponent<Matter>().savedMatterDimensions.y % 2 != 0) {
                        if (GameplayControl.containedInArea(new Vector3(Mathf.Sign(inputPosition.x) * (Mathf.Abs((int)inputPosition.x) + 0.5f), Mathf.Round(inputPosition.y), 0f), targetedObject.GetComponent<Unit>().movementAreaListed_deprecated, targetedObject.transform.position))
                        {
                            inputPosition = new Vector3(Mathf.Sign(inputPosition.x) * (Mathf.Abs((int)inputPosition.x) + 0.5f), Mathf.Round(inputPosition.y), 0f);
                            inArea = true;
                        }
                    }

                    if (inArea) {
                        //inputPosition = new Vector3(Mathf.Sign(inputPosition.x) * (Mathf.Abs((int)inputPosition.x) + 0.5f), Mathf.Sign(inputPosition.y) * (Mathf.Abs((int)inputPosition.y) + 0.5f), 0f);
                        hoverPosition = inputPosition;
                    } else
                    {
                        inputPosition = new Vector3(Mathf.Round(inputPosition.x), Mathf.Round(inputPosition.y), 0f);
                        hoverPosition = inputPosition;
                    }
                } else
                {
                    // if odd

                    inputPosition = new Vector3(Mathf.Round(inputPosition.x), Mathf.Round(inputPosition.y), 0f);
                    hoverPosition = inputPosition;
                }
                

                //display selected hover data
            }
        }
    }
}
