using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    
    public bool[,] movementArea = new bool[17, 17];
    public bool movementWallCrossable;
    public bool movementAllyCrossable;
    public bool movementEnemyCrossable;
    public List<Vector3> movementAreaListed = new List<Vector3>();

    public enum MovementType {Straight, Autodirect, Jump, Teleport}

    public int[,] damageArea = new int[17, 17];
    public bool damageWallCrossable;
    public bool damageAllyCrossable;
    public bool damageEnemyCrossable;
    public List<Vector3> damageAreaListed = new List<Vector3>();
    public List<int> damageListed = new List<int>();

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

    public void move(Vector3 targetPosition) {
        //animate transition

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
        return false;
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
        return false;
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
