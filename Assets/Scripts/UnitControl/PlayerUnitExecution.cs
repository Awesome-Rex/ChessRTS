using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitExecution : MonoBehaviour
{
    public bool defender;

    public bool abilityException;

    private SideDefine SideDefine_Comp;
    private Unit Unit_Comp;
    private Matter Matter_Comp;

    public IEnumerator abilityExceptionFrame ()
    {
        abilityException = true;

        yield return null;

        abilityException = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        abilityException = false;

        SideDefine_Comp = GetComponent<SideDefine>();
        Unit_Comp = GetComponent<Unit>();
        Matter_Comp = GetComponent<Matter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!abilityException) {
            if (GameplayControl.gameplayControl.currentTurn == SideDefine_Comp.side && GetComponent<Selectable>().selected)
            {

                if (Input.GetMouseButtonDown(0))
                {
                    Vector3 inputPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    

                    if (GameplayControl.gameplayControl.visualUnitAbility == GameplayControl.VisualUnitAbility.Movement)
                    {
                        if (Matter_Comp.savedMatterDimensions.x % 2 != 0 && Matter_Comp.savedMatterDimensions.y % 2 != 0)
                        {
                            inputPosition = new Vector3(Mathf.Round(inputPosition.x), Mathf.Round(inputPosition.y), 0f);
                        }
                        else if (Matter_Comp.savedMatterDimensions.x % 2 == 0 || Matter_Comp.savedMatterDimensions.y % 2 == 0)
                        {
                            if (Matter_Comp.savedMatterDimensions.x % 2 == 0 && Matter_Comp.savedMatterDimensions.y % 2 == 0)
                            {
                                inputPosition = new Vector3(Mathf.Sign(inputPosition.x) * (Mathf.Abs((int)inputPosition.x) + 0.5f), Mathf.Sign(inputPosition.y) * (Mathf.Abs((int)inputPosition.y) + 0.5f), 0f);
                            }
                            else if (Matter_Comp.savedMatterDimensions.x % 2 != 0 && Matter_Comp.savedMatterDimensions.y % 2 == 0)
                            {
                                inputPosition = new Vector3(Mathf.Round(inputPosition.x), Mathf.Sign(inputPosition.y) * (Mathf.Abs((int)inputPosition.y) + 0.5f), 0f);
                            }
                            else if (Matter_Comp.savedMatterDimensions.x % 2 == 0 && Matter_Comp.savedMatterDimensions.y % 2 != 0)
                            {
                                inputPosition = new Vector3(Mathf.Sign(inputPosition.x) * (Mathf.Abs((int)inputPosition.x) + 0.5f), Mathf.Round(inputPosition.y), 0f);
                            }
                        }

                        if (GameplayControl.containedInArea(inputPosition, Unit_Comp.movementAreaListed, transform.position))
                        {
                            if (Unit_Comp.checkMovable(inputPosition))
                            {
                                Unit_Comp.move(inputPosition);

                                StartCoroutine(GameplayControl.gameplayControl.GetComponent<SelectionManagement>().selectionExceptionFrame());
                            }
                        }
                    }
                    else if (GameplayControl.gameplayControl.visualUnitAbility == GameplayControl.VisualUnitAbility.Damage)
                    {
                        inputPosition = new Vector3(Mathf.Round(inputPosition.x), Mathf.Round(inputPosition.y), 0f);
                        /*if (Matter_Comp.savedMatterDimensions.x % 2 != 0 && Matter_Comp.savedMatterDimensions.y % 2 != 0)
                        {
                            inputPosition = new Vector3(Mathf.Round(inputPosition.x), Mathf.Round(inputPosition.y), 0f);
                        }
                        else if (Matter_Comp.savedMatterDimensions.x % 2 == 0 || Matter_Comp.savedMatterDimensions.y % 2 == 0)
                        {
                            if (Matter_Comp.savedMatterDimensions.x % 2 == 0 && Matter_Comp.savedMatterDimensions.y % 2 == 0)
                            {
                                inputPosition = new Vector3(Mathf.Sign(inputPosition.x) * (Mathf.Abs((int)inputPosition.x) + 0.5f), Mathf.Sign(inputPosition.y) * (Mathf.Abs((int)inputPosition.y) + 0.5f), 0f);
                            }
                            else if (Matter_Comp.savedMatterDimensions.x % 2 != 0 && Matter_Comp.savedMatterDimensions.y % 2 == 0)
                            {
                                inputPosition = new Vector3(Mathf.Round(inputPosition.x), Mathf.Sign(inputPosition.y) * (Mathf.Abs((int)inputPosition.y) + 0.5f), 0f);
                            }
                            else if (Matter_Comp.savedMatterDimensions.x % 2 == 0 && Matter_Comp.savedMatterDimensions.y % 2 != 0)
                            {
                                inputPosition = new Vector3(Mathf.Sign(inputPosition.x) * (Mathf.Abs((int)inputPosition.x) + 0.5f), Mathf.Round(inputPosition.y), 0f);
                            }
                        }*/

                        if (GameplayControl.containedInArea(inputPosition, Unit_Comp.damageAreaListed, transform.position))
                        {
                            if (Unit_Comp.checkDamagable(inputPosition))
                            {
                                Unit_Comp.attack(inputPosition);

                                StartCoroutine(GameplayControl.gameplayControl.GetComponent<SelectionManagement>().selectionExceptionFrame());
                            }
                        }
                    }
                }
            }
        }
    }
}
