using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitExecution : MonoBehaviour
{
    public bool abilityException;

    private SideDefine SideDefine_Comp;
    private Unit Unit_Comp;

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
                    inputPosition = new Vector3(Mathf.Round(inputPosition.x), Mathf.Round(inputPosition.y), 0f);

                    if (GameplayControl.gameplayControl.visualUnitAbility == GameplayControl.VisualUnitAbility.Movement)
                    {
                        Vector3 selectedSpot = transform.position;

                        foreach (Vector3 spot in Unit_Comp.movementAreaListed)
                        {
                            if (inputPosition == transform.position + spot)
                            {
                                selectedSpot = transform.position + spot;
                            }
                        }

                        if (selectedSpot != transform.position)
                        {
                            if (Unit_Comp.checkMovable(selectedSpot))
                            {
                                Unit_Comp.move(selectedSpot);

                                StartCoroutine(GameplayControl.gameplayControl.GetComponent<SelectionManagement>().selectionExceptionFrame());
                            }
                        }
                    }
                    else if (GameplayControl.gameplayControl.visualUnitAbility == GameplayControl.VisualUnitAbility.Damage)
                    {
                        Vector3 selectedSpot = transform.position;

                        foreach (Vector3 spot in Unit_Comp.damageAreaListed)
                        {
                            if (inputPosition == transform.position + spot)
                            {
                                selectedSpot = transform.position + spot;
                            }
                        }

                        if (selectedSpot != transform.position)
                        {
                            if (Unit_Comp.checkDamagable(selectedSpot))
                            {
                                Unit_Comp.attack(selectedSpot);

                                StartCoroutine(GameplayControl.gameplayControl.GetComponent<SelectionManagement>().selectionExceptionFrame());
                            }
                        }
                    }
                }
            }
        }
    }
}
