using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorControl : Editor
{
    [MenuItem("Game/GameObject Area Control/Reset Unit Movement")]
    public static void resetUnitMovement ()
    {
        foreach (Unit unit in FindObjectsOfType<Unit>())
        {
            Unit_Editor.loadMovementList(unit);
            Unit_Editor.saveMovementArea(unit);
        }
    }

    [MenuItem("Game/GameObject Area Control/Reset Unit Damage")]
    public static void resetUnitDamage ()
    {
        foreach (Unit unit in FindObjectsOfType<Unit>())
        {
            Unit_Editor.loadDamageList(unit);
            Unit_Editor.saveDamageArea(unit);
        }
    }

    [MenuItem("Game/GameObject Area Control/Reset Matter")]
    public static void resetMatter ()
    {

    }
}
