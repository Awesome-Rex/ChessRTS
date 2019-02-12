using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Matter)), CanEditMultipleObjects]

public class Matter_Editor : Editor
{
    protected static bool showDefaultInspector = false;

    public static void loadMatterList(Matter target)
    {
        (target as Matter).matterArea = new bool[Mathf.RoundToInt((target as Matter).savedMatterDimensions.x), Mathf.RoundToInt((target as Matter).savedMatterDimensions.y)];

        (target as Matter).matterArea = GameplayControl.listTo2DArray((target as Matter).matterAreaListed, new Vector2((target as Matter).savedMatterDimensions.x, (target as Matter).savedMatterDimensions.y));
    }
    public static void saveMatterArea (Matter target)
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
        if ((target as Matter).transform.Find("VisualAbilities").Find("ExtraVisualAreas").GetChild(2).childCount > 0)
        {
            foreach (Transform spot in (target as Matter).transform.Find("VisualAbilities").Find("ExtraVisualAreas").GetChild(2).GetComponentsInChildren<Transform>())
            {

                if (spot.gameObject.GetInstanceID() != (target as Matter).transform.Find("VisualAbilities").Find("ExtraVisualAreas").GetChild(2).gameObject.GetInstanceID())
                {
                    Undo.DestroyObjectImmediate(spot.gameObject);
                }
            }
        }

        float minX = 0f;
        float maxX = 0f;
        float minY = 0f;
        float maxY = 0f;

        List<AbilitySpot> areas = GameplayControl.convert2DtoVector3((target as Matter).matterArea, target as Matter);

        (target as Matter).matterAreaListed = areas;
        (target as Matter).savedMatterDimensions = new Vector2((target as Matter).matterArea.GetLength(0), (target as Matter).matterArea.GetLength(1));

        //visualizes
        foreach (AbilitySpot spot in areas)
        {
            if (spot.location.x < minX)
            {
                minX = spot.location.x;
            }
            else if (spot.location.x > maxX)
            {
                maxX = spot.location.x;
            }
            if (spot.location.y < minY)
            {
                minY = spot.location.y;
            }
            else if (spot.location.y > maxY)
            {
                maxY = spot.location.y;
            }

            GameObject colliderSpotPrefab = PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/ObjectColliderTile")) as GameObject;

            colliderSpotPrefab.transform.SetParent((target as Matter).transform.Find("Colliders").GetChild(0));
            colliderSpotPrefab.transform.position = (target as Matter).transform.position + spot.location;

            GameObject visualColliderSpotPrefab = PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/AbilitySpots/MatterSpot")) as GameObject;

            visualColliderSpotPrefab.transform.SetParent((target as Matter).transform.Find("VisualAbilities").Find("ExtraVisualAreas").GetChild(2));
            visualColliderSpotPrefab.transform.position = (target as Matter).transform.position + spot.location;
        }

        (target as Matter).savedMinX = minX;
        (target as Matter).savedMaxX = maxX;
        (target as Matter).savedMinY = minY;
        (target as Matter).savedMaxY = maxY;

        //canvas resizing

        (target as Matter).transform.Find("UI").Find("Canvas").GetComponent<RectTransform>().localPosition = new Vector3((target as Matter).savedMinX - 0.5f, (target as Matter).savedMaxY + 0.5f, 0f);
        (target as Matter).transform.Find("UI").Find("Canvas").GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Abs((target as Matter).savedMaxX - (target as Matter).savedMinX) + 1f, Mathf.Abs((target as Matter).savedMaxY - (target as Matter).savedMinY) + 1f) * 100f;
    }

    public override void OnInspectorGUI()
    {
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
            saveMatterArea(target as Matter);
        }
        if (GUILayout.Button("Load Matter List"))
        {
            loadMatterList(target as Matter);
        }
        EditorGUILayout.EndHorizontal();



        EditorGUILayout.LabelField(string.Empty);
        showDefaultInspector = EditorGUILayout.Foldout(showDefaultInspector, "Default Inspector");
        if (showDefaultInspector)
        {
            base.OnInspectorGUI();
        }
    }
}
