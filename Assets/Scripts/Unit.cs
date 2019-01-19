using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    
    public bool[,] movementArea = new bool[17, 17];
    public bool movementWallCrossable;
    public bool movementAllyCrossable;
    public bool movementEnemyCrossable;

    public enum MovementType {Straight, Autodirect, Jump, Teleport}

    public int[,] damageArea = new int[17, 17];
    public bool damageWallCrossable;
    public bool damageAllyCrossable;
    public bool damageEnemyCrossable;


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

    }

    public void attack(Vector3 targetPosition) {
        //Physics2D.Raycast()

        /*if () {

        }*/
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
