using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorControl : Editor
{
    /*[MenuItem("Game/Update unit areas listed")]
    public static void updateUnitAreas ()
    {
        foreach (Unit unit in FindObjectsOfType<Unit>())
        {
            foreach (Vector3 spot in unit.movementAreaListed)
            {
                unit.movementAreaListed_new.Add(new AbilitySpot(spot));
            }

            for (int i = 0; i < unit.damageAreaListed.Count; i++)
            {
                unit.damageAreaListed_new.Add(new AbilitySpot(unit.damageAreaListed[i], unit.damageListed[i]));
            }
        }
    }*/

    [MenuItem("Game/check updated areas")]
    public static void checkupdateUnitAreas()
    {
        foreach (Unit unit in FindObjectsOfType<Unit>())
        {
            Debug.Log(unit.movementAreaListed.Count);
            Debug.Log(unit.damageAreaListed.Count);
        }
    }

    /*[MenuItem("Game/GameObject Area Control/Reset Unit Movement")]
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
    }*/
}
