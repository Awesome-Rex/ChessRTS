using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorControl : Editor
{
    [MenuItem("Game/Update unit areas listed")]
    public static void updateUnitAreas ()
    {
        foreach (Unit unit in FindObjectsOfType<Unit>())
        {
            Undo.RegisterCompleteObjectUndo(unit, "movement Undo");
            unit.movementAreaListed.Clear();
            foreach (Vector3 spot in unit.movementAreaListed_deprecated)
            {
                unit.movementAreaListed.Add(new AbilitySpot(spot));
            }

            Undo.RegisterCompleteObjectUndo(unit, "damage Undo");
            unit.damageAreaListed.Clear();
            for (int i = 0; i < unit.damageAreaListed_deprecated.Count; i++)
            {
                unit.damageAreaListed.Add(new AbilitySpot(unit.damageAreaListed_deprecated[i], unit.damageListed_deprecated[i]));
            }
        }
    }

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
        foreach (Matter matter in FindObjectsOfType<Matter>())
        {
            Matter_Editor.loadMatterList(matter);
            Matter_Editor.saveMatterArea(matter);
        }
    }
}
