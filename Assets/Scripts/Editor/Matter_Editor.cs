using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Matter))]
[CanEditMultipleObjects]

public class Matter_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        GUIStyle areaStyle = new GUIStyle();
        areaStyle.fontStyle = FontStyle.Bold;
        areaStyle.alignment = TextAnchor.MiddleCenter;

        EditorGUILayout.LabelField("Matter Area", areaStyle);

        EditorGUILayout.BeginHorizontal();

        for (int x = 0; x < (target as Matter).matterArea.GetLength(0) + 1; x++)
        {
            EditorGUILayout.BeginVertical();

            GUIStyle colStyle = new GUIStyle();
            colStyle.fontSize = 10;

            //label along x axis
            if (x > 0)
            {
                EditorGUILayout.LabelField((x - 1).ToString(), colStyle, GUILayout.Width(5));
            }
            else
            {
                EditorGUILayout.LabelField(string.Empty, colStyle, GUILayout.Width(5));
            }

            for (int y = 1; y < (target as Matter).matterArea.GetLength(1) + 1; y++)
            {
                if (x > 0)
                {
                    if (
                        ((target as Matter).matterArea.GetLength(0) % 2 == 0 || (target as Matter).matterArea.GetLength(1) % 2 == 0) ||
                        (((target as Matter).matterArea.GetLength(0) % 2 != 0 && (target as Matter).matterArea.GetLength(1) % 2 != 0) && 
                        (x != (Mathf.CeilToInt((target as Matter).matterArea.GetLength(0) / 2) + 1) || (x == (Mathf.CeilToInt((target as Matter).matterArea.GetLength(0) / 2) + 1) && y != (Mathf.CeilToInt((target as Matter).matterArea.GetLength(1) / 2) + 1))))
                    ) {
                        if (((Matter)target).matterArea[x - 1, y - 1])
                        {
                            GUI.color = new Color(0, 0, 1);
                        }

                        ((Matter)target).matterArea[x - 1, y - 1] = EditorGUILayout.Toggle(((Matter)target).matterArea[x - 1, y - 1]);

                        GUI.color = Color.white;
                    }
                    else
                    {
                        GUI.enabled = false;

                        GUI.color = new Color(0, 0, 1);

                        ((Matter)target).matterArea[x - 1, y - 1] = true;
                        EditorGUILayout.Toggle(((Matter)target).matterArea[x - 1, y - 1]);

                        GUI.color = Color.white;

                        GUI.enabled = true;
                    }
                }
                else if (x == 0)
                {
                    /// label along y axis
                    GUIStyle rowStyle = new GUIStyle();
                    rowStyle.fontSize = 10;
                    rowStyle.alignment = TextAnchor.MiddleRight;

                    EditorGUILayout.LabelField((y - 1).ToString(), rowStyle, GUILayout.Width(10));
                }
            }
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndHorizontal();

        (target as Matter).matterDimensions = EditorGUILayout.Vector2Field("Dimensions", (target as Matter).matterDimensions);



        GUI.color = Color.red;
        if (GUILayout.Button("Clear"))
        {
            (target as Matter).matterArea = new bool[Mathf.RoundToInt((target as Matter).matterDimensions.x), Mathf.RoundToInt((target as Matter).matterDimensions.y)];
        }
        GUI.color = Color.white;

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save Matter Area"))
        {
            if ((target as Matter).transform.Find("Colliders").GetChild(0).childCount > 0)
            {
                foreach (Transform spot in (target as Matter).transform.Find("Colliders").GetChild(0).GetComponentsInChildren<Transform>())
                {

                    if (spot.gameObject.GetInstanceID() != (target as Matter).transform.Find("Colliders").GetChild(0).gameObject.GetInstanceID())
                    {
                        Undo.DestroyObjectImmediate(spot.gameObject);
                    }
                }
            }

            List<Vector3> areas = GameplayControl.convert2DtoVector3((target as Matter).matterArea);

            (target as Matter).matterAreaListed = areas;
            (target as Matter).savedMatterDimensions = new Vector2((target as Matter).matterArea.GetLength(0), (target as Matter).matterArea.GetLength(1));

            //visualizes
            foreach (Vector3 spot in areas)
            {
                GameObject colliderSpotPrefab = PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/ObjectColliderTile")) as GameObject;

                colliderSpotPrefab.transform.SetParent((target as Matter).transform.Find("Colliders").GetChild(0));
                colliderSpotPrefab.transform.position = (target as Matter).transform.position + spot;
            }
        }
        if (GUILayout.Button("Load Matter List"))
        {
            (target as Matter).matterArea = new bool[Mathf.RoundToInt((target as Matter).savedMatterDimensions.x), Mathf.RoundToInt((target as Matter).savedMatterDimensions.y)];

            (target as Matter).matterArea = GameplayControl.listTo2DArray((target as Matter).matterAreaListed, new Vector2((target as Matter).matterArea.GetLength(0), (target as Matter).matterArea.GetLength(1)));
        }
        EditorGUILayout.EndHorizontal();
    }
}
