using CustomMath;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Vec3))]
public class Vec3Drawer : PropertyDrawer
{
    private const float LabelWidth = 15f;
    private const float OutsideMargin = 5f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Begin property field
        EditorGUI.BeginProperty(position, label, property);

        // Get the property fields
        SerializedProperty xProp = property.FindPropertyRelative("x");
        SerializedProperty yProp = property.FindPropertyRelative("y");
        SerializedProperty zProp = property.FindPropertyRelative("z");

        // Create a rect for the label
        Rect labelRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, position.height);

        // Create rects for the fields
        float fieldWidth = (position.width - EditorGUIUtility.labelWidth - 3 * LabelWidth - 3 * OutsideMargin) / 3;
        
        Rect xLabelRect = new Rect(position.x + EditorGUIUtility.labelWidth + OutsideMargin, position.y, LabelWidth, position.height);
        Rect xRect = new Rect(xLabelRect.x + LabelWidth, position.y, fieldWidth, position.height);
        
        Rect yLabelRect = new Rect(xRect.x + fieldWidth + OutsideMargin, position.y, LabelWidth, position.height);
        Rect yRect = new Rect(yLabelRect.x + LabelWidth, position.y, fieldWidth, position.height);
        
        Rect zLabelRect = new Rect(yRect.x + fieldWidth + OutsideMargin, position.y, LabelWidth, position.height);
        Rect zRect = new Rect(zLabelRect.x + LabelWidth, position.y, fieldWidth, position.height);

        // Draw the label
        EditorGUI.LabelField(labelRect, label);

        // Draw the fields with labels
        EditorGUI.LabelField(xLabelRect, "X");
        EditorGUI.PropertyField(xRect, xProp, GUIContent.none);
        EditorGUI.LabelField(yLabelRect, "Y");
        EditorGUI.PropertyField(yRect, yProp, GUIContent.none);
        EditorGUI.LabelField(zLabelRect, "Z");
        EditorGUI.PropertyField(zRect, zProp, GUIContent.none);

        // End property field
        EditorGUI.EndProperty();
    }
}
