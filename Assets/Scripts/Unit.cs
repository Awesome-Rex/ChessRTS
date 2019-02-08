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

    [SerializeField]
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

    [SerializeField]
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

    [SerializeField]
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

    [SerializeField]
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
        Game.currentGame.findSide(GameplayControl.gameplayControl.currentTurn).currentOrbs -= 1;

        if (GetComponent<Selectable>().selected)
        {
            GameplayControl.gameplayControl.GetComponent<SelectionManagement>().relocateSelected(targetPosition);
        }
        transform.position = targetPosition;

        if (Selectable_Comp.selected)
        {
            visualizeMovementArea();
        }
    } public void attack(Vector3 targetPosition) {
        Health damageTarget = Physics2D.Raycast(targetPosition, Vector3.zero, 0f, ~LayerMask.NameToLayer("Object")).collider.GetComponent<Health>() != null ? Physics2D.Raycast(targetPosition, Vector3.zero, 0f, ~LayerMask.NameToLayer("Object")).collider.GetComponent<Health>() : null;

        if (damageTarget != null) {
            Game.currentGame.findSide(GameplayControl.gameplayControl.currentTurn).currentOrbs -= 1;

            damageTarget.takeDamage(damageListed[damageAreaListed.IndexOf(targetPosition - transform.position)]);

            if (Selectable_Comp.selected) {
                visualizeDamageArea();
            }
        }
    }

    public bool checkMovable(Vector3 targetPosition) {
        foreach (Vector3 spot in Matter_Comp.matterAreaListed)
        {
            if (GameplayControl.objectInSpot(targetPosition + spot, gameObject))
            {
                return false;
            }
        }


        RaycastHit2D[] objectCasts = Physics2D.RaycastAll(transform.position, targetPosition - transform.position, Vector3.Distance(transform.position, targetPosition), ~LayerMask.NameToLayer("Object"));

        if (!movementAllyCrossable) {
            foreach (RaycastHit2D objectCast in objectCasts)
            {
                if (objectCast.collider.gameObject != gameObject && objectCast.collider.GetComponent<SideDefine>() != null && objectCast.collider.GetComponent<SideDefine>().side != Side.Nothing)
                {
                    bool isAlly = false;
                    
                    foreach (string castTag in objectCast.collider.GetComponent<SideDefine>().tags)
                    {
                        if (GetComponent<SideDefine>().allies.Contains(castTag))
                        {
                            isAlly = true;
                        }
                    }

                    if (objectCast.collider.GetComponent<SideDefine>().side == GetComponent<SideDefine>().side || isAlly)
                    {
                        return false;
                    }
                }
            }
        }
        if (!movementEnemyCrossable)
        {
            foreach (RaycastHit2D objectCast in objectCasts)
            {
                if (objectCast.collider.gameObject != gameObject && objectCast.collider.GetComponent<SideDefine>() != null && objectCast.collider.GetComponent<SideDefine>().side != Side.Nothing)
                {
                    bool isEnemy = false;

                    foreach (string castTag in objectCast.collider.GetComponent<SideDefine>().tags)
                    {
                        if (GetComponent<SideDefine>().enemies.Contains(castTag))
                        {
                            isEnemy = true;
                        }
                    }

                    if (objectCast.collider.GetComponent<SideDefine>().side != GetComponent<SideDefine>().side || isEnemy)
                    {
                        return false;
                    }
                }
            }
        }
        if (!movementWallCrossable)
        {
            foreach (RaycastHit2D objectCast in objectCasts)
            {
                if (objectCast.collider.gameObject != gameObject) {
                    /*bool isAlly = false;
                    bool isEnemy = false;

                    if (objectCast.collider.GetComponent<SideDefine>() != null && objectCast.collider.GetComponent<SideDefine>().side == Side.Nothing) {
                        foreach (string castTag in objectCast.collider.GetComponent<SideDefine>().tags)
                        {
                            if (GetComponent<SideDefine>().allies.Contains(castTag))
                            {
                                isAlly = true;
                            }
                            if (GetComponent<SideDefine>().enemies.Contains(castTag))
                            {
                                isEnemy = true;
                            }
                        }
                    }*/

                    if (objectCast.collider.GetComponent<SideDefine>() == null || (objectCast.collider.GetComponent<SideDefine>() != null && objectCast.collider.GetComponent<SideDefine>().side == Side.Nothing/* && !isAlly && !isEnemy*/))
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }
    public bool checkDamagable(Vector3 targetPosition) {
        RaycastHit2D healthCast = Physics2D.Raycast(targetPosition, Vector3.zero, 0f, ~LayerMask.NameToLayer("Object"));

        if (GameplayControl.objectInSpot(targetPosition, gameObject) && healthCast.collider.GetComponent<Health>() != null)
        {
            if (healthCast.collider.GetComponent<SideDefine>().side != GetComponent<SideDefine>().side || healthCast.collider.GetComponent<SideDefine>().side == Side.Nothing) {

            } else if (healthCast.collider.GetComponent<SideDefine>().side == GetComponent<SideDefine>().side)
            {
                foreach (string castTag in healthCast.collider.GetComponent<SideDefine>().tags)
                {
                    if (GetComponent<SideDefine>().enemies.Contains(castTag))
                    {
                        
                    } else
                    {
                        return false;
                    }
                }
            }
        } else {
            return false;
        }



        RaycastHit2D[] objectCasts = Physics2D.RaycastAll(transform.position, targetPosition - transform.position, Vector3.Distance(transform.position, targetPosition), ~LayerMask.NameToLayer("Object"));

        if (!damageAllyCrossable)
        {
            foreach (RaycastHit2D objectCast in objectCasts)
            {
                if (objectCast.collider.gameObject != gameObject && objectCast.collider.gameObject != healthCast.collider.gameObject && objectCast.collider.GetComponent<SideDefine>() != null && objectCast.collider.GetComponent<SideDefine>().side != Side.Nothing)
                {
                    bool isAlly = false;

                    foreach (string castTag in objectCast.collider.GetComponent<SideDefine>().tags)
                    {
                        if (GetComponent<SideDefine>().allies.Contains(castTag))
                        {
                            isAlly = true;
                        }
                    }

                    if (objectCast.collider.GetComponent<SideDefine>().side == GetComponent<SideDefine>().side || isAlly)
                    {
                        return false;
                    }
                }
            }
        }
        if (!damageEnemyCrossable)
        {
            foreach (RaycastHit2D objectCast in objectCasts)
            {
                if (objectCast.collider.gameObject != gameObject && objectCast.collider.gameObject != healthCast.collider.gameObject && objectCast.collider.GetComponent<SideDefine>() != null && objectCast.collider.GetComponent<SideDefine>().side != Side.Nothing)
                {
                    bool isEnemy = false;

                    foreach (string castTag in objectCast.collider.GetComponent<SideDefine>().tags)
                    {
                        if (GetComponent<SideDefine>().enemies.Contains(castTag))
                        {
                            isEnemy = true;
                        }
                    }

                    if (objectCast.collider.GetComponent<SideDefine>().side != GetComponent<SideDefine>().side || isEnemy)
                    {
                        return false;
                    }
                }
            }
        }
        if (!damageWallCrossable)
        {
            foreach (RaycastHit2D objectCast in objectCasts)
            {
                if (objectCast.collider.gameObject != gameObject && objectCast.collider.gameObject != healthCast.collider.gameObject)
                {
                    if (objectCast.collider.GetComponent<SideDefine>() == null || (objectCast.collider.GetComponent<SideDefine>() != null && objectCast.collider.GetComponent<SideDefine>().side == Side.Nothing/* && !isAlly && !isEnemy*/))
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    public void onSelect()
    {
        if (GameplayControl.gameplayControl.visualUnitAbility == GameplayControl.VisualUnitAbility.Movement) {
            visualizeMovementArea();
            //show movable/unmovable tiles
        } else if (GameplayControl.gameplayControl.visualUnitAbility == GameplayControl.VisualUnitAbility.Damage) {
            visualizeDamageArea();
            //same here
        } else if (GameplayControl.gameplayControl.visualUnitAbility == GameplayControl.VisualUnitAbility.Nothing) {
            transform.Find("VisualAbilities").Find("VisualAreas").GetChild(0).gameObject.SetActive(false);
            transform.Find("VisualAbilities").Find("VisualAreas").GetChild(1).gameObject.SetActive(false);
        }
    }
    public void onDeselect() {
        transform.Find("VisualAbilities").Find("VisualAreas").GetChild(0).gameObject.SetActive(false);
        transform.Find("VisualAbilities").Find("VisualAreas").GetChild(1).gameObject.SetActive(false);
    }


    public void visualizeMovementArea()
    {
        transform.Find("VisualAbilities").Find("VisualAreas").GetChild(0).gameObject.SetActive(true);

        foreach (Transform spot in transform.Find("VisualAbilities").Find("VisualAreas").GetChild(0).gameObject.GetComponentsInDirectChildren<Transform>())
        {
            if (checkMovable(spot.position))
            {
                spot.GetComponent<SpriteRenderer>().color = Color.white;
            }
            else
            {
                spot.GetComponent<SpriteRenderer>().color = Color.grey;
            }
        }
    }

    public void visualizeDamageArea()
    {
        transform.Find("VisualAbilities").Find("VisualAreas").GetChild(1).gameObject.SetActive(true);

        foreach (Transform spot in transform.Find("VisualAbilities").Find("VisualAreas").GetChild(1).gameObject.GetComponentsInDirectChildren<Transform>())
        {
            if (checkDamagable(spot.position))
            {
                spot.GetComponent<SpriteRenderer>().color = Color.white;
            }
            else
            {
                spot.GetComponent<SpriteRenderer>().color = Color.grey;
            }
        }
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
                if (GameplayControl.gameplayControl.visualUnitAbility == GameplayControl.VisualUnitAbility.Movement)
                {
                    Vector3 inputPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    if (Matter_Comp.savedMatterDimensions.x % 2f != 0 && Matter_Comp.savedMatterDimensions.y % 2f != 0){
                        inputPosition = new Vector3(Mathf.Round(inputPosition.x), Mathf.Round(inputPosition.y), 0f);
                    } else if (Matter_Comp.savedMatterDimensions.x % 2 == 0 || Matter_Comp.savedMatterDimensions.y % 2 == 0){
                        if (Matter_Comp.savedMatterDimensions.x % 2 == 0 && Matter_Comp.savedMatterDimensions.y % 2 == 0){
                            inputPosition = new Vector3(Mathf.Sign(inputPosition.x) * (Mathf.Abs((int)inputPosition.x) + 0.5f), Mathf.Sign(inputPosition.y) * (Mathf.Abs((int)inputPosition.y) + 0.5f), 0f);
                        }
                        else if (Matter_Comp.savedMatterDimensions.x % 2 != 0 && Matter_Comp.savedMatterDimensions.y % 2 == 0){
                            inputPosition = new Vector3(Mathf.Round(inputPosition.x), Mathf.Sign(inputPosition.y) * (Mathf.Abs((int)inputPosition.y) + 0.5f), 0f);
                        }
                        else if (Matter_Comp.savedMatterDimensions.x % 2 == 0 && Matter_Comp.savedMatterDimensions.y % 2 != 0){
                            inputPosition = new Vector3(Mathf.Sign(inputPosition.x) * (Mathf.Abs((int)inputPosition.x) + 0.5f), Mathf.Round(inputPosition.y), 0f);
                        }
                    }

                    if (GameplayControl.containedInArea(inputPosition, movementAreaListed, transform.position)) {
                        transform.Find("VisualAbilities").Find("VisualPointers").GetChild(0).gameObject.SetActive(true);
                        transform.Find("VisualAbilities").Find("VisualPointers").GetChild(0).right = inputPosition - transform.position;
                        transform.Find("VisualAbilities").Find("VisualPointers").GetChild(0).GetComponent<SpriteRenderer>().size = new Vector2(Vector3.Distance(transform.position, inputPosition), 0.125f);

                        transform.Find("VisualAbilities").Find("VisualPointers").GetChild(1).gameObject.SetActive(false);
                    } else
                    {
                        transform.Find("VisualAbilities").Find("VisualPointers").GetChild(0).gameObject.SetActive(false);
                        transform.Find("VisualAbilities").Find("VisualPointers").GetChild(1).gameObject.SetActive(false);
                    }
                } else if (GameplayControl.gameplayControl.visualUnitAbility == GameplayControl.VisualUnitAbility.Damage)
                {
                    Vector3 inputPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    inputPosition = new Vector3(Mathf.Round(inputPosition.x), Mathf.Round(inputPosition.y), 0f);

                    if (GameplayControl.containedInArea(inputPosition, damageAreaListed, transform.position))
                    {
                        transform.Find("VisualAbilities").Find("VisualPointers").GetChild(1).gameObject.SetActive(true);
                        transform.Find("VisualAbilities").Find("VisualPointers").GetChild(1).right = inputPosition - transform.position;
                        transform.Find("VisualAbilities").Find("VisualPointers").GetChild(1).GetComponent<SpriteRenderer>().size = new Vector2(Vector3.Distance(transform.position, inputPosition), 0.125f);

                        transform.Find("VisualAbilities").Find("VisualPointers").GetChild(0).gameObject.SetActive(false);
                    } else
                    {
                        transform.Find("VisualAbilities").Find("VisualPointers").GetChild(0).gameObject.SetActive(false);
                        transform.Find("VisualAbilities").Find("VisualPointers").GetChild(1).gameObject.SetActive(false);
                    }
                } else if (GameplayControl.gameplayControl.visualUnitAbility == GameplayControl.VisualUnitAbility.Nothing)
                {
                    transform.Find("VisualAbilities").Find("VisualPointers").GetChild(0).gameObject.SetActive(false);
                    transform.Find("VisualAbilities").Find("VisualPointers").GetChild(1).gameObject.SetActive(false);
                }

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
                        //Debug.Log("Supposed to be disabling hover object!");

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

                transform.Find("VisualAbilities").Find("VisualPointers").GetChild(0).gameObject.SetActive(false);
                transform.Find("VisualAbilities").Find("VisualPointers").GetChild(1).gameObject.SetActive(false);
                //disable visual areass
            }
        }
    }
}
