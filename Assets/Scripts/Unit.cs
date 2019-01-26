﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    
    public bool[,] movementArea = new bool[17, 17];
    public Vector2 movementAreaDimensions = new Vector2(17, 17);

    public bool movementWallCrossable;
    public bool movementAllyCrossable;
    public bool movementEnemyCrossable;

    public List<Vector3> movementAreaListed = new List<Vector3>();
    public Vector2 savedMovementAreaDimensions;

    public enum MovementType {Straight, Autodirect, Jump, Teleport}


    public int[,] damageArea = new int[17, 17];
    public Vector2 damageAreaDimensions = new Vector2(17, 17);

    public bool damageWallCrossable;
    public bool damageAllyCrossable;
    public bool damageEnemyCrossable;

    public List<Vector3> damageAreaListed = new List<Vector3>();
    public List<int> damageListed = new List<int>();
    public Vector2 savedDamageAreaDimensions;



    public bool AI = false;

    private float _defensive;
    public float defensive {
        get {
            return _defensive;
        }
        set {
            _defensive = value;
            _offensive = 100 - value;
        }
    }

    private float _offensive;
    public float offensive {
        get {
            return _offensive;
        }
        set {
            _offensive = value;
            _defensive = 100 - value;
        }
    }

    public float retreative = 50;

    private float _lowAggressive;
    public float lowAggressive {
        get {
            return _lowAggressive;
        }
        set {
            _lowAggressive = value;
            _highAggressive = 100 - value;
        }
    }

    private float _highAggressive;
    public float highAggressive
    {
        get
        {
            return _highAggressive;
        }
        set
        {
            _highAggressive = value;
            _lowAggressive = 100 - value;
        }
    }

    public float defenseInfluence = 0;
    public float retreatInfluence = 0;

    public float priority;

    public bool hasDeterminedPriority;
    public float determinedPriority;

    //temp
    private Selectable Selectable_Comp;

    public void move(Vector3 targetPosition) {
        //animate transition

        if (GetComponent<Selectable>().selected)
        {
            GameplayControl.gameplayControl.GetComponent<SelectionManagement>().relocateSelected(targetPosition);
        }
        transform.position = targetPosition;
    } public void attack(Vector3 targetPosition) {
        Health damageTarget = Physics2D.Raycast(targetPosition, Vector3.zero, 0f).collider != null ? Physics2D.Raycast(targetPosition, Vector3.zero, 0f).collider.GetComponent<Health>() : null;

        if (damageTarget != null) {
            //checks if given damage is in damage area

            //foreach (Vector3 damagePosition in damageAreaListed) {
                //if (damagePosition == targetPosition) {
                    //raycast and subtract health

                    damageTarget.takeDamage(damageListed[damageAreaListed.IndexOf(targetPosition)]);
                //}
            //}
        }
    }

    public bool checkMovable(Vector3 targetPosition) {
        if (!movementAllyCrossable) {

        }
        if (!movementEnemyCrossable)
        {

        }
        if (!movementWallCrossable)
        {

        }
        return true;
    } public bool checkDamagable(Vector3 targetPosition) {
        if (!movementAllyCrossable)
        {

        }
        if (!movementEnemyCrossable)
        {

        }
        if (!movementWallCrossable)
        {

        }
        return true;
    }

    public void onSelect()
    {
        if (GameplayControl.gameplayControl.visualUnitAbility == GameplayControl.VisualUnitAbility.Movement) {
            visualizeMovementArea();
        } else if (GameplayControl.gameplayControl.visualUnitAbility == GameplayControl.VisualUnitAbility.Damage) {
            visualizeDamageArea();
        } else if (GameplayControl.gameplayControl.visualUnitAbility == GameplayControl.VisualUnitAbility.Nothing) {

        }
    }
    public void onDeselect() {
        //disable visualized ability

        transform.Find("VisualAbilities").Find("VisualAreas").GetChild(0).gameObject.SetActive(false);
        transform.Find("VisualAbilities").Find("VisualAreas").GetChild(1).gameObject.SetActive(false);
    }


    public void visualizeMovementArea()
    {
        transform.Find("VisualAbilities").Find("VisualAreas").GetChild(0).gameObject.SetActive(true);
    }

    public void visualizeDamageArea()
    {
        transform.Find("VisualAbilities").Find("VisualAreas").GetChild(1).gameObject.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        Selectable_Comp = GetComponent<Selectable>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse X") != 0f || Input.GetAxis("Mouse Y") != 0f) {
            if (Selectable_Comp.selected)
            {
                if (GameplayControl.gameplayControl.visualUnitAbility == GameplayControl.VisualUnitAbility.Movement && GameplayControl.gameplayControl.visualUnitAbilityMovement != GameplayControl.VisualUnitAbility.Nothing)
                {
                    Vector3 inputPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    inputPosition = new Vector3(Mathf.Round(inputPosition.x), Mathf.Round(inputPosition.y), 0f);

                    bool includedInArea = false;
                    
                    foreach (Vector3 movementSpot in movementAreaListed)
                    {
                        if (movementSpot + transform.position == inputPosition)
                        {
                            includedInArea = true;
                            break;
                        }
                    }

                    if (includedInArea) {
                        if (GameplayControl.gameplayControl.visualUnitAbilityMovement == GameplayControl.VisualUnitAbility.Movement)
                        {
                            transform.Find("VisualAbilities").Find("ExtraVisualAreas").GetChild(0).gameObject.SetActive(true);
                            transform.Find("VisualAbilities").Find("ExtraVisualAreas").GetChild(0).position = inputPosition;

                            transform.Find("VisualAbilities").Find("ExtraVisualAreas").GetChild(1).gameObject.SetActive(false);
                            //show extra movement area
                        } else if (GameplayControl.gameplayControl.visualUnitAbilityMovement == GameplayControl.VisualUnitAbility.Damage)
                        {
                            transform.Find("VisualAbilities").Find("ExtraVisualAreas").GetChild(1).gameObject.SetActive(true);
                            transform.Find("VisualAbilities").Find("ExtraVisualAreas").GetChild(1).position = inputPosition;

                            transform.Find("VisualAbilities").Find("ExtraVisualAreas").GetChild(0).gameObject.SetActive(false);
                            // show extra damage area
                        }
                    } else
                    {
                        transform.Find("VisualAbilities").Find("ExtraVisualAreas").GetChild(0).gameObject.SetActive(false);
                        transform.Find("VisualAbilities").Find("ExtraVisualAreas").GetChild(1).gameObject.SetActive(false);
                        //disable visual areas
                    }
                } else {
                    transform.Find("VisualAbilities").Find("ExtraVisualAreas").GetChild(0).gameObject.SetActive(false);
                    transform.Find("VisualAbilities").Find("ExtraVisualAreas").GetChild(1).gameObject.SetActive(false);
                    //disable visual areas
                }
            } else
            {
                transform.Find("VisualAbilities").Find("ExtraVisualAreas").GetChild(0).gameObject.SetActive(false);
                transform.Find("VisualAbilities").Find("ExtraVisualAreas").GetChild(1).gameObject.SetActive(false);
                //disable visual areass
            }
        }
    }
}
