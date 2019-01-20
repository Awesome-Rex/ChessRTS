using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitExecution : MonoBehaviour
{
    private SideDefine SideDefine_Comp;

    // Start is called before the first frame update
    void Start()
    {
        SideDefine_Comp = GetComponent<SideDefine>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameplayControl.gameplayControl.currentTurn == SideDefine_Comp.side) {

            //if (/*on click*/) {
                if (GameplayControl.gameplayControl.visualUnitAbility == GameplayControl.VisualUnitAbility.Movement) {

                } else if (GameplayControl.gameplayControl.visualUnitAbility == GameplayControl.VisualUnitAbility.Damage) {

                }
            //}

        }
    }
}
