using System.Collections;
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
    private Matter Matter_Comp;

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
        Matter_Comp = GetComponent<Matter>();
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

                    if (Matter_Comp.savedMatterDimensions.x % 2f != 0 && Matter_Comp.savedMatterDimensions.y % 2f != 0) {
                        inputPosition = new Vector3(Mathf.Round(inputPosition.x), Mathf.Round(inputPosition.y), 0f);
                    } else if (Matter_Comp.savedMatterDimensions.x % 2 == 0 || Matter_Comp.savedMatterDimensions.y % 2 == 0)
                    {
                        if (Matter_Comp.savedMatterDimensions.x % 2 == 0 && Matter_Comp.savedMatterDimensions.y % 2 == 0) {
                            inputPosition = new Vector3(Mathf.Sign(inputPosition.x) * (Mathf.Abs((int)inputPosition.x) + 0.5f), Mathf.Sign(inputPosition.y) * (Mathf.Abs((int)inputPosition.y) + 0.5f), 0f);
                        } else if (Matter_Comp.savedMatterDimensions.x % 2 != 0 && Matter_Comp.savedMatterDimensions.y % 2 == 0)
                        {
                            inputPosition = new Vector3(Mathf.Round(inputPosition.x), Mathf.Sign(inputPosition.y) * (Mathf.Abs((int)inputPosition.y) + 0.5f), 0f);
                        } else if (Matter_Comp.savedMatterDimensions.x % 2 == 0 && Matter_Comp.savedMatterDimensions.y % 2 != 0)
                        {
                            inputPosition = new Vector3(Mathf.Sign(inputPosition.x) * (Mathf.Abs((int)inputPosition.x) + 0.5f), Mathf.Round(inputPosition.y), 0f);
                        }
                    }

                    if (GameplayControl.containedInArea(inputPosition, movementAreaListed, transform.position)) {
                        GameplayControl.gameplayControl.GetComponent<SelectionManagement>().hoverPositionObject.GetComponent<SpriteRenderer>().color = Color.clear;
                        //GameplayControl.gameplayControl.GetComponent<SelectionManagement>().hoverPositionObject.SetActive(false);
                        Debug.Log("Supposed to be disabling hover object!");

                        transform.Find("VisualAbilities").Find("ExtraVisualAreas").GetChild(2).gameObject.SetActive(true);
                        transform.Find("VisualAbilities").Find("ExtraVisualAreas").GetChild(2).position = inputPosition;

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
                        //GameplayControl.gameplayControl.GetComponent<SelectionManagement>().hoverPositionObject.GetComponent<SpriteRenderer>().enabled = true;
                        GameplayControl.gameplayControl.GetComponent<SelectionManagement>().hoverPositionObject.GetComponent<SpriteRenderer>().color = Color.white;
                        transform.Find("VisualAbilities").Find("ExtraVisualAreas").GetChild(0).gameObject.SetActive(false);
                        transform.Find("VisualAbilities").Find("ExtraVisualAreas").GetChild(1).gameObject.SetActive(false);
                        transform.Find("VisualAbilities").Find("ExtraVisualAreas").GetChild(2).gameObject.SetActive(false);
                        //disable visual areas
                    }
                } else {
                    GameplayControl.gameplayControl.GetComponent<SelectionManagement>().hoverPositionObject.GetComponent<SpriteRenderer>().color = Color.white;
                    //GameplayControl.gameplayControl.GetComponent<SelectionManagement>().hoverPositionObject.SetActive(true);
                    transform.Find("VisualAbilities").Find("ExtraVisualAreas").GetChild(0).gameObject.SetActive(false);
                    transform.Find("VisualAbilities").Find("ExtraVisualAreas").GetChild(1).gameObject.SetActive(false);
                    transform.Find("VisualAbilities").Find("ExtraVisualAreas").GetChild(2).gameObject.SetActive(false);
                    //disable visual areas
                }
            } else
            {
                //GameplayControl.gameplayControl.GetComponent<SelectionManagement>().hoverPositionObject.GetComponent<SpriteRenderer>().color = Color.white;
                //GameplayControl.gameplayControl.GetComponent<SelectionManagement>().hoverPositionObject.SetActive(true);
                transform.Find("VisualAbilities").Find("ExtraVisualAreas").GetChild(0).gameObject.SetActive(false);
                transform.Find("VisualAbilities").Find("ExtraVisualAreas").GetChild(1).gameObject.SetActive(false);
                transform.Find("VisualAbilities").Find("ExtraVisualAreas").GetChild(2).gameObject.SetActive(false);
                //disable visual areass
            }
        }
    }
}
