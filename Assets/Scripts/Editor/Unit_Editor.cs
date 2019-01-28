using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Unit))] [CanEditMultipleObjects]
public class Unit_Editor : Editor
{
    private void OnEnable()
    {
        
    }

    public override void OnInspectorGUI() {
        GUIStyle areaStyle = new GUIStyle();
        areaStyle.fontStyle = FontStyle.Bold;
        areaStyle.alignment = TextAnchor.MiddleCenter;

        EditorGUILayout.LabelField("Movement Area", areaStyle);

        EditorGUILayout.BeginHorizontal();

        for (int x = 0; x < (target as Unit).movementArea.GetLength(0) + 1; x++)
        {
            EditorGUILayout.BeginVertical();

            GUIStyle colStyle = new GUIStyle();
            colStyle.fontSize = 10;

            if (x > 0)
            {
                EditorGUILayout.LabelField((x - 1).ToString(), colStyle, GUILayout.Width(5));
            }
            else
            {
                EditorGUILayout.LabelField(string.Empty, colStyle, GUILayout.Width(5));
            }

            for (int y = 1; y < (target as Unit).movementArea.GetLength(1) + 1; y++)
            {
                if (x > 0)
                {
                    if (
                        /*x != 9 || (x == 9 && y != 9)*/
                        ((target as Unit).movementArea.GetLength(0) % 2 == 0 || (target as Unit).movementArea.GetLength(1) % 2 == 0) ||
                        (((target as Unit).movementArea.GetLength(0) % 2 != 0 && (target as Unit).movementArea.GetLength(1) % 2 != 0) &&
                        (x != (Mathf.CeilToInt((target as Unit).movementArea.GetLength(0) / 2) + 1) || (x == (Mathf.CeilToInt((target as Unit).movementArea.GetLength(0) / 2) + 1) && y != (Mathf.CeilToInt((target as Unit).movementArea.GetLength(1) / 2) + 1))))
                    ) {
                        if (((Unit)target).movementArea[x - 1, y - 1]) {
                            GUI.color = new Color(0, 1, 0);
                        }
                        
                        ((Unit)target).movementArea[x - 1, y - 1] = EditorGUILayout.Toggle(((Unit)target).movementArea[x - 1, y - 1]/*, tileStyle*/);

                        GUI.color = Color.white;
                    }
                    else
                    {
                        GUI.enabled = false;

                        GUI.color = new Color(0, 0, 1);
                        EditorGUILayout.Toggle(((Unit)target).movementArea[x - 1, y - 1]);
                        GUI.color = Color.white;

                        GUI.enabled = true;
                    }
                }
                else if (x == 0)
                {
                    GUIStyle rowStyle = new GUIStyle();
                    rowStyle.fontSize = 10;
                    rowStyle.alignment = TextAnchor.MiddleRight;

                    EditorGUILayout.LabelField((y - 1).ToString(), rowStyle, GUILayout.Width(10));
                }
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();

        (target as Unit).movementAreaDimensions = EditorGUILayout.Vector2Field("Dimensions", (target as Unit).movementAreaDimensions);


        GUI.color = Color.red;
        if (GUILayout.Button("Clear")) {
            (target as Unit).movementArea = new bool[Mathf.RoundToInt((target as Unit).movementAreaDimensions.x), Mathf.RoundToInt((target as Unit).movementAreaDimensions.y)];
        }
        GUI.color = Color.white;

        EditorGUILayout.LabelField("");
        (target as Unit).movementWallCrossable = EditorGUILayout.ToggleLeft("Can move across walls?", (target as Unit).movementWallCrossable);
        (target as Unit).movementAllyCrossable = EditorGUILayout.ToggleLeft("Can move across allies?", (target as Unit).movementAllyCrossable);
        (target as Unit).movementEnemyCrossable = EditorGUILayout.ToggleLeft("Can move across enemies?", (target as Unit).movementEnemyCrossable);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save Movement Area"))
        {
            if ((target as Unit).transform.Find("VisualAbilities").Find("VisualAreas").GetChild(0).childCount > 0)
            {
                foreach (Transform spot in (target as Unit).transform.Find("VisualAbilities").Find("VisualAreas").GetChild(0).gameObject.GetComponentsInDirectChildren<Transform>())
                {

                    if (spot.gameObject.tag == "AbilitySpot")
                    {
                        Undo.DestroyObjectImmediate(spot.gameObject);
                    }
                }
            } if ((target as Unit).transform.Find("VisualAbilities").Find("ExtraVisualAreas").GetChild(0).childCount > 0)
            {
                foreach (Transform spot in (target as Unit).transform.Find("VisualAbilities").Find("ExtraVisualAreas").GetChild(0).gameObject.GetComponentsInDirectChildren<Transform>())
                {

                    if (spot.gameObject.tag == "AbilitySpot")
                    {
                        Undo.DestroyObjectImmediate(spot.gameObject);
                    }
                }
            }

            List<Vector3> areas = GameplayControl.convert2DtoVector3((target as Unit).movementArea);

            (target as Unit).movementAreaListed = areas;
            (target as Unit).savedMovementAreaDimensions = new Vector2((target as Unit).movementArea.GetLength(0), (target as Unit).movementArea.GetLength(1));

            //visualizes
            foreach (Vector3 spot in areas)
            {
                GameObject movementSpotPrefab = PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/AbilitySpots/MovementSpot")) as GameObject;

                movementSpotPrefab.transform.SetParent((target as Unit).transform.Find("VisualAbilities").Find("VisualAreas").GetChild(0));
                movementSpotPrefab.transform.position = (target as Unit).transform.position + spot;
                movementSpotPrefab.transform.GetChild(0).right = movementSpotPrefab.transform.position - (target as Unit).transform.position;


                (target as Unit).transform.Find("VisualAbilities").Find("ExtraVisualAreas").GetChild(0).position = (target as Unit).transform.position;
                GameObject extraMovementSpotPrefab = PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/AbilitySpots/MovementSpot")) as GameObject;

                extraMovementSpotPrefab.transform.SetParent((target as Unit).transform.Find("VisualAbilities").Find("ExtraVisualAreas").GetChild(0));
                extraMovementSpotPrefab.transform.position = (target as Unit).transform.position + spot;
                extraMovementSpotPrefab.transform.GetChild(0).right = movementSpotPrefab.transform.position - (target as Unit).transform.position;

                extraMovementSpotPrefab.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
            }
        } if (GUILayout.Button("Load Movement List")) {
            (target as Unit).movementArea = new bool[Mathf.RoundToInt((target as Unit).savedMovementAreaDimensions.x), Mathf.RoundToInt((target as Unit).savedMovementAreaDimensions.y)];

            (target as Unit).movementArea = GameplayControl.listTo2DArray((target as Unit).movementAreaListed, new Vector2((target as Unit).savedMovementAreaDimensions.x, (target as Unit).savedMovementAreaDimensions.y));
        }
        EditorGUILayout.EndHorizontal();

        

        EditorGUILayout.LabelField("Damage Area", areaStyle);

        EditorGUILayout.BeginHorizontal();

        for (int x = 0; x < (target as Unit).damageArea.GetLength(0) + 1; x++)
        {
            EditorGUILayout.BeginVertical();

            GUIStyle colStyle = new GUIStyle();
            colStyle.fontSize = 10;

            if (x > 0)
            {
                EditorGUILayout.LabelField((x - 1).ToString(), colStyle, GUILayout.Width(5));
            }
            else
            {
                EditorGUILayout.LabelField(string.Empty, colStyle, GUILayout.Width(5));
            }

            GUILayoutOption[] verticalLayout = { GUILayout.Width(20)};

            for (int y = 1; y < (target as Unit).damageArea.GetLength(1) + 1; y++)
            {
                if (x > 0)
                {
                    if (
                        //x != 9 || (x == 9 && y != 9)
                        ((target as Unit).damageArea.GetLength(0) % 2 == 0 || (target as Unit).damageArea.GetLength(1) % 2 == 0) ||
                        (((target as Unit).damageArea.GetLength(0) % 2 != 0 && (target as Unit).damageArea.GetLength(1) % 2 != 0) &&
                        (x != (Mathf.CeilToInt((target as Unit).damageArea.GetLength(0) / 2) + 1) || (x == (Mathf.CeilToInt((target as Unit).damageArea.GetLength(0) / 2) + 1) && y != (Mathf.CeilToInt((target as Unit).damageArea.GetLength(1) / 2) + 1))))
                    ) {
                        if (((Unit)target).damageArea[x - 1, y - 1] > 0)
                        {
                            GUI.backgroundColor = Color.red;
                        }

                        ((Unit)target).damageArea[x - 1, y - 1] = EditorGUILayout.IntField(((Unit)target).damageArea[x - 1, y - 1]/*, tileStyle*/, verticalLayout);

                        GUI.backgroundColor = Color.white;
                    }
                    else
                    {
                        GUI.enabled = false;
                        GUI.backgroundColor = Color.blue;

                        EditorGUILayout.IntField(((Unit)target).damageArea[x - 1, y - 1], verticalLayout);
                        
                        GUI.enabled = true;
                        GUI.backgroundColor = Color.white;
                    }
                }
                else if (x == 0)
                {
                    GUIStyle rowStyle = new GUIStyle();
                    rowStyle.fontSize = 10;
                    rowStyle.alignment = TextAnchor.MiddleRight;

                    EditorGUILayout.LabelField((y - 1).ToString(), rowStyle, verticalLayout);
                }
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();

        (target as Unit).damageAreaDimensions = EditorGUILayout.Vector2Field("Dimensions", (target as Unit).damageAreaDimensions);

        GUI.color = Color.red;
        if (GUILayout.Button("Clear"))
        {
            (target as Unit).damageArea = new int[Mathf.RoundToInt((target as Unit).damageAreaDimensions.x), Mathf.RoundToInt((target as Unit).damageAreaDimensions.y)];
        }
        GUI.color = Color.white;

        EditorGUILayout.LabelField("");
        (target as Unit).damageWallCrossable = EditorGUILayout.ToggleLeft("Can attack across walls?", (target as Unit).damageWallCrossable);
        (target as Unit).damageAllyCrossable = EditorGUILayout.ToggleLeft("Can attack across allies?", (target as Unit).damageAllyCrossable);
        (target as Unit).damageEnemyCrossable = EditorGUILayout.ToggleLeft("Can attack across enemies?", (target as Unit).damageEnemyCrossable);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save Damage Area")) {
            if ((target as Unit).transform.Find("VisualAbilities").Find("VisualAreas").GetChild(1).childCount > 0)
            {
                foreach (Transform spot in (target as Unit).transform.Find("VisualAbilities").Find("VisualAreas").GetChild(1).GetComponentsInChildren<Transform>())
                {

                    if (spot.gameObject.tag == "AbilitySpot")
                    {
                        Undo.DestroyObjectImmediate(spot.gameObject);
                    }
                }
            } if ((target as Unit).transform.Find("VisualAbilities").Find("ExtraVisualAreas").GetChild(1).childCount > 0)
            {
                foreach (Transform spot in (target as Unit).transform.Find("VisualAbilities").Find("ExtraVisualAreas").GetChild(1).GetComponentsInChildren<Transform>())
                {

                    if (spot.gameObject.tag == "AbilitySpot")
                    {
                        Undo.DestroyObjectImmediate(spot.gameObject);
                    }
                }
            }

            List<Vector3> areas = GameplayControl.convert2DtoVector3((target as Unit).damageArea);
            List<int> damageList = GameplayControl.damageAreaToDamageList((target as Unit).damageArea);

            (target as Unit).damageAreaListed = areas;
            (target as Unit).damageListed = damageList;
            (target as Unit).savedDamageAreaDimensions = new Vector2((target as Unit).damageArea.GetLength(0), (target as Unit).damageArea.GetLength(1));

            for (int index = 0; index < ((areas.Count + damageList.Count) / 2); index++) {
                GameObject damageSpotPrefab = PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/AbilitySpots/DamageSpot")) as GameObject;

                damageSpotPrefab.transform.SetParent((target as Unit).transform.Find("VisualAbilities").Find("VisualAreas").GetChild(1));
                damageSpotPrefab.transform.position = (target as Unit).transform.position + areas[index];
                //add damage number


                (target as Unit).transform.Find("VisualAbilities").Find("ExtraVisualAreas").GetChild(1).position = (target as Unit).transform.position;
                GameObject extraDamageSpotPrefab = PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/AbilitySpots/DamageSpot")) as GameObject;

                extraDamageSpotPrefab.transform.SetParent((target as Unit).transform.Find("VisualAbilities").Find("ExtraVisualAreas").GetChild(1));
                extraDamageSpotPrefab.transform.position = (target as Unit).transform.position + (target as Unit).damageAreaListed[index];

                extraDamageSpotPrefab.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
            }
        } if (GUILayout.Button("Load Damage List")) {
            (target as Unit).damageArea = new int[Mathf.RoundToInt((target as Unit).savedDamageAreaDimensions.x), Mathf.RoundToInt((target as Unit).savedDamageAreaDimensions.y)];

            (target as Unit).damageArea = GameplayControl.listTo2DArray((target as Unit).damageAreaListed, (target as Unit).damageListed, new Vector2((target as Unit).savedDamageAreaDimensions.x, (target as Unit).savedDamageAreaDimensions.y));
        }
        EditorGUILayout.EndHorizontal();



        EditorGUILayout.LabelField("");
        (target as Unit).AI = EditorGUILayout.BeginToggleGroup("Is this an AI?", (target as Unit).AI);

        (target as Unit).defensive = EditorGUILayout.Slider("Defensive/Offensive", (target as Unit).defensive, 0f, 100f);
        (target as Unit).retreative = EditorGUILayout.Slider("Retreative/Dodgitive", (target as Unit).retreative, 0f, 100f);
        (target as Unit).lowAggressive = EditorGUILayout.Slider("Low/High health aggresivness", (target as Unit).lowAggressive, 0f, 100f);

        (target as Unit).defenseInfluence = EditorGUILayout.FloatField("Defence Influence", (target as Unit).defenseInfluence);
        (target as Unit).retreatInfluence = EditorGUILayout.FloatField("Retreat Influence", (target as Unit).retreatInfluence);

        EditorGUILayout.EndToggleGroup();

        //DrawDefaultInspector();

        

        

        /*if (serializedObject.FindProperty("defensive").floatValue) {

        }*/
    }
}
