﻿using System.Collections;
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

    public IEnumerator depth ()
    {
        while (true)
        {
            yield return null;

            if (Input.GetKey(KeyCode.R) && Camera.main.orthographicSize > 1.5f) {
                yield return StartCoroutine(zoomTo(Camera.main.orthographicSize - 1.5f));
            } else if (Input.GetKey(KeyCode.F) && Camera.main.orthographicSize < 21f) {
                yield return StartCoroutine(zoomTo(Camera.main.orthographicSize + 1.5f));
            }
        }
    }

    public IEnumerator zoomTo (float targetSize)
    {
        while (Camera.main.orthographicSize != targetSize)
        {
            yield return null;

            Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, targetSize, 20f * Time.deltaTime);
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(movement());
        StartCoroutine(depth());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            Camera.main.transform.rotation = Quaternion.Euler(Camera.main.transform.rotation.eulerAngles + new Vector3(0f, 0f, Time.deltaTime * 90f));
        } else if (Input.GetKey(KeyCode.E))
        {
            Camera.main.transform.rotation = Quaternion.Euler(Camera.main.transform.rotation.eulerAngles + new Vector3(0f, 0f, Time.deltaTime * -90f));
        }
    }
}
