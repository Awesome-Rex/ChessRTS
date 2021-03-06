﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Selectable : MonoBehaviour
{
    public bool _selected;
    [SerializeField] public bool selected
    {
        get
        {
            return _selected;
        }
        set
        {
            _selected = value;

            if (value) {
                select();
            } else if (!value) {
                deselect();
            }
        }
    }

    public UnityEvent selectEvents;
    public UnityEvent deselectEvents;

    public void select() {
        selectEvents.Invoke();

        if (GetComponent<Unit>() != null) {
            GetComponent<Unit>().onSelect();
        }
    }

    public void deselect() {
        deselectEvents.Invoke();

        if (GetComponent<Unit>() != null) {
            GetComponent<Unit>().onDeselect();
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
