using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(QuestStepConfig))]
public class QuestStepConfigPropertyDrawer : PropertyDrawer
{
    private const float lineHeight = 18f;
    private const float verticalSpacing = 2f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        //Draw foldout
        property.isExpanded = EditorGUI.Foldout(new Rect(position.x, position.y, position.width, lineHeight),
            property.isExpanded, label, true);

        if (property.isExpanded)
        {
            EditorGUI.indentLevel++;

            float currentY = position.y + lineHeight + verticalSpacing;

            SerializedProperty stepTypeProp = property.FindPropertyRelative("stepType");
            var stepTypeRect = new Rect(position.x, currentY, position.width, lineHeight);
            EditorGUI.PropertyField(stepTypeRect, stepTypeProp);
            currentY += lineHeight + verticalSpacing;

            //Draw conditional fields
            switch ((QuestStepType)stepTypeProp.enumValueIndex)
            {
                case QuestStepType.COLLECT_ITEM:
                    currentY = DrawProperty(property, "itemsToCollect", position, currentY);
                    break;

                case QuestStepType.GO_TO_LOCATION:
                    currentY = DrawProperty(property, "targetPosition", position, currentY);
                    currentY = DrawProperty(property, "triggerRadius", position, currentY);
                    break;
            }

            //Draw common fields
            DrawProperty(property, "description", position, currentY);

            EditorGUI.indentLevel--;
        }

        EditorGUI.EndProperty();
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

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (!property.isExpanded)
            return lineHeight;

        float height = lineHeight + verticalSpacing; //Foldout

        SerializedProperty stepTypeProp = property.FindPropertyRelative("stepType");
        height += lineHeight + verticalSpacing; //stepType

        //Conditional fields height
        switch ((QuestStepType)stepTypeProp.enumValueIndex)
        {
            case QuestStepType.COLLECT_ITEM:
                height += GetPropertyHeight(property, "itemsToCollect") + verticalSpacing;
                break;

            case QuestStepType.GO_TO_LOCATION:
                height += GetPropertyHeight(property, "targetPosition") + verticalSpacing;
                height += GetPropertyHeight(property, "triggerRadius") + verticalSpacing;
                break;
        }

        //Common fields
        height += GetPropertyHeight(property, "description") + verticalSpacing;

        return height;
    }

    private float GetPropertyHeight(SerializedProperty parentProperty, string propertyName)
    {
        SerializedProperty prop = parentProperty.FindPropertyRelative(propertyName);
        return prop != null ? EditorGUI.GetPropertyHeight(prop, true) : 0;
    }
}