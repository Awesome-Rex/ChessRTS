using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matter : MonoBehaviour
{
    public bool[,] matterArea = new bool[17, 17];
    public Vector2 matterDimensions = new Vector2(17, 17);


    public List<Vector3> matterAreaListed;
    public Vector2 savedMatterDimensions = new Vector2(17, 17);


    public float savedMinX;
    public float savedMaxX;

    public float savedMinY;
    public float savedMaxY;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
