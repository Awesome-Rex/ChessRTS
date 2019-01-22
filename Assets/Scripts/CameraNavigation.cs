using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraNavigation : MonoBehaviour
{
    public IEnumerator movement ()
    {
        while (true) {
            yield return null;

            if (Input.GetKey(KeyCode.W)) {
                yield return StartCoroutine(moveTo(new Vector3(0f, 0.5f, 0f)));
            } else if (Input.GetKey(KeyCode.S)) {
                yield return StartCoroutine(moveTo(new Vector3(0f, -0.5f, 0f)));
            } else if (Input.GetKey(KeyCode.A)) {
                yield return StartCoroutine(moveTo(new Vector3(-0.5f, 0f, 0f)));
            } else if (Input.GetKey(KeyCode.D)) {
                yield return StartCoroutine(moveTo(new Vector3(0.5f, 0f, 0f)));
            }
        }
    }

    public IEnumerator moveTo (Vector3 addedPosition)
    {
        Vector3 targetPosition = transform.position + addedPosition;

        while (transform.position != targetPosition) {
            yield return null;

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, 20 * Time.deltaTime);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(movement());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
