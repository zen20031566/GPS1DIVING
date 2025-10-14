using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(RequiredAmountPair))]
public class RequiredAmountPairPropertyDrawer : PropertyDrawer
{
    private const float lineHeight = 18f;
    private const float verticalSpacing = 2f;

    //Static field to store the collectByItemTag value from the parent drawer
    public static bool CurrentCollectByItemTag { get; set; }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        //Draw foldout
        property.isExpanded = EditorGUI.Foldout(
            new Rect(position.x, position.y, position.width, lineHeight),
            property.isExpanded, label, true
        );

        if (property.isExpanded)
        {
            float currentY = position.y + lineHeight + verticalSpacing;

            if (CurrentCollectByItemTag)
            {
                // Show only ItemTag when collectByItemTag is true
                currentY = DrawProperty(property, "ItemTag", position, currentY);
            }
            else
            {
                // Show only Id when collectByItemTag is false
                currentY = DrawProperty(property, "Id", position, currentY);
            }

            // Always show RequiredAmount
            DrawProperty(property, "RequiredAmount", position, currentY);
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (!property.isExpanded)
            return lineHeight;

        float height = lineHeight + verticalSpacing; // Foldout

        // Add height based on which fields are visible
        if (CurrentCollectByItemTag)
        {
            height += GetPropertyHeight(property, "ItemTag") + verticalSpacing;
        }
        else
        {
            height += GetPropertyHeight(property, "Id") + verticalSpacing;
        }

        height += GetPropertyHeight(property, "RequiredAmount") + verticalSpacing;

        return height;
    }

    private float DrawProperty(SerializedProperty parentProperty, string propertyName, Rect position, float currentY)
    {
        SerializedProperty prop = parentProperty.FindPropertyRelative(propertyName);
        if (prop != null)
        {
            float height = EditorGUI.GetPropertyHeight(prop, true);
            EditorGUI.PropertyField(new Rect(position.x, currentY, position.width, height), prop, true);
            return currentY + height + verticalSpacing;
        }
        return currentY;
    }

    private float GetPropertyHeight(SerializedProperty parentProperty, string propertyName)
    {
        SerializedProperty prop = parentProperty.FindPropertyRelative(propertyName);
        return prop != null ? EditorGUI.GetPropertyHeight(prop, true) : 0;
    }
}

