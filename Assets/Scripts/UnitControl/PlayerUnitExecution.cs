using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitExecution : MonoBehaviour
{
    private SideDefine SideDefine_Comp;
    private Unit Unit_Comp;

    // Start is called before the first frame update
    void Start()
    {
        SideDefine_Comp = GetComponent<SideDefine>();
        Unit_Comp = GetComponent<Unit>();
    }

    // Update is called once per frame
    void Update()
    {
/*        if (GameplayControl.gameplayControl.currentTurn == SideDefine_Comp.side) {

            if (/*on click*//*) {
                if (GameplayControl.gameplayControl.visualUnitAbility == GameplayControl.VisualUnitAbility.Movement) {
                    Vector3 selectedSpot = Vector3.positiveInfinity;

                    foreach (Vector3 spot in Unit_Comp.movementAreaListed) {
                        if (/*cursor position rounded == spot*//*) {
/*                            selectedSpot = spot;
                            break;
                        }
                    }

                    if (selectedSpot != Vector3.positiveInfinity) {
                        if (Unit_Comp.checkMovable(selectedSpot)) {
                            Unit_Comp.move(selectedSpot);
                        }
                    }
                } else if (GameplayControl.gameplayControl.visualUnitAbility == GameplayControl.VisualUnitAbility.Damage) {
                    Vector3 selectedSpot = Vector3.positiveInfinity;

                    foreach (Vector3 spot in Unit_Comp.damageAreaListed)
                    {
                        if (/*cursor position rounded == spot*//*)
/*                        {
                            selectedSpot = spot;
                            break;
                        }
                    }

                    if (selectedSpot != Vector3.positiveInfinity)
                    {
                        if (Unit_Comp.checkDamagable(selectedSpot))
                        {
                            Unit_Comp.attack(selectedSpot);
                        }
                    }
                }
            }
            
        }*/
    }
}
