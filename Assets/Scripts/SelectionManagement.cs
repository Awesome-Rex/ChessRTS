using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManagement : MonoBehaviour
{
    public GameObject targetPositionObject;
    public Vector3 targetPosition
    {
        get
        {
            return targetPositionObject.transform.position;
        }
        set
        {
            targetPositionObject.transform.position = value;
        }
    }

    public Selectable targetedObject;
    public bool objectSelected;

    public GameObject hoverPositionObject;
    public Vector3 hoverPosition
    {
        get
        {
            return hoverPositionObject.transform.position;
        }
        set
        {
            hoverPositionObject.transform.position = value;
        }
    }

    public Selectable hoveredObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 inputPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            inputPosition = new Vector3(Mathf.Round(inputPosition.x), Mathf.Round(inputPosition.y), 0f);

            targetPosition = inputPosition;

            if (targetedObject != null && inputPosition != targetedObject.transform.position)
            {
                targetedObject.selected = false;

            }

            Selectable targetedObjectCast = Physics2D.Raycast(inputPosition, Vector3.zero, 0f, LayerMask.NameToLayer("Object")).collider != null ? 
                Physics2D.Raycast(inputPosition, Vector3.zero, 0f, LayerMask.NameToLayer("Object")).collider.GetComponent<Selectable>() : 
                null;

            if (targetedObjectCast != null) {
                targetedObject = targetedObjectCast;

                targetedObjectCast.selected = !targetedObjectCast.selected;
                objectSelected = targetedObjectCast.selected;
            } else
            {
                targetedObject = null;
            }

            // display selected data
        }

        if (Input.GetAxis("Mouse X") != 0f || Input.GetAxis("Mouse Y") != 0f) {
            Vector3 inputPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            inputPosition = new Vector3(Mathf.Round(inputPosition.x), Mathf.Round(inputPosition.y), 0f);

            hoverPosition = inputPosition;

            //display selected hover data
        }
    }
}
