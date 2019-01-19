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

        for (int x = 0; x < 18; x++)
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

            for (int y = 1; y < 18; y++)
            {
                if (x > 0)
                {
                    if (x != 9 || (x == 9 && y != 9))
                    {
                        if (((Unit)target).movementArea[x - 1, y - 1]) {
                            GUI.color = new Color(0, 1, 0);
                        }

                        GUIStyle tileStyle = new GUIStyle();
                        /*tileStyle.margin.left = 0;
                        tileStyle.margin.right = 0;
                        tileStyle.margin.top = 0;
                        tileStyle.margin.bottom = 0;*/

                        
                        ((Unit)target).movementArea[x - 1, y - 1] = EditorGUILayout.Toggle(((Unit)target).movementArea[x - 1, y - 1]/*, tileStyle*/);

                        GUI.color = Color.white;
                    }
                    else
                    {
                        GUI.enabled = false;

                        /*GUIStyle originStyle = new GUIStyle();
                        originStyle.*/

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

        EditorGUILayout.LabelField("");
        (target as Unit).movementWallCrossable = EditorGUILayout.ToggleLeft("Can move across walls?", (target as Unit).movementWallCrossable);
        (target as Unit).movementAllyCrossable = EditorGUILayout.ToggleLeft("Can move across allies?", (target as Unit).movementAllyCrossable);
        (target as Unit).movementEnemyCrossable = EditorGUILayout.ToggleLeft("Can move across enemies?", (target as Unit).movementEnemyCrossable);

        EditorGUILayout.LabelField("Damage Area", areaStyle);

        EditorGUILayout.BeginHorizontal();

        for (int x = 0; x < 18; x++)
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

            GUILayoutOption[] verticalLayout = { GUILayout.Width(20)/*, GUILayout.Height(15)*/};

            for (int y = 1; y < 18; y++)
            {
                if (x > 0)
                {
                    if (x != 9 || (x == 9 && y != 9))
                    {
                        if (((Unit)target).damageArea[x - 1, y - 1] > 0)
                        {
                            GUI.color = new Color(1, 0, 0);
                        }

                        GUIStyle tileStyle = new GUIStyle();
                        tileStyle.alignment = TextAnchor.MiddleCenter;
                        tileStyle.fontSize = 10;
                        tileStyle.margin = new RectOffset(0, 0, 0, 0);

                        ((Unit)target).damageArea[x - 1, y - 1] = EditorGUILayout.IntField(((Unit)target).damageArea[x - 1, y - 1], tileStyle, verticalLayout);

                        GUI.color = Color.white;
                    }
                    else
                    {
                        GUI.enabled = false;
                        GUI.color = new Color(0, 0, 1);

                        GUIStyle originStyle = new GUIStyle();
                        originStyle.alignment = TextAnchor.MiddleCenter;
                        originStyle.fontSize = 10;
                        originStyle.margin = new RectOffset(0, 0, 0, 0);

                        GUI.color = new Color(1, 0, 0);
                        EditorGUILayout.IntField(((Unit)target).damageArea[x - 1, y - 1], originStyle, verticalLayout);
                        GUI.color = Color.white;

                        GUI.enabled = true;
                        GUI.color = new Color(1, 1, 1);
                    }
                }
                else if (x == 0)
                {
                    GUIStyle rowStyle = new GUIStyle();
                    rowStyle.fontSize = 10;
                    rowStyle.alignment = TextAnchor.MiddleRight;
                    rowStyle.margin = new RectOffset(5, 5, 0, 0);

                    EditorGUILayout.LabelField((y - 1).ToString(), rowStyle, verticalLayout);
                }
            }
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField("");
        (target as Unit).damageWallCrossable = EditorGUILayout.ToggleLeft("Can attack across walls?", (target as Unit).damageWallCrossable);
        (target as Unit).damageAllyCrossable = EditorGUILayout.ToggleLeft("Can attack across allies?", (target as Unit).damageAllyCrossable);
        (target as Unit).damageEnemyCrossable = EditorGUILayout.ToggleLeft("Can attack across enemies?", (target as Unit).damageEnemyCrossable);
        EditorGUILayout.LabelField("");

        {
            GUIStyle AIStyle = new GUIStyle();
            AIStyle.fontStyle = FontStyle.Bold;

            //(target as Unit).AI = EditorGUILayout.Toggle("Is this an AI?", (target as Unit).AI/*, AIStyle*/);
        }

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
