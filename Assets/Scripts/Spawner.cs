using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Spawnable
{
    public GameObject spawnObject;
    public float percentage;
}

public class Spawner : MonoBehaviour
{
    public bool[,] spawnArea = new bool[9, 9];
    public Vector2 spawnAreaDimensions = new Vector2(9, 9);

    public List<Vector3> spawnAreaListed;
    public Vector2 savedSpawnAreaDimensions = new Vector2(9, 9);


    public List<Spawnable> spawnObjects = new List<Spawnable>();

    public int minObjects;
    public int maxObjects;

    public enum SpawnType
    {
        OnDestroy, OverTime
    }
    public SpawnType spawntype;


    public int turnsUntilSpawn;
    public bool destroyOnOverTimeSpawn;


    public void spawn() {
        List<Vector3> remainingAreas = new List<Vector3>();


        for (int i = Random.Range(minObjects, maxObjects + 1); i > 0; i--) {

        }
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
