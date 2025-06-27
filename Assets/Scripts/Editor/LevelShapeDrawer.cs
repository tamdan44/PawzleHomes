#if UNITY_EDITOR
using UnityEditor;
using UnityEngine; 

[CustomEditor(typeof(ShapeData))]
[CanEditMultipleObjects]
[System.Serializable]
public class LevelShapeDrawer : Editor
{
    public override void OnInspectorGUI()
    {
        // Update the serialized object
        serializedObject.Update();

        // Get serialized properties
        SerializedProperty columnsProp = serializedObject.FindProperty("columns");
        SerializedProperty rowsProp = serializedObject.FindProperty("rows");
        SerializedProperty trianglesProp = serializedObject.FindProperty("triangles");

        // Store old values to detect changes
        int oldColumns = columnsProp.intValue;
        int oldRows = rowsProp.intValue;

        // Draw columns and rows fields
        EditorGUILayout.PropertyField(columnsProp);
        EditorGUILayout.PropertyField(rowsProp);
        
        int columns = columnsProp.intValue;
        int rows = rowsProp.intValue;
        int expectedLength = columns * rows * 4;

        // Check if dimensions changed
        bool dimensionsChanged = (oldColumns != columns || oldRows != rows);

        // Resize triangles array if needed
        if (trianglesProp.arraySize != expectedLength)
        {
            trianglesProp.arraySize = expectedLength;
        }

        EditorGUILayout.LabelField("Triangles:");

        bool trianglesChanged = false;

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                for (int t = 0; t < 4; t++)
                {
                    int index = (r * columns + c) * 4 + t;
                    if (index < trianglesProp.arraySize)
                    {
                        SerializedProperty triangleElement = trianglesProp.GetArrayElementAtIndex(index);
                        string label = $"Row {-r}, Col {c}, Tri {t}";

                        EditorGUI.BeginChangeCheck();
                        EditorGUILayout.PropertyField(triangleElement, new GUIContent(label));
                        if (EditorGUI.EndChangeCheck())
                        {
                            trianglesChanged = true;
                        }

                    }
                }
            }
        }

        // Apply changes to the serialized object
        if (serializedObject.ApplyModifiedProperties())
        {
            // If triangles were modified directly in editor, update the board
            if (trianglesChanged && !dimensionsChanged)
            {
                ShapeData shapeData = (ShapeData)target;
                shapeData.UpdateBoardFromTriangles();
            }
        }

    }
}
#endif