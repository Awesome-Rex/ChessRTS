using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matter : MonoBehaviour
{
    public bool[,] matterArea = new bool[17, 17];

    public List<Vector3> matterAreaListed;

    public void createCollider ()
    {
        if (transform.Find("Colliders").GetChild(0).childCount > 0)
        {
            foreach (Transform colliderChild in transform.Find("Colliders").GetChild(0).GetComponentInChildren<Transform>())
            {
                Destroy(colliderChild.gameObject);
            }
        }

        foreach (Vector3 matterSpot in matterAreaListed)
        {
            
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
