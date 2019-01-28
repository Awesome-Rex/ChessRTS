using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static List<T> GetComponentsRecursive<T>(this GameObject gameObject) where T : Component
    {
        int length = gameObject.transform.childCount;
        List<T> components = new List<T>(length + 1);
        T comp = gameObject.transform.GetComponent<T>();
        if (comp != null) components.Add(comp);
        for (int i = 0; i < length; i++)
        {
            comp = gameObject.transform.GetChild(i).GetComponent<T>();
            if (comp != null) components.Add(comp);
        }
        return components;
    }

    public static List<T> GetComponentsInDirectChildren<T>(this GameObject gameObject) where T : Component
    {
        int length = gameObject.transform.childCount;
        List<T> components = new List<T>(length);
        for (int i = 0; i < length; i++)
        {
            T comp = gameObject.transform.GetChild(i).GetComponent<T>();
            if (comp != null) components.Add(comp);
        }
        return components;
    }
}